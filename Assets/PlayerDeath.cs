using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Doodlenite;

public class PlayerDeath : NetworkBehaviour
{
    private float timeSpentInDeadZone = 0;
    private float deadTime;

    // Start is called before the first frame update
    private void Start()
    {
        deadTime = GameManager.Instance.deadTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isServer) return;
        
        // If spent too much time in deadzone, disconnect from server
        if (!(timeSpentInDeadZone >= deadTime)) return;

        GetComponent<Player>().isAlive = false;
        Destroy(this);

        //GetComponent<Player>().enabled = false;
        //GetComponent<InputManager>().enabled = false;
        //GetComponent<PlayerMovement>().enabled = false;
        //GetComponent<KeyboardInput>().enabled = false;
        //if(NetworkServer.connections.Count > 1) connectionToServer.Disconnect();
        //Debug.Log("I dies");
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
