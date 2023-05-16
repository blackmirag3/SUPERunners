using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MoveSettings settings;

    public Transform orientation;
    public Transform groundCheck;
    public LayerMask groundMask;

    //[Header("Keybinds")]
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode sprintHoldKey = KeyCode.LeftShift;
    private KeyCode sprintToggleKey = KeyCode.LeftAlt;
    private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    public float walkSpeed, sprintSpeed, wallRunSpeed, groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCD, airMultiplier, wallJumpUpForce, wallJumpSideForce;
    private bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Sliding")]
    public float slideSpeedIncrease;
    public float slideSpeedDecrease;

    [Header("Movement States")]
    public Vector3 moveDirection;
    private Vector3 currVelocity;
    public float currVelocityMagnitude;
    public bool grounded;
    public bool isSprinting;// = false;
    public bool isCrouching;
    public bool isSliding;
    public bool isWallRunning;// = false;
    public bool isWASD;

    float horizontalInput;
    float verticalInput;

    Rigidbody rb;

    private void Start()
    {
        InitializeSettings();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;

        startYScale = transform.localScale.y;
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
        // Move keybinds
        jumpKey = settings.jumpKey;
        sprintToggleKey = settings.sprintToggleKey;
        sprintHoldKey = settings.sprintHoldKey;
        crouchKey = settings.crouchKey;
        // Misc
    }


    private void Update()
    {
        // Ground check
        grounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);

        // Find curr vel
        currVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        currVelocityMagnitude = currVelocity.magnitude;

        MyInput();
        ControlDrag();
        SpeedLimit();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        // Decrease move speed if sliding
        if (isSliding)
            moveSpeed -= slideSpeedDecrease;
    }

    private void ControlDrag()
    {
        // handle drag
        if (grounded || isWallRunning)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 1f;
        }
    }

    private void MyInput()
    {
        // WASD inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isWASD = (horizontalInput + verticalInput != 0);

        // Jump
        if (Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCD);
        }
        // Crouching and sliding
        if (Input.GetKeyDown(crouchKey) && !isWallRunning)
        {
            Crouch();
        }
        if (Input.GetKeyUp(crouchKey))
        {
            Uncrouch();
        }
        // Sprinting
        if (Input.GetKeyDown(sprintToggleKey))
        {
            isSprinting ^= true;
        }
        if (Input.GetKeyDown(sprintHoldKey))
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(sprintHoldKey))
        {
            isSprinting = false;
        }
    }

    private void SpeedControl()
    {
        if (!isCrouching)
        {
            if (grounded && isSprinting)
            {
                moveSpeed = sprintSpeed;
            }
            else if (grounded)
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

            if (currVelocityMagnitude <= walkSpeed)
                isSliding = false;
        }
        else if (grounded && isCrouching)
        {
            moveSpeed = crouchSpeed;
        }

    }

    private void MovePlayer()
    {
        // movement direction   
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


        if (grounded)
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
        if (currVelocityMagnitude > moveSpeed)
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

    private void Crouch()
    {
        isCrouching = true;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        Debug.Log("Crouching");
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        if (grounded && (currVelocityMagnitude - 0.5f) > walkSpeed)
        {
            isSliding = true;
            moveSpeed += slideSpeedIncrease;
            Debug.Log($"Sliding Called, moveSpeed: {moveSpeed}");
        }
    }

    private void Uncrouch()
    {
        isCrouching = false;
        isSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

    }
}
