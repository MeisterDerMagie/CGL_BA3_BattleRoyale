//(c) copyright by Martin M. Klöckener
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class ServerRunningCallback : NetworkBehaviour
{
    public override void OnStartServer()
    {
        Debug.Log("Server started.");
    }
}
}