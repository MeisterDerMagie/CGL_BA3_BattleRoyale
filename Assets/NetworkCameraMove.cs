using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkCameraMove : NetworkBehaviour
{
    [SerializeField] public GameObject GameManagerObject;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
