//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class NetworkPlayerMovement : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] internal Rigidbody2D rb = null;

    [Tooltip("Set to true if moves come from owner client, set to false if moves always come from server")]
    public  bool clientAuthority = false;

    public float hardSetPositionInterval = 0.5f;
    private float nextHardSetPositionSyncTime = 0f;
    
    private bool IgnoreSync => isServer || ClientWithAuthority;
    private bool ClientWithAuthority => clientAuthority && hasAuthority;
    
    /// <summary>
    /// Values sent on client with authority after they are sent to the server
    /// </summary>
    readonly ClientSyncState previousValue = new ClientSyncState();
    
    #region Sync vars
    [SyncVar(hook = nameof(OnVelocityChanged))]
    private Vector2 velocity;

    //[SyncVar(hook = nameof(OnPositionChanged))]
    //private Vector3 position;

    private void OnVelocityChanged(Vector2 _, Vector2 newValue)
    {
        if (IgnoreSync) return;
        rb.velocity = newValue;
    }

    private void OnPositionChanged(Vector3 _, Vector3 newValue)
    {
        if(IgnoreSync) return;
        transform.position = newValue;
    }
    
    #endregion
    
    internal void Update()
    {
        if (isServer)
        {
            SyncToClients();
        }
        else if (ClientWithAuthority)
        {
            SendToServer();
        }
    }
    
    /// <summary>
    /// Updates sync var values on server so that they sync to the client
    /// </summary>
    [Server]
    private void SyncToClients()
    {
        Vector2 currentVelocity = rb.velocity;
        //bool velocityChanged = syncVelocity && ((previousValue.velocity - currentVelocity).sqrMagnitude > velocitySensitivity * velocitySensitivity);
        
        bool velocityChanged = true; //hier sollte man checken, ob die velocity sich geöndert hat

        if (!velocityChanged) return;
        
        velocity = currentVelocity;
        previousValue.velocity = currentVelocity;
        //position = transform.position;
    }

    /// <summary>
    /// Uses Command to send values to server
    /// </summary>
    [Client]
    private void SendToServer()
    {
        if (!hasAuthority)
        {
            Debug.LogWarning("SendToServer called without authority");
            return;
        }

        SendVelocity();
        //SendPosition();
    }

    [Client]
    private void SendPosition()
    {
        float now = Time.time;
        if (now < nextHardSetPositionSyncTime)
            return;
        
        CmdSendPosition(transform.position);
    }
    
    [Client]
    private void SendVelocity()
    {
        float now = Time.time;
        if (now < previousValue.nextSyncTime)
            return;

        Vector2 currentVelocity = rb.velocity;

        //bool velocityChanged = syncVelocity && ((previousValue.velocity - currentVelocity).sqrMagnitude > velocitySensitivity * velocitySensitivity);
        bool velocityChanged = true;

        if (!velocityChanged) return;
        
       CmdSendVelocity(currentVelocity);
       previousValue.velocity = currentVelocity;
       
       // only update syncTime if velocity has changed
       previousValue.nextSyncTime = now + syncInterval;
    }
    
    /// <summary>
    /// Called when only Velocity has changed on the client
    /// </summary>
    [Command]
    private void CmdSendVelocity(Vector2 _velocity)
    {
        // Ignore messages from client if not in client authority mode
        if (!clientAuthority)
            return;

        velocity = _velocity;
        rb.velocity = _velocity;
    }

    [Command]
    private void CmdSendPosition(Vector3 _position)
    {
        // Ignore messages from client if not in client authority mode
        if (!clientAuthority)
            return;

        transform.position = _position;
    }
    
    /// <summary>
    /// holds previously synced values
    /// </summary>
    public class ClientSyncState
    {
        /// <summary>
        /// Next sync time that velocity will be synced, based on syncInterval.
        /// </summary>
        public float nextSyncTime;
        public Vector2 velocity;
    }
}
}