//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Doodlenite {
public class PlayerLobbySlot : MonoBehaviour
{
    [ReadOnly]
    public int slotIndex;

    [SerializeField] private Transform ready, notReady;
    [SerializeField] private TextMeshProUGUI playerName;
    
    [SerializeField] private List<Transform> activeObjectsOnLocalPlayer;
    [SerializeField] private List<Transform> activeObjectsOnLocalAndRemotePlayers;
    [SerializeField] private List<Transform> activeObjectsOnRemotePlayers;

    private NetworkRoomManagerExt roomManager;
    private List<NetworkRoomPlayer> networkRoomPlayers = new List<NetworkRoomPlayer>();
    private NetworkRoomPlayer associatedNetworkRoomPlayer;
    private RoomPlayerData associatedRoomPlayerData;

    private void Start()
    {
        //get manager
        roomManager = FindObjectOfType<NetworkRoomManagerExt>();

        //initially deactivate all objects
        DeactivateAllUIElements();
    }
    
    //why update the UI every update? Because the NetworkRoomManager callbacks (OnClientConnect / Disconnect) are poorly implemented. When they fire, the players aren't yet properly initialized... 
    private void Update()
    {
        GetAssociatedPlayer();
        UpdateSlotUI();
        UpdatePlayerColor();
        UpdatePlayerName();
    }

    //gets the player that's associated to this slot
    private void GetAssociatedPlayer()
    {
        networkRoomPlayers = roomManager.roomSlots;

        //reset in case a player left the room
        associatedNetworkRoomPlayer = null;
        associatedRoomPlayerData = null;
        
        //assign player to slot
        foreach (var roomPlayer in networkRoomPlayers)
        {
            if (roomPlayer.index != slotIndex)
                continue;
            
            //get network room player
            associatedNetworkRoomPlayer = roomPlayer;
            
            //get room player data
            associatedRoomPlayerData = roomPlayer.GetComponent<RoomPlayerData>();
        }
    }

    //show / hide UI elements
    private void UpdateSlotUI()
    {
        //hide all UI elements, if this slot has no player assigned
        if (associatedNetworkRoomPlayer == null)
        {
            DeactivateAllUIElements();
            return;
        }
        
        //activate ui elements for local player
        if(associatedNetworkRoomPlayer.isLocalPlayer) 
        {
            foreach (Transform t in activeObjectsOnLocalPlayer)
            {
                if(!t.gameObject.activeInHierarchy) t.gameObject.SetActive(true);
            }
            
            foreach (Transform t in activeObjectsOnRemotePlayers)
            {
                t.gameObject.SetActive(false);
            }
        }
        //activate ui elements only for remote players
        else
        {
            foreach (Transform t in activeObjectsOnLocalPlayer)
            {
                t.gameObject.SetActive(false);
            }

            foreach (Transform t in activeObjectsOnRemotePlayers)
            {
                t.gameObject.SetActive(true);
            }
        }
        
        
        //activate ui elements for local and remote players
        foreach (Transform t in activeObjectsOnLocalAndRemotePlayers)
        {
            if(!t.gameObject.activeInHierarchy) t.gameObject.SetActive(true);
        }
        
        //update ready status
        if (associatedNetworkRoomPlayer.readyToBegin)
        {
            ready.gameObject.SetActive(true);
            notReady.gameObject.SetActive(false);
        }
        else
        {
            ready.gameObject.SetActive(false);
            notReady.gameObject.SetActive(true);
        }
    }

    private void DeactivateAllUIElements()
    {
        foreach (Transform t in activeObjectsOnLocalPlayer) { t.gameObject.SetActive(false); }
        foreach (Transform t in activeObjectsOnLocalAndRemotePlayers) { t.gameObject.SetActive(false); }
        foreach (Transform t in activeObjectsOnRemotePlayers) { t.gameObject.SetActive(false); }
        
        ready.gameObject.SetActive(false);
        notReady.gameObject.SetActive(false);
    }

    private void UpdatePlayerColor()
    {
        if (associatedNetworkRoomPlayer == null) return;
        
        var playerColorComponent = GetComponentInChildren<PlayerColor>();
        playerColorComponent.SetColor(associatedRoomPlayerData.PlayerColor);
    }

    private void UpdatePlayerName()
    {
        if (associatedNetworkRoomPlayer == null) return;
        
        playerName.SetText(associatedRoomPlayerData.PlayerName);
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        slotIndex = transform.GetSiblingIndex();
    }
    #endif
}
}