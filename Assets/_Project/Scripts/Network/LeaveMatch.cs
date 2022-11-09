//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class LeaveMatch : MonoBehaviour
{
    public void Leave()
    {
        var manager = FindObjectOfType<NetworkRoomManager>();
        if(manager != null) manager.StopClient();
        else Debug.LogError("Can't disconnect. Something went wrong.");
    }
}
}