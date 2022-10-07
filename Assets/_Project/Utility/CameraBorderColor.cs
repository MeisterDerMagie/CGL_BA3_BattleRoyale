using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorderColor : MonoBehaviour
{
    void OnDrawGizmos()
    {
        float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;
	
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(verticalWidthSeen, verticalHeightSeen, 0));
    }
}
