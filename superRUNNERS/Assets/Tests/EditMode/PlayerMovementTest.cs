using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTest
{
    [Test]
    public void MoveAlongZAxisForVerticalInput()
    {
        MovementLogic player = new MovementLogic(0, 0, 0, 0, 0, 0);
        var transform = new GameObject().transform;
        transform.rotation = Quaternion.identity;

        Assert.AreEqual(new Vector3(0,0,1), player.CalculateLinearDirection(transform, 1f, 0));
    }

    [Test]
    public void MoveAlongXAxisForHorizontalInput()
    {
        MovementLogic player = new MovementLogic(0, 0, 0, 0, 0, 0);
        var transform = new GameObject().transform;
        transform.rotation = Quaternion.identity;

        Assert.AreEqual(new Vector3(1,0,0), player.CalculateLinearDirection(transform, 0, 1f));
    }

    [Test]
    public void SlideSpeedBoostOnDownwardsSlope()
    {
        float boost = 69;
        MovementLogic player = new MovementLogic(boost, 0, 0, 0, 0, 0);
        Assert.AreEqual(boost, player.CalculateSlideBoost(true, true, -1));
    }

    [Test]
    public void SlideSpeedDecreaseOnAnyOtherSurface()
    {
        float unboost = 69;
        MovementLogic player = new MovementLogic(0, unboost, 0, 0, 0, 0);
        Assert.AreEqual(-unboost, player.CalculateSlideBoost(true, true, 1));
        Assert.AreEqual(-unboost, player.CalculateSlideBoost(true, false, -1));
    }

    [Test]
    public void NoSlideSpeedChangeIfNotSliding()
    {
        float unboost = 69;
        float boost = 96;
        MovementLogic player = new MovementLogic(boost, unboost, 0, 0, 0, 0);
        Assert.AreEqual(0, player.CalculateSlideBoost(false, true, 1));
        Assert.AreEqual(0, player.CalculateSlideBoost(false, true, 0));
        Assert.AreEqual(0, player.CalculateSlideBoost(false, false, 1));
        Assert.AreEqual(0, player.CalculateSlideBoost(false, false, 0));
    }

    [Test]
    public void WalkingSpeedWhenWalkingOnGround()
    {
        float walk = 69;
        MovementLogic player = new MovementLogic(0, 0, walk, 0, 0, 0);
        Assert.AreEqual(walk, player.CalculateSpeed(true, false, false, false, false, 0));
    }

    [Test]
    public void SprintingSpeedWhenSprintingOnGround()
    {
        float sprint = 69;
        MovementLogic player = new MovementLogic(0, 0, 0, sprint, 0, 0);
        Assert.AreEqual(sprint, player.CalculateSpeed(true, true, false, false, false, 0));
    }

    [Test]
    public void WallrunSpeedWhenWallrunning()
    {
        float wallrun = 69;
        MovementLogic player = new MovementLogic(0, 0, 0, 0, 0, wallrun);
        Assert.AreEqual(wallrun, player.CalculateSpeed(false, false, true, false, false, 0));
    }

    [Test]
    public void CrouchSpeedWhenCrouchingOnGround()
    {
        float crouch = 69;
        MovementLogic player = new MovementLogic(0, 0, 0, 0, crouch, 0);
        Assert.AreEqual(crouch, player.CalculateSpeed(true, false, false, true, false, 0));
    }

    [Test]
    public void BaseSpeedUnchangedWhenSliding()
    {
        float currentVelocityWhileSliding = 69;
        MovementLogic player = new MovementLogic(0, 0, 0, 0, 0, currentVelocityWhileSliding);
        Assert.AreEqual(currentVelocityWhileSliding, player.CalculateSpeed(true, false, false, true, true, currentVelocityWhileSliding));
    }

    [Test]
    public void SlidingStateOnlyIfSlidingFasterThanCrouchSpeed()
    {
        float currentVelocity = 69f;
        float crouchSpeed = 10f;
        MovementLogic player = new MovementLogic(0, 0, 0, 0, 0, 0);
        Assert.AreEqual(true, player.CheckSlide(true, currentVelocity, crouchSpeed));
        Assert.AreEqual(false, player.CheckSlide(true, crouchSpeed, crouchSpeed));
        Assert.AreEqual(false, player.CheckSlide(true, crouchSpeed - 1f, crouchSpeed));
    }

    [Test]
    public void CanOnlyJumpWhileOnGround()
    {
        MovementLogic player = new MovementLogic(0, 0, 0, 0, 0, 0);
        Assert.AreEqual(true, player.CheckJump(true, true));
        Assert.AreEqual(false, player.CheckJump(false, false));
        Assert.AreEqual(false, player.CheckJump(true, false));
        Assert.AreEqual(false, player.CheckJump(false, true));
    }
}
