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
        if (!collision.CompareTag("DeadZone")) return;
        
        GetComponent<Player>().isAlive = false;
        
        //if we're the server the isAlive hook will not get called, so call that method manually
        if(isServer) GetComponent<Player>().OnPlayerAliveStateChanged(true, false);
        
        Destroy(this);
    }
}
