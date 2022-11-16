//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;
using Wichtel.Extensions;

namespace Doodlenite {
public class Deadzone : NetworkBehaviour
{
    [SyncVar(hook = nameof(Move))]
    private float yPos;

    [SerializeField] private float speed = 1f;
    [SerializeField] private BoxCollider2D deadZoneCollider;
    
    private bool move;
    private Tween tween;
    
    private void Awake()
    {
        yPos = transform.position.y;
    }

    public void BeginMovement()
    {
        if (!isServer) return;
        move = true;
    }

    public void StopAndDestroyCollider()
    {
        move = false;
        if (deadZoneCollider == null) return;
        Destroy(deadZoneCollider);
    }
    
    private void Update()
    {
        if (!isServer || !move) return;

        yPos = transform.position.y + Game.Instance.Difficulty * speed * Time.deltaTime;
        transform.position = transform.position.With(y: yPos);
    }

    private void Move(float oldYPos, float newYPos)
    {
        float updateInterval = Game.Instance.GetComponent<NetworkBehaviour>().syncInterval;
        
        if(tween != null && tween.active) tween.Kill();
        tween = transform.DOMoveY(yPos, updateInterval);
    }
    
    private void OnDestroy()
    {
        tween.Kill();
    }
}
}