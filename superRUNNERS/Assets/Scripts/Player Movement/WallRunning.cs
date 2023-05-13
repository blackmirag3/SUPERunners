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

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                Debug.Log("Wall run on left");
                WallRun();
            }
            else if (wallRight)
            {
                Debug.Log("Wall run on right");
                WallRun();
            }
            else
                ExitWallRun();
        }
        else
        {
            ExitWallRun();
        }
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

    private void WallRun()
    {
        playerMovement.isWallRunning = true;
        rb.useGravity = false;

    }

    private void ExitWallRun()
    {
        playerMovement.isWallRunning = false;
        rb.useGravity = true;
    }
}
