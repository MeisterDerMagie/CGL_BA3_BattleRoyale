using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Wichtel.Extensions;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private Vector2 groundedBoxCastSize;
    
    private float horizontalAxis;
    private bool jump;
    
    private void FixedUpdate()
    {
        if(jump && IsGrounded()) PerformJump();
        PerformMoveHorizontal();
        
        //reset input
        horizontalAxis = 0f;
        jump = false;
    }

    public void MoveHorizontally(float axis) => horizontalAxis = axis;
    public void Jump() => jump = true;

    private void PerformJump()
    {
        rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
    }

    private void PerformMoveHorizontal()
    {
        float horizontalVelocity = horizontalAxis * moveSpeed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        int layerMask = LayerMask.GetMask("Platforms");
        //RaycastHit2D groundCheck = Physics2D.Raycast(transform.position.With(y: transform.position.y + 0.7f), Vector2.down, rayCastLength, layerMask);
        var groundCheck = Physics2D.BoxCast(transform.position.With(y: transform.position.y + 0.1f),  new Vector2(groundedBoxCastSize.x, groundedBoxCastSize.y),  0f,Vector2.down, 0f, layerMask: layerMask);

        bool isGrounded = groundCheck.collider != null;// && groundCheck.collider.CompareTag("platform");

        return isGrounded;
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}