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
        player.Kill();
        
        Destroy(this);
    }
}
