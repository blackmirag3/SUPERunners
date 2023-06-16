using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    public Transform orientation;
    public PlayerMovement playerMovement;

    [Header("Distance from wall to detect")]
    public float wallDist;
    public LayerMask wallMask;
    public float minJumpHeight;


    RaycastHit wallRightHit, wallLeftHit;
    bool wallRight, wallLeft;
    bool wallJumping = false;
    bool wallLatching = false;
    Vector3 wallNormal;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode latchKey = KeyCode.LeftControl;
    private Rigidbody rb;

    float horizontalInput;
    float verticalInput;

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

    public float Tilt { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseFov = playerCam.fieldOfView;
    }

    void Update()
    {
        CheckWall();
        MoveKeyInputs();

        if ((wallLeft || wallRight) && CanWallRun() && verticalInput > 0 && !wallJumping && !wallLatching)
        {

            EnableWallRun();
            if (Input.GetKeyDown(jumpKey))
            {
                onPlayerAction.CallEvent(this, jumpActionDur);
                WallJump();
                ExitWallRun();
            }

            if (Input.GetKeyDown(latchKey))
            {
                WallLatch();
            }

        }
        else if (wallLatching)
        {
            Tilt = Mathf.Lerp(Tilt, 0, camTiltTime * Time.deltaTime);

            if (Input.GetKeyDown(jumpKey))
            {
                onPlayerAction.CallEvent(this, jumpActionDur);
                WallLatchJump();
                WallDetach();
                ExitWallRun();
            }

            if (Input.GetKeyUp(latchKey))
            {
                WallDetach();
            }
        }
        else
        {
            ExitWallRun();
        }
    }

    private void FixedUpdate()
    {
        if (playerMovement.isWallRunning && !wallJumping && !wallLatching)
        {
            WallRun();
        }
    }

    private void MoveKeyInputs()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
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
        if (!playerMovement.isWallRunning)
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        playerMovement.isWallRunning = true;
        rb.useGravity = false;

        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);
        if (wallLeft)
        {
            Tilt = Mathf.Lerp(Tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            Tilt = Mathf.Lerp(Tilt, camTilt, camTiltTime * Time.deltaTime);
        }
    }

    private void ExitWallRun()
    {
        playerMovement.isWallRunning = false;
        if (!playerMovement.onSlope)
        {
            rb.useGravity = true;
        }

        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, baseFov, wallRunfovTime * Time.deltaTime);
        Tilt = Mathf.Lerp(Tilt, 0, camTiltTime * Time.deltaTime);
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
}
