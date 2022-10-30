using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMoveDeadzone : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance)
        {
            if (GameManager.Instance.currentState != GameManager.GameState.Preparation &&
                GameManager.Instance.currentState != GameManager.GameState.Warmup)
            {
                Vector3 currentLocation = transform.position;
                Vector3 newTargetLocation = new Vector3(currentLocation.x, GameManager.Instance.currentDeadZonePositionY, currentLocation.z);
                transform.position = Vector3.MoveTowards(currentLocation, newTargetLocation,
                    10.0f);
            }
        }
    }
}
