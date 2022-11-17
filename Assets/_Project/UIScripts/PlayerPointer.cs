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
        rectTrans = GetComponent<RectTransform>();

        Debug.Log($"rectTrans == null: {rectTrans == null}");
    }

    private void OnDestroy()
    {
        player.OnPlayerSettingsChanged -= SetPlayerName;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        player.OnPlayerSettingsChanged += SetPlayerName;
        SetPlayerName();
    }

    public void UpdateXPos()
    {
        if (Camera.main == null || player == null) return;
        if (rectTrans == null) rectTrans = GetComponent<RectTransform>();
        if (rectTrans == null) return;

        float canvasXPos = Camera.main.WorldToScreenPoint(player.transform.position).x;
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
}
}