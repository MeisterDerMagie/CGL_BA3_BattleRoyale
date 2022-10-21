//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class ReadyUp : MonoBehaviour
{
    private NetworkRoomManager roomManager;

    [SerializeField] private Transform iconReady, iconNotReady, textReady, textUnready;
    
    private void Start()
    {
        roomManager = FindObjectOfType<NetworkRoomManager>();
    }

    public void ToggleReady()
    {
        foreach (var roomPlayer in roomManager.roomSlots)
        {
            if(!roomPlayer.isLocalPlayer) continue;
            
            //set UI
            iconReady.gameObject.SetActive(!roomPlayer.readyToBegin);
            textReady.gameObject.SetActive(roomPlayer.readyToBegin);
            iconNotReady.gameObject.SetActive(roomPlayer.readyToBegin);
            textUnready.gameObject.SetActive(!roomPlayer.readyToBegin);
            
            //change state
            roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
        }
    }
}
}