using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private float rayCastLength = 0.8f;
    
    private float horizontalAxis;
    private bool jump;
    
    private void FixedUpdate()
    {
        if(jump && IsGrounded()) PerformJump();
        if(horizontalAxis != 0f) PerformMoveHorizontal();
        
        //reset input
        horizontalAxis = 0f;
        jump = false;
    }

    public void MoveHorizontally(float axis) => horizontalAxis = axis;
    public void Jump() => jump = true;

    private void PerformJump()
    {
        Debug.Log("Jump!");
        rb.AddForce(new Vector2(0f, jumpPower));
    }

    private void PerformMoveHorizontal()
    {
        float velocity = horizontalAxis * moveSpeed;
        rb.velocity = new Vector2(velocity, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        Debug.LogError("This method doesn't work properly. Needs overhaul.");
        
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, rayCastLength);
        
        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - rayCastLength), Color.magenta, 3f, false);
        
        bool isGrounded = groundCheck.collider != null && groundCheck.collider.CompareTag("platform");
        Debug.Log("isGrounded = " + isGrounded);

        return isGrounded;
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}