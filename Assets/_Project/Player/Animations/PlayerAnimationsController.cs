//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite {
public class PlayerAnimationsController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private PlayerAnimations anims;

    [SerializeField] private float walkingSpeedModifier = 1f;
    
    private StateMachine stateMachine;

    private Vector2 playerLastPosition;
    private Vector2 playerDeltaPosition;

    private bool wasGroundedBefore;
    private bool isGrounded;

    private MovementDirection movementDirection;
    
    private bool isDead;

    public float DEBUG_walkingSpeed;
    
    //States
    private PlayerAnimationState_Idle idle;
    private PlayerAnimationState_WalkingLeft walkingLeft;
    private PlayerAnimationState_WalkingRight walkingRight;
    private PlayerAnimationState_InAirRising inAirRising;
    private PlayerAnimationState_InAirFalling inAirFalling;
    
    private void Start()
    {
        stateMachine = new StateMachine();
        
        //set up states
        idle = new PlayerAnimationState_Idle(anims);
        walkingLeft = new PlayerAnimationState_WalkingLeft(anims);
        walkingRight = new PlayerAnimationState_WalkingRight(anims);
        inAirRising = new PlayerAnimationState_InAirRising(anims);
        inAirFalling = new PlayerAnimationState_InAirFalling(anims);
        
        //transitions
        stateMachine.AddAnyTransition(inAirRising, IsInAirRising);
        stateMachine.AddAnyTransition(inAirFalling, IsInAirFalling);
        stateMachine.AddAnyTransition(idle, IsIdle);
        stateMachine.AddAnyTransition(walkingLeft, IsWalkingLeft);
        stateMachine.AddAnyTransition(walkingRight, IsWalkingRight);
    }

    private void FixedUpdate()
    {
        playerDeltaPosition = (Vector2)player.position - playerLastPosition;
        isGrounded = groundCheck.IsGrounded();
        
        //calculate the movementDirection
        CalculateMovementDirection();
        
        //walking speed
        float walkingSpeed = Mathf.Abs(playerDeltaPosition.x) / Time.deltaTime * walkingSpeedModifier;

        walkingLeft.walkingSpeed = walkingSpeed;
        walkingRight.walkingSpeed = walkingSpeed;

        DEBUG_walkingSpeed = walkingSpeed;

        //should the landing animation be played?
        idle.playLandingAnimation = IsLanding();
        
        //Tick Statemachine
        stateMachine.Tick();
        
        //values before
        wasGroundedBefore = isGrounded;
        playerLastPosition = player.position;
    }

    
    //--- Conditions ---
    
    private bool IsInAirRising()
    {
        //if the player is dead, he is not jumping
        if (isDead) return false;
        
        //if the player is on the ground, he is not jumping
        if (isGrounded == true) return false;
        //if the player is falling down, he is not rising
        if (playerDeltaPosition.y <= 0f) return false;

        //otherwise he is in the air and rising
        return true;
    }

    private bool IsInAirFalling()
    {
        //if the player is dead, he is not jumping
        if (isDead) return false;
        
        //if the player is on the ground, he is not in the air
        if (isGrounded == true) return false;
        //if the player is rising, he is not falling down
        if (playerDeltaPosition.y > 0f) return false;

        //otherwise he is falling down
        return true;
    }

    private bool IsIdle()
    {
        //if the player is dead, he is not idle
        if (isDead) return false;
        //if the player is on the ground and he did not move horizontally, he is idle
        return (isGrounded && (movementDirection == MovementDirection.NoMovement));
    }

    private bool IsWalkingLeft()
    {
        if (isDead) return false;
        
        if (!isGrounded) return false;
        if (movementDirection != MovementDirection.Left) return false;

        return true;
    }

    private bool IsWalkingRight()
    {
        if (isDead) return false;
        
        if (!isGrounded) return false;
        if (movementDirection != MovementDirection.Right) return false;

        return true;
    }

    private bool IsLanding()
    {
        if (isDead) return false;

        if (!isGrounded) return false;
        if (wasGroundedBefore) return false;
        if (IsWalkingLeft()) return false;
        if (IsWalkingRight()) return false;
        
        return true;
    }

    private void CalculateMovementDirection()
    {
        //no movement
        if (playerDeltaPosition.x is > -0.01f and < 0.01f) movementDirection = MovementDirection.NoMovement;
        if (playerDeltaPosition.x <= -0.01f) movementDirection = MovementDirection.Left;
        if (playerDeltaPosition.x >= 0.01f) movementDirection = MovementDirection.Right;
    }

    private enum MovementDirection
    {
        NoMovement,
        Left,
        Right
    }
}
}