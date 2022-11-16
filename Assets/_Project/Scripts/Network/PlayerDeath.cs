using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Doodlenite;

public class PlayerDeath : NetworkBehaviour
{
    //die when touching the deadzone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        if (!collision.CompareTag("DeadZone")) return;
        
        var player = GetComponent<Player>();
        player.isAlive = false;
        
        //if we're the server the isAlive hook will not get called, so call that method manually
        if(isServer) player.OnPlayerAliveStateChanged(true, false);

        var manager = FindObjectOfType<NetworkRoomManagerExt>();
        manager.OnPlayerDied(player);
        
        Destroy(this);
    }
}
