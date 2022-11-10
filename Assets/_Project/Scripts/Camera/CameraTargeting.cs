//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Doodlenite {
public class CameraTargeting : MonoBehaviour
{
    [SerializeField] private ProCamera2D camera2D;
    private Player currentTarget;
    
    private void Update()
    {
        if (currentTarget != null && currentTarget.isAlive) return;
        NextTarget();
    }

    private void UpdateTarget()
    {
        camera2D.RemoveAllCameraTargets();
        if (currentTarget == null) return;
        camera2D.AddCameraTarget(currentTarget.transform);
    }
    
    [Button]
    public void NextTarget()
    {
        //if the local player is still alive, pick it as target
        if (NetworkRoomManagerExt.LocalPlayer != null && NetworkRoomManagerExt.LocalPlayer.isAlive && currentTarget != NetworkRoomManagerExt.LocalPlayer)
        {
            currentTarget = NetworkRoomManagerExt.LocalPlayer;
            UpdateTarget();
            return;
        }
        
        //if no player is alive...
        if (NetworkRoomManagerExt.LivingPlayers.Count == 0)
        {
            if (currentTarget == null) return;
            //... don't follow any player
            currentTarget = null;
            UpdateTarget();
            return;
        }
        
        //if only one player is alive...
        if (NetworkRoomManagerExt.LivingPlayers.Count == 1)
        {
            //... pick this one as camera target
            if(currentTarget == null) currentTarget = NetworkRoomManagerExt.LivingPlayers[0];
            else if(currentTarget != NetworkRoomManagerExt.LivingPlayers[0]) currentTarget = NetworkRoomManagerExt.LivingPlayers[0];
            return;
        }
        
        //if more than one player is alive...
        if (NetworkRoomManagerExt.LivingPlayers.Count > 1)
        {
            //... and if the curret target is still alive...
            if (NetworkRoomManagerExt.LivingPlayers.Contains(currentTarget))
            {
                //... pick the next player as target
                int indexOfCurrentTarget = NetworkRoomManagerExt.LivingPlayers.IndexOf(currentTarget);
                int nextIndex = (indexOfCurrentTarget < NetworkRoomManagerExt.LivingPlayers.Count - 1)
                    ? indexOfCurrentTarget + 1
                    : 0;
                currentTarget = NetworkRoomManagerExt.LivingPlayers[nextIndex];
                UpdateTarget();
            }
            //... otherwise if the current target is not alive...
            else
            {
                //...pick another player as target
                currentTarget = NetworkRoomManagerExt.LivingPlayers[0];
            }
        }
    }
}
}