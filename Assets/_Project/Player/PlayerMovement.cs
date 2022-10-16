using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Wichtel.Extensions;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpPower = 8f;
    
    private float horizontalAxis;
    private bool jump;

    private void Start()
    {
        //ignore collision between players - layer 6 needs to be the player layer
        Physics2D.IgnoreLayerCollision(6, 6);
    }

    private void FixedUpdate()
    {
        //only move your own player
        if (!isLocalPlayer) return;
        
        //jump
        if(jump && groundCheck.IsGrounded()) PerformJump();
        //move
        PerformMoveHorizontal();
        
        //reset input
        horizontalAxis = 0f;
        jump = false;
    }

    public void MoveHorizontally(float axis) => horizontalAxis = axis;
    public void Jump() => jump = true;

    private void PerformJump()
    {
        //jump
        rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
        
        //prevent jump boosting
        if (rb.velocity.y > jumpPower) rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    private void PerformMoveHorizontal()
    {
        float horizontalVelocity = horizontalAxis * moveSpeed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);

        Debug.Log($"horizontal velocity: {rb.velocity.x}");
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}