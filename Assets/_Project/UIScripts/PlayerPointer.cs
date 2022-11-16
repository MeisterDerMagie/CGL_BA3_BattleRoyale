using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Wichtel.Extensions;

namespace Doodlenite {
public class PlayerPointer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Transform danger1, danger2, dead;
    private Player player;
    private float xPos;
    private RectTransform rectTrans;

    private void Start()
    {
        SetNoDangerAndAlive();
        player.OnPlayerSettingsChanged += SetPlayerName;
        rectTrans = GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        player.OnPlayerSettingsChanged -= SetPlayerName;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        SetPlayerName();
    }

    public void UpdateXPos()
    {
        float canvasXPos = WorldSpaceToCanvas(rectTrans, Camera.main, player.transform.position).x;
        rectTrans.anchoredPosition = rectTrans.anchoredPosition.With(x: canvasXPos);
    }
    
    public void SetPlayerName()
    {
        if (player == null) return;
        playerNameText.SetText(player.playerName);
    }

    public void SetNoDangerAndAlive()
    {
        danger1.gameObject.SetActive(false);
        danger2.gameObject.SetActive(false);
        dead.gameObject.SetActive(false);
    }

    public void SetDanger1()
    {
        if(!danger1.gameObject.activeSelf) danger1.gameObject.SetActive(true);
        danger2.gameObject.SetActive(false);
        dead.gameObject.SetActive(false);
    }

    public void SetDanger2()
    {
        danger1.gameObject.SetActive(false);
        if(!danger2.gameObject.activeSelf) danger2.gameObject.SetActive(true);
        dead.gameObject.SetActive(false);
    }

    public void SetDead()
    {
        danger1.gameObject.SetActive(false);
        danger2.gameObject.SetActive(false);
        if(!dead.gameObject.activeSelf)dead.gameObject.SetActive(true);
    }

    public static Vector2 WorldSpaceToCanvas(RectTransform canvasRect, Camera camera, Vector3 worldPos)
    {
        Vector2 viewportPosition= camera.WorldToViewportPoint(worldPos);
        Vector2 canvasPos = new Vector2
        (
            (
                (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x*0.5f) 
            ),
            (
                (viewportPosition.y* canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f)
            )
        );
 
        return canvasPos;
    }
}
}