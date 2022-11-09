using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Doodlenite;

public class PlayerDeath : NetworkBehaviour
{
    private float timeSpentInDeadZone = 0;
    private float deadTime;

    private void Start()
    {
        deadTime = GameManager.Instance.deadTime;
    }
    
    private void Update()
    {
        if (!isServer) return;
        
        // If spent too much time in deadzone, disconnect from server
        if (!(timeSpentInDeadZone >= deadTime)) return;

        GetComponent<Player>().isAlive = false;
        
        //if we're the server the isAlive hook will not get called, so call that method manually
        if(isServer) GetComponent<Player>().OnPlayerAliveStateChanged(true, false);
        
        Destroy(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone")) timeSpentInDeadZone += Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone")) timeSpentInDeadZone = 0;
    }
}
