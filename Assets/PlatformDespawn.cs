//(c) copyright by by Patrick Handwerk, CGL Th Koeln, Matrikelnummer 11135936

using Mirror;
using UnityEngine;

// Checks the platforms negative distance to the camera location and despawns the platform on the server and all clients once out of frame
public class PlatformDespawn : NetworkBehaviour
{
    // Update is called once per frame
    public void Update()
    {
        if (isServer)
        {
            if (GameManager.Instance)
            {
                if (transform.position.y - GameManager.Instance.CurrentCameraPositionY  <= -20.0f && GameManager.Instance.CurrentCameraPositionY > transform.position.y)
                {
                    NetworkServer.Destroy(gameObject);
                }
            }
        }
    }
}
