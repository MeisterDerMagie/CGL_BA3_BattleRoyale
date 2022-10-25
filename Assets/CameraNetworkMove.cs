using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNetworkMove : MonoBehaviour
{
    [SerializeField] public GameObject gameManagerObject;


    private GameManager gameManagerReference;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerReference = gameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerReference)
        {
            if (gameManagerReference.gameHasStarted)
            {
                Vector3 currentLocation = transform.position;
                Vector3 newTargetLocation = new Vector3(currentLocation.x, gameManagerReference.CurrentCameraPositionY, currentLocation.z);
                transform.position = Vector3.MoveTowards(currentLocation, newTargetLocation,
                    10.0f);
            }
        }
    }
}
