//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class LeaveLobby : MonoBehaviour
{
    private NetworkRoomManager manager;

    private void Start()
    {
        manager = FindObjectOfType<NetworkRoomManager>();
    }

    public void Leave() => manager.StopClient();
}
}