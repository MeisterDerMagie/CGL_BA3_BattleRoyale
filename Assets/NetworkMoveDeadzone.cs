using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;

public class NetworkMoveDeadzone : MonoBehaviour
{
    private Tween tween;
    
    private void Update()
    {
        if (!GameManager.Instance)
            return;
        
        //remove collider if the game is over to prevent the winner from dying
        if (GameManager.Instance.currentState == GameManager.GameState.GameOver)
        {
            var deadZoneCollider = GetComponentInChildren<BoxCollider2D>();
            if (deadZoneCollider == null)
                return;
            else
                Destroy(deadZoneCollider);
        }
        
        if (GameManager.Instance.currentState is
            GameManager.GameState.Preparation or
            GameManager.GameState.Warmup or
            GameManager.GameState.GameOver)
            return;

        float updateInterval = GameManager.Instance.GetComponent<NetworkBehaviour>().syncInterval;
        
        if(tween != null && tween.active) tween.Kill();
        tween = transform.DOMoveY(GameManager.Instance.currentDeadZonePositionY, updateInterval);
    }

    private void OnDestroy()
    {
        tween.Kill();
    }
}
