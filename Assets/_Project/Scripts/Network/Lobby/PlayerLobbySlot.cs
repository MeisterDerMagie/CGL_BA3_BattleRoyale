//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Doodlenite {
public class PlayerLobbySlot : MonoBehaviour
{
    [ReadOnly]
    public int slotIndex;

    [SerializeField] private Transform ready, notReady;
    
    [SerializeField] private List<Transform> activeObjectsOnLocalPlayer;
    [SerializeField] private List<Transform> activeObjectsOnAllPlayers;

    private NetworkRoomManager roomManager;

    private void Start()
    {
        //get manager
        roomManager = FindObjectOfType<NetworkRoomManager>();

        //initially deactivate all objects
        DeactivateAllObjects();
    }

    private void Update()
    {
        List<NetworkRoomPlayer> roomPlayers = roomManager.roomSlots;

        //show UI objects
        DeactivateAllObjects();
        
        foreach (var roomPlayer in roomPlayers)
        {
            if (roomPlayer.index != slotIndex) continue;
            
            //activate objects for local player
            if(roomPlayer.isLocalPlayer) 
                foreach (Transform t in activeObjectsOnLocalPlayer)
                {
                    t.gameObject.SetActive(roomPlayer.isLocalPlayer);
                }
            
            //activate objects for other players too
            foreach (Transform t in activeObjectsOnAllPlayers)
            {
                t.gameObject.SetActive(true);
            }
            
            //ready status
            if (roomPlayer.readyToBegin)
            {
                ready.gameObject.SetActive(true);
                notReady.gameObject.SetActive(false);
            }
            else
            {
                ready.gameObject.SetActive(false);
                notReady.gameObject.SetActive(true);
            }
            
            return;
        }
    }

    private void DeactivateAllObjects()
    {
        foreach (Transform t in activeObjectsOnLocalPlayer) { t.gameObject.SetActive(false); }
        foreach (Transform t in activeObjectsOnAllPlayers) { t.gameObject.SetActive(false); }
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        slotIndex = transform.GetSiblingIndex();
    }
    #endif
}
}