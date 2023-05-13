using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private MoveSettings settings;

    [Header("Movement")]
    public Vector3 flatVel;
    public float moveSpeed;
    public float walkSpeed, sprintSpeed, groundDrag, slideDrag, wallDrag, slideBoost;
    public float jumpMultiplier, jumpCD, airMultiplier;
    bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintHoldKey;
    public KeyCode sprintToggleKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    public bool isGrounded;

    [Header("Wall Check")]
    public bool isOnRightWall;
    public bool isOnLeftWall;
    public LayerMask wall;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    [Header("Slide")]
    public float currentSlideTimer;
    public float slideTimer;

    [Header("Movement States")]
    public bool isSliding;
    public bool isSprinting;
    public bool isCrouching;
    public bool isWallRunning;

    private bool isSprintToggled = false;

    public Transform orientation;
    private string orientationName = "Orientation";

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        InitializeSettings();

        orientation = GameObject.Find(orientationName).GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
        startYScale = transform.localScale.y;
        currentSlideTimer = slideTimer;
        
    }

    private void Update()
    {
        flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground);
        if (!isGrounded)
        {
            isOnRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 0.5f, wall);
            isOnLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.5f, wall);
        }

        MyInput();
        SpeedControl();
        //StateControl();

        // handle drag
        if (isGrounded)
        {
            if (isSliding) rb.drag = slideDrag;
            rb.drag = groundDrag;
        }
        else if (isOnRightWall || isOnLeftWall)
        {
            rb.drag = wallDrag;
        }
        else
        {
            rb.drag = 0;
        }

        // Debug.Log(rb.velocity.magnitude);
    }

    private void InitializeSettings()
    {
        // Walk/Run
        walkSpeed = settings.walkSpeed;
        sprintSpeed = settings.sprintSpeed;
        //groundDrag = settings.groundDrag;
        slideDrag = settings.slideDrag;
        wallDrag = settings.wallDrag;                         //TODO add grounddrag to settings scriptable object
        // Jump
        jumpMultiplier = settings.jumpMulti;
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
        playerHeight = settings.playerHeight;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // WASD inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // jump from ground or wall
        if (canJump && Input.GetKey(jumpKey))
        {
        isSprinting = false;
            if (isGrounded)
            {
                canJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCD);
            }

            else if (isOnLeftWall || isOnRightWall)
            {
                //WallJump();
                canJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCD);
            }
        }

        //crouch from walk or slide from sprint
            if (Input.GetKeyDown(crouchKey))
        {
            Crouch();
        }

        //reset from crouch/slide
        if (Input.GetKeyUp(crouchKey))
        {
            Uncrouch();
            moveSpeed = walkSpeed;
        }

        if (!isCrouching)
        {
            if (Input.GetKeyDown(sprintToggleKey))
            {
                isSprintToggled = !isSprintToggled;
            }
            if ((Input.GetKeyDown(sprintHoldKey) || isSprintToggled))
            {
                isSprinting = true;
                moveSpeed = sprintSpeed;
            }

            if (Input.GetKeyUp(sprintHoldKey) || !isSprintToggled)
            {
                isSprinting = false;
                moveSpeed = walkSpeed;
            }
        }
        
    }

    //TODO lerp function

    private void MovePlayer()
    {
        // movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isSliding)
        {
            if (currentSlideTimer >= slideTimer)
            {
                rb.AddForce(moveDirection.normalized * slideBoost, ForceMode.Impulse);
                currentSlideTimer = 0;
            }
        }

        else
        {
            if (currentSlideTimer < slideTimer)
            {
                currentSlideTimer += 1f * Time.deltaTime;
            }
            if (isGrounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }
    }

private void SpeedControl()
    {

        // Velocity limit
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    private void Jump()
    {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpMultiplier, ForceMode.Impulse);

    }

    /*private void WallJump()
    {

    }
    */

    private void ResetJump()
    {
        canJump = true;
    }

    private void Crouch()
    {
        if ((isSprinting | isSliding) && (flatVel.magnitude > crouchSpeed)) //slide
        {
            isSliding = true;
            moveSpeed = 0;
        }

        else
        {
            isSprinting = false;
            isSprintToggled = false;
            isCrouching = true;
            moveSpeed = crouchSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            if (isGrounded) rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
    }

    private void Uncrouch()
    {
        isCrouching = false;
        isSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }
}
    