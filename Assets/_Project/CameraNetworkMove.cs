//(c) copyright by by Patrick Handwerk, CGL Th Koeln, Matrikelnummer 11135936

using UnityEngine;

// Interpolates the current position Y of the camera towards the target position Y set in the "gameManager"
public class CameraNetworkMove : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance)
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Preparation)
            {
                Vector3 currentLocation = transform.position;
                Vector3 newTargetLocation = new Vector3(currentLocation.x, GameManager.Instance.CurrentCameraPositionY, currentLocation.z);
                transform.position = Vector3.MoveTowards(currentLocation, newTargetLocation,
                    10.0f);
            }
        }
    }
}
