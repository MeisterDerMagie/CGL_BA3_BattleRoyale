using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Wichtel.Extensions;

public class PlayerScreenEdgeTeleport : NetworkBehaviour
{
    [SyncVar(hook = nameof(Teleport))]
    private float playerXPosition;

    [SyncVar] private bool isDirty;

    private Camera camera;
    
    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        //if Server: check if teleport
        if (!isServer) return;
        
        float cameraHeight = camera.orthographicSize * 2.0f;
        float cameraWidth = cameraHeight * camera.aspect;

        float cameraBorderLeft = camera.transform.position.x - cameraWidth / 2f;
        float cameraBorderRight = camera.transform.position.x + cameraWidth / 2f;

        if (transform.position.x < cameraBorderLeft)
        {
            playerXPosition = cameraBorderRight;
            isDirty = true;
        }

        if (transform.position.x > cameraBorderRight)
        {
            playerXPosition = cameraBorderLeft;
            isDirty = true;
        }

        Teleport();
    }

    private void Teleport(float oldValue, float newValue) => Teleport();

    private void Teleport()
    {
        if (!isDirty) return;
        
        transform.position = transform.position.With(x: playerXPosition);
        isDirty = false;
    }
}
