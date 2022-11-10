﻿//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Doodlenite {
public class PlayerColorPicker : MonoBehaviour
{
    [SerializeField] private Image colorPicker;

    private int slotIndex;
    private NetworkRoomManager roomManager;

    private void Start()
    {
        //get manager
        roomManager = FindObjectOfType<NetworkRoomManager>();
        slotIndex = GetComponentInParent<PlayerLobbySlot>().transform.GetSiblingIndex();
    }

    public void PickColor()
    {
        List<NetworkRoomPlayer> roomPlayers = roomManager.roomSlots;

        foreach (var roomPlayer in roomPlayers)
        {
            if(roomPlayer.index != slotIndex) continue;
            if(!roomPlayer.isLocalPlayer) continue;
            
            var playerData = roomPlayer.GetComponentInChildren<RoomPlayerData>();
            playerData.CmdSetColor(colorPicker.color);
        }
    }
}
}