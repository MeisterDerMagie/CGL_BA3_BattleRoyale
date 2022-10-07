using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float speed = 8f;
    
    private void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        rigidbody.MovePosition((Vector2)transform.position + (input * speed * Time.deltaTime));
    }

    private void OnValidate()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
}
