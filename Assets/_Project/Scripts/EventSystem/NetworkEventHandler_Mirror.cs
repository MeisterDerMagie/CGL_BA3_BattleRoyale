//(c) copyright by Martin M. Klöckener
using System;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class NetworkEventHandler_Mirror : NetworkBehaviour, INetworkEventHandler
{
    public static NetworkEventHandler_Mirror Instance;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(true);
    }

    [Command(requiresAuthority = false)]
    private void CmdSend(string _serializedEvent)
    {
        if(isServer)
            CmdSendToClients(_serializedEvent);
    }

    [ClientRpc]
    private void CmdSendToClients(string _serializedEvent)
    {
        NetworkEvents.Release(_serializedEvent);
    }
    
    public void Send(string _serializedEvent)
    {
        CmdSend(_serializedEvent);
    }

    public void Receive(string _serializedEvent)
    {
        NetworkEvents.Release(_serializedEvent);
    }
}
}