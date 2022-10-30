using UnityEngine;

public class CameraNetworkMove : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance)
        {
            if (GameManager.Instance.currentState != GameManager.GameState.Preparation)
            {
                Vector3 currentLocation = transform.position;
                Vector3 newTargetLocation = new Vector3(currentLocation.x, GameManager.Instance.currentCameraPositionY, currentLocation.z);
                transform.position = Vector3.MoveTowards(currentLocation, newTargetLocation,
                    10.0f);
            }
        }
    }
}
