using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlatformMove : NetworkBehaviour
{
    Vector3 start;
    Vector3 target;

    [SerializeField, Range(1, 10)] private int speed = 1;
    public GameObject gameManagerObject;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        target = new Vector3(transform.position.x , transform.position.y - 100.0f, transform.position.z);

        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isServer && gameManager.gameHasStarted) {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }
    }
}
