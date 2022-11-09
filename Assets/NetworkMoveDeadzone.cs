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
        if (GameManager.Instance.currentState == GameManager.GameState.Preparation ||
            GameManager.Instance.currentState == GameManager.GameState.Warmup)
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
