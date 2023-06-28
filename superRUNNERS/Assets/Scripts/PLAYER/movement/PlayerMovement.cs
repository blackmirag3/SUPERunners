using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MoveSettings settings;

    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    public float walkSpeed, sprintSpeed, wallRunSpeed, groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float wallJumpUpForce, wallJumpSideForce;
    [SerializeField]
    private float jumpCD, airMultiplier, jumpActionDur;
    private bool canJump;

    [Header("Crouching")]
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float crouchYScale;
    private float startYScale;

    [Header("Sliding")]
    [SerializeField]
    private float slideSpeedIncrease;
    [SerializeField]
    private float slideSlopeIncrease;
    [SerializeField]
    private float slideSpeedDecrease;

    [Header("Movement States")]
    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private Vector3 currVelocity;
    [SerializeField]
    private float currVelocityMagnitude;
    public bool isGrounded;
    public bool isSprinting;
    public bool isCrouching;
    public bool isSliding;
    public bool isWallRunning;
    public bool hasMovementInputs;
    public bool onSlope;

    public float horizontalInput;
    public float verticalInput;

    private Rigidbody rb;

    private RaycastHit slopeHit;

    public GameEvent onPlayerAction;

    private bool isPaused;

    [SerializeField]
    private PlayerInput inputs = null;
    private InputAction moveInput = null;
    private InputAction jumpInput = null;
    private InputAction crouchInput = null;

    private void Awake()
    {
        inputs = InputManager.instance.PlayerInput;
        moveInput = inputs.actions["Move"];
        jumpInput = inputs.actions["Jump"];
        crouchInput = inputs.actions["Crouch"];

        jumpInput.performed += CallJump;
        crouchInput.performed += Crouch;
        crouchInput.canceled += Uncrouch;
    }

    private void Start()
    {
        InitializeSettings();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;

        startYScale = transform.localScale.y;
        isSprinting = true;

        isPaused = false;
    }

    private void InitializeSettings()
    {
        // Walk/Run
        walkSpeed = settings.walkSpeed;
        sprintSpeed = settings.sprintSpeed;
        groundDrag = settings.groundDrag;
        //TODO add grounddrag to settings scriptable object
        // Jump
        jumpForce = settings.jumpMulti;
        jumpCD = settings.jumpCD;
        airMultiplier = settings.airMulti;
        // Crouch
        crouchSpeed = settings.crouchSpeed;
        crouchYScale = settings.yScale;
        // Misc
    }


    private void Update()
    {
        currVelocityMagnitude = currVelocity.magnitude;
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask, QueryTriggerInteraction.Ignore);

        // Find curr vel
        currVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        GetBasicMovementInputs();
        CheckSlope();
        GravityControl();
        ControlDrag();
        SpeedLimit();
        SpeedControl();

    }

    private void FixedUpdate()
    {
        MovePlayer();
        // Decrease move speed if sliding
        if (isSliding)
        {
            if (onSlope && rb.velocity.y < 0)
            {
                moveSpeed += slideSlopeIncrease;
            }
            else
            {
                moveSpeed -= slideSpeedDecrease;
            }
        }
    }

    private void CallJump(InputAction.CallbackContext ctx)
    {
        if (!isPaused && canJump && isGrounded)
        {
            onPlayerAction.CallEvent(this, jumpActionDur);
            canJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCD);
        }
    }

    private void ControlDrag()
    {
        // handle drag
        if (isGrounded || isWallRunning)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 1f;
        }
    }

    private void GetBasicMovementInputs()
    {
        Vector2 moveVal = moveInput.ReadValue<Vector2>();
        horizontalInput = moveVal.x;
        verticalInput = moveVal.y;
        hasMovementInputs = (horizontalInput != 0) || (verticalInput != 0);
    }

    private void SpeedControl()
    {
        if (!isCrouching)
        {
            if (isGrounded && isSprinting)
            {
                moveSpeed = sprintSpeed;
            }
            else if (isGrounded)
            {
                moveSpeed = walkSpeed;
            }
            else if (isWallRunning)
            {
                moveSpeed = wallRunSpeed;
            }
        }
        else if (isSliding)
        {

            if (currVelocity.magnitude <= crouchSpeed)
                isSliding = false;
        }
        else if (isGrounded && isCrouching)
        {
            moveSpeed = crouchSpeed;
        }

    }

    private void MovePlayer()
    {
        // movement direction   
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (onSlope)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        }

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!isWallRunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedLimit()
    {

        // Velocity limit
        if (currVelocity.magnitude > moveSpeed)
        {
            Vector3 limitVel = currVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    private void Jump()
    {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void Crouch(InputAction.CallbackContext ctx)
    {
        if (isPaused)
        {
            return;
        }

        isCrouching = true;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        if (isGrounded && currVelocity.magnitude > crouchSpeed) //(currVelocity.magnitude - 0.5f) > walkSpeed)
        {
            isSliding = true;
            moveSpeed += slideSpeedIncrease;
        }
    }

    private void Uncrouch(InputAction.CallbackContext ctx)
    {
        isCrouching = false;
        isSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }

    private void CheckSlope()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, out slopeHit, 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                onSlope = true;
                return;
            }
        }

        onSlope = false;
    }

    private void GravityControl()
    {
        if (onSlope && !hasMovementInputs)
        {
            rb.useGravity = false;
        }
        else if (!isWallRunning)
        {
            rb.useGravity = true;
        }
    }

    private void OnDisable()
    {
        jumpInput.performed -= CallJump;
        crouchInput.performed -= Crouch;
        crouchInput.canceled -= Uncrouch;
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
