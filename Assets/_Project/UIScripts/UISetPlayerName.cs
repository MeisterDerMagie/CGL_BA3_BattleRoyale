//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Doodlenite {
public class UISetPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private int slotIndex;
    private NetworkRoomManager roomManager;

    private void Start()
    {
        //get manager
        roomManager = FindObjectOfType<NetworkRoomManager>();
        slotIndex = GetComponentInParent<PlayerLobbySlot>().transform.GetSiblingIndex();
    }

    public void ConfirmPlayeName()
    {
        List<NetworkRoomPlayer> roomPlayers = roomManager.roomSlots;

        foreach (var roomPlayer in roomPlayers)
        {
            if(roomPlayer.index != slotIndex) continue;
            if(!roomPlayer.isLocalPlayer) continue;
            
            var playerData = roomPlayer.GetComponentInChildren<RoomPlayerData>();
            playerData.CmdSetPlayerName(inputField.text);
        }
    }
}
}