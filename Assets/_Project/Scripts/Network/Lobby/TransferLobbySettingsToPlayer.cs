using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using Mirror;
using UnityEngine;

public class TransferLobbySettingsToPlayer : NetworkBehaviour
{
    [SerializeField] private Player player;
    private RoomPlayerData roomPlayerData;

    private NetworkRoomManager manager;
    
    //Tell the server to sync the local players settings
    public override void OnStartClient()
    {
        //do nothing if you're not the local player
        if (!isLocalPlayer) return;

        //get the manager
        if(manager == null) manager = FindObjectOfType<NetworkRoomManager>();

        //throw error if no manager was found
        if (manager == null)
        {
            Debug.LogWarning("Couldn't find a NetworkRoomManager. No lobby settings will be transferred to the player.");
            return;
        }
        
        //Transfer settings
        foreach (var roomPlayer in manager.roomSlots)
        {
            if (!roomPlayer.isLocalPlayer) continue;
            
            TransferSettings(roomPlayer.index);
        }
    }

    [Command(requiresAuthority = false)]
    private void TransferSettings(int _roomPlayerIndex)
    {
        var roomPlayer = GetRoomPlayerByIndex(_roomPlayerIndex);

        if (roomPlayer == null)
        {
            Debug.LogWarning($"Could not find NetworkRoomPlayer with index {_roomPlayerIndex}. Will not transfer any lobby settings to the player.");
            return;
        }
        
        roomPlayerData = roomPlayer.GetComponent<RoomPlayerData>();
        
        player.playerColor = roomPlayerData.PlayerColor;
        player.playerName = roomPlayerData.PlayerName;
    }

    private NetworkRoomPlayer GetRoomPlayerByIndex(int _index)
    {
        if(manager == null) manager = FindObjectOfType<NetworkRoomManager>();
        
        foreach (var roomPlayer in manager.roomSlots)
        {
            if (roomPlayer.index == _index) return roomPlayer;
        }

        return null;
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        player = GetComponent<Player>();
        if(player == null) Debug.LogError("Can't find player script. Please put this script on the same game object as the player script.", this);
    }
    #endif
}
