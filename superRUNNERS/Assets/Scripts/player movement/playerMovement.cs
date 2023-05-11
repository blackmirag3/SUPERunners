using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jump")]
    public float jumpMultiplier;
    public float jumpCD;
    public float airMultiplier;
    bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public enum MoveState
    {
        walk,
        sprint,
        crouch,
        air
    }
    public MoveState state;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    
    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground);

        MyInput();
        SpeedControl();
        StateControl();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        // Debug.Log(rb.velocity.magnitude);
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

        // jump
        if (Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCD);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateControl()
    {

        if (Input.GetKey(crouchKey))
        {
            state = MoveState.crouch;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MoveState.sprint;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MoveState.walk;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MoveState.air;
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
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        } 
        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
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

    private void ResetJump()
    {
        canJump = true;
    }


}
