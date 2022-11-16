using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using Mirror;
using UnityEngine;

public class TransferLobbySettingsToServer : NetworkBehaviour
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

        //Transfer settings to server
        foreach (var roomPlayer in manager.roomSlots)
        {
            Debug.Log($"OnStartClient(): roomPlayer.isLocalPlayer = {roomPlayer.isLocalPlayer}");

            if (!roomPlayer.isLocalPlayer) continue;

            var roomPlayerData = roomPlayer.GetComponent<RoomPlayerData>();
            var data = new PlayerCustomizableData(roomPlayerData.PlayerName, roomPlayerData.PlayerColor);
            uint playerNetId = player.netId;
            TransferSettings(playerNetId, data);
        }
    }

    [Command(requiresAuthority = false)]
    private void TransferSettings(uint playerNetId, PlayerCustomizableData data)
    {
        var lobbySettingsApplier = FindObjectOfType<ApplyLobbySettingsToPlayers>();

        Debug.Log($"Transfer settings: netId = {playerNetId}, playerName = {data.PlayerName}");
        lobbySettingsApplier.playerCustomizableDatas.Add(playerNetId, data);
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        player = GetComponent<Player>();
        if(player == null) Debug.LogError("Can't find player script. Please put this script on the same game object as the player script.", this);
    }
    #endif
}
