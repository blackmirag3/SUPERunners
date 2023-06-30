using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallRunning : MonoBehaviour
{

    public Transform orientation;
    public PlayerMovement playerMovement;

    [Header("Distance from wall to detect")]
    public float wallDist;
    public LayerMask wallMask;
    public float minJumpHeight;

    RaycastHit wallRightHit, wallLeftHit;
    private bool wallRight, wallLeft;
    private bool wallJumping = false;
    private bool wallLatching = false;
    private bool wallRunning = false;
    Vector3 wallNormal;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode latchKey = KeyCode.LeftControl;
    private Rigidbody rb;

    private float verticalInput;

    [Header("Camera")]
    public Camera playerCam;
    private float baseFov;
    [SerializeField] private float wallRunfov;
    [SerializeField] private float wallRunfovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    [Header("Player action event")]
    public GameEvent onPlayerAction;
    [SerializeField] private float jumpActionDur;

    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    private bool isPaused;

    public bool defaultHandActive { get; private set; } //default hand is the right hand

    public float Tilt { get; private set; }

    private PlayerInput inputs = null;
    private InputAction moveInput = null;
    private InputAction jumpInput = null;
    private InputAction latchInput = null;

    private void Awake()
    {
        inputs = InputManager.instance.PlayerInput;
        moveInput = InputManager.instance.PlayerInput.actions["Move"];
        jumpInput = InputManager.instance.PlayerInput.actions["Jump"];
        latchInput = InputManager.instance.PlayerInput.actions["Crouch"];

        jumpInput.performed += JumpCall;
        latchInput.performed += Latch;
        latchInput.canceled += Unlatch;
    }

    private void OnDisable()
    {
        jumpInput.performed -= JumpCall;
        latchInput.performed -= Latch;
        latchInput.canceled -= Unlatch;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseFov = playerCam.fieldOfView;
        defaultHandActive = true;

        isPaused = false;
    }

    private void JumpCall(InputAction.CallbackContext ctx)
    {
        if (!isPaused && wallRunning)
        {
            onPlayerAction.CallEvent(this, jumpActionDur);
            if (wallLatching)
            {
                WallLatchJump();
                WallDetach();
                ExitWallRun();
                return;
            }
            WallJump();
            ExitWallRun();
        }

    }

    private void Latch(InputAction.CallbackContext ctx)
    {
        if (!isPaused && wallRunning)
        {
            WallLatch();
        }
    }

    private void Unlatch(InputAction.CallbackContext ctx)
    {
        if (wallLatching)
        {
            WallDetach();
        }
    }

    private void Update()
    {
        CheckWall();
        MoveKeyInputs();

        if ((wallLeft || wallRight) && CanWallRun() && verticalInput > 0 && !wallJumping && !wallLatching)
        {
            EnableWallRun();
        }
        else if (wallLatching)
        {
            Tilt = Mathf.Lerp(Tilt, 0, camTiltTime * Time.deltaTime);
        }
        else
        {
            ExitWallRun();
        }
    }

    private void FixedUpdate()
    {
        if (wallRunning && !wallJumping && !wallLatching)
        {
            WallRun();
        }
    }

    private void MoveKeyInputs()
    {
        verticalInput = moveInput.ReadValue<Vector2>().y;
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight);
    }

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out wallLeftHit, wallDist, wallMask);
        wallRight = Physics.Raycast(transform.position, orientation.right, out wallRightHit, wallDist, wallMask);
    }

    private void EnableWallRun()
    {
        // reset y velocity on wall run start
        if (!wallRunning)
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        playerMovement.isWallRunning = true;
        wallRunning = true;
        rb.useGravity = false;

        //playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);
        if (wallLeft)
        {
            Tilt = Mathf.Lerp(Tilt, -camTilt, camTiltTime * Time.deltaTime);
            SetRightHand();
        }
        else if (wallRight)
        {
            Tilt = Mathf.Lerp(Tilt, camTilt, camTiltTime * Time.deltaTime);
            SetLeftHand();
        }
    }

    private void ExitWallRun()
    {
        playerMovement.isWallRunning = false;
        wallRunning = false;
        if (!playerMovement.onSlope)
        {
            rb.useGravity = true;
        }

        //playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, baseFov, wallRunfovTime * Time.deltaTime);
        Tilt = Mathf.Lerp(Tilt, 0, camTiltTime * Time.deltaTime);
        SetRightHand();
    }

    private void CalcWallNormal()
    {
        if (wallLeft)
        {
            wallNormal = wallLeftHit.normal;
        }
        else if (wallRight)
        {
            wallNormal = wallRightHit.normal;
        }
    }

    private void WallRun()
    {
        CalcWallNormal();
        Vector3 runDir = Vector3.Cross(wallNormal, transform.up);
        if (Vector3.Dot(runDir, orientation.forward) < 0)
        {
            runDir = -runDir;
        }

        rb.AddForce(10f * playerMovement.wallRunSpeed * runDir.normalized, ForceMode.Force);
    }

    private void WallJump()
    {
        wallJumping = true;
        CalcWallNormal();
        Vector3 wallJumpDirForce = wallNormal * playerMovement.wallJumpSideForce + transform.up * playerMovement.wallJumpUpForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(wallJumpDirForce, ForceMode.Impulse);
        Invoke(nameof(ResetJump), 0.5f);
    }

    private void ResetJump()
    {
        wallJumping = false;
    }

    private void WallLatch()
    {
        wallLatching = true;
    }

    private void WallDetach()
    {
        wallLatching = false;
    }

    private void WallLatchJump()
    {
        wallJumping = true;
        Vector3 wallJumpDirForce = orientation.forward * playerMovement.wallJumpSideForce + orientation.up * playerMovement.wallJumpUpForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(wallJumpDirForce, ForceMode.Impulse);
        Invoke(nameof(ResetJump), 0.5f);
    }

    private void SetLeftHand()
    {
        if (!defaultHandActive)
        {
            return;
        }
        leftHand.SetActive(true);
        defaultHandActive = false;

        IHoldable item = rightHand.GetComponentInChildren<IHoldable>();
        if (item != null)
        {
            item.SetItemInHand(leftHand.transform);
        }
        rightHand.SetActive(false);
    }

    private void SetRightHand()
    {
        if (defaultHandActive)
        {
            return;
        }
        rightHand.SetActive(true);
        defaultHandActive = true;

        IHoldable item = leftHand.GetComponentInChildren<IHoldable>();
        if (item != null)
        {
            item.SetItemInHand(rightHand.transform);
        }
        leftHand.SetActive(false);
    }

    public void PauseCalled(Component sender, object data)
    {
        if (data is bool)
        {
            isPaused = (bool)data;
            return;
        }
        Debug.Log($"Unwanted event call from {sender}");
    }
}
