//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wichtel.Extensions;

namespace Doodlenite {
public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Vector2 groundedBoxCastSize;
    
    public bool IsGrounded()
    {
        int layerMask = LayerMask.GetMask("Platforms");
        var groundCheck = Physics2D.BoxCast(transform.position.With(y: transform.position.y + 0.1f),  new Vector2(groundedBoxCastSize.x, groundedBoxCastSize.y),  0f,Vector2.down, 0f, layerMask: layerMask);

        bool isGrounded = groundCheck.collider != null;

        return isGrounded;
    }
}
}