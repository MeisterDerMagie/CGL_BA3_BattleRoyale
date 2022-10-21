//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Doodlenite {
[RequireComponent(typeof(NetworkRoomPlayer))]
public class PositionRoomPlayerOnSlot : MonoBehaviour
{
    private NetworkRoomStartPosition[] roomStartPositions;
    
    private void Start() => roomStartPositions = FindObjectsOfType<NetworkRoomStartPosition>();

    //yes, it's not very performant to do this in Update, but doing it in Start did not work...
    private void Update()
    {
        int roomPlayerIndex = GetComponent<NetworkRoomPlayer>().index;
        
        foreach (var startPosition in roomStartPositions)
        {
            if (startPosition.index == roomPlayerIndex)
                transform.position = startPosition.transform.position;
        }
    }
}
}