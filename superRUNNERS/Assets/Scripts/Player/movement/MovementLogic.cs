using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLogic //handles logic and speed for movement states 
{
    private float slideSlopeIncrease;
    private float slideSpeedDecrease;
    private float walkSpeed;
    private float sprintSpeed;
    private float crouchSpeed;
    private float wallRunSpeed;

    public MovementLogic(float slopeBoost, float slopeUnboost, float walk, float sprint, float crouch, float wall)
    {
        slideSlopeIncrease = slopeBoost;
        slideSpeedDecrease = slopeUnboost;
        walkSpeed = walk;
        sprintSpeed = sprint;
        crouchSpeed = crouch;
        wallRunSpeed = wall;
    }
    
    public float CalculateSlideBoost(bool slideState, bool slopeState, float verticalVelocity)
    {
        if (slideState)
        {
            if (slopeState && verticalVelocity < 0)
            {
                return slideSlopeIncrease;
            }
            return -slideSpeedDecrease;
        }
        return 0f;
    }

    public float CalculateSpeed(bool groundState, bool sprintState, bool wallrunState, bool crouchState, bool slideState, float previousSpeed)
    {
        if (!crouchState)
        {
            if (groundState && sprintState)
            {
                return sprintSpeed;
            }
            else if (groundState)
            {
                return walkSpeed;
            }
            else if (wallrunState)
            {
                return wallRunSpeed;
            }
        }

        else if (!slideState && groundState && crouchState)
        {
            return crouchSpeed;
        }

        return previousSpeed;
    }

    public bool CheckSlide(bool slideState, float currentVelocity, float crouchSpeed)
    {
        return (slideState && (currentVelocity > crouchSpeed)); //returns true if sliding AND current speed > crouch speed
    }

    public bool CheckJump(bool canJump, bool groundState)
    {
        return (canJump && groundState);
    }

    public bool EnableSlide(bool groundState, float currentVelocity)
    {
        return (groundState && currentVelocity > crouchSpeed);
    }

    public Vector3 CalculateLinearDirection(Transform orientation, float verticalInput, float horizontalInput)
    {
        return (orientation.forward * verticalInput + orientation.right * horizontalInput);
    }
}
