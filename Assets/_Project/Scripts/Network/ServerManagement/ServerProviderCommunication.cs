//(c) copyright by Martin M. Klöckener
using System;
using System.Collections.Generic;
using Doodlenite.ServerProvider;
using kcp2k;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

namespace Doodlenite {
public class ServerProviderCommunication : MonoBehaviour
{
    [SerializeField] private string serverIp = "127.0.0.1";
    [SerializeField] private int serverPort = 50880;
    
    private static ServerProviderCommunication instance;
    public static ServerProviderCommunication Instance => instance;

    private NetworkRoomManagerExt manager;
    private KcpTransport transport;

    private ServerProviderClient client;
    private bool disconnectClientOnDestroy = true;

    private void Awake()
    {
        if(instance == null) instance = this;
        else
        {
            disconnectClientOnDestroy = false;
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        
        //get references
        manager = FindObjectOfType<NetworkRoomManagerExt>();
        transport = FindObjectOfType<KcpTransport>();
        
        if(manager == null) Debug.LogError("manager is null!");
        if(transport == null) Debug.LogError("transport is null!");
        
        //start client
        client = new ServerProviderClient(serverIp, serverPort);
    }

    private void OnDestroy()
    {
        if(this.disconnectClientOnDestroy)
            ServerProviderClient.DisconnectClient();
    }

    #region Outgoing
    //-- Client -> Provider --
    public void HostRequest()
    {
        ServerProviderClient.SendMessage("host", null);
    }

    public void JoinRequest(string _lobbyCode)
    {
        var metadata = new Dictionary<object, object>();
        metadata.Add("lobbyCode", _lobbyCode);
        ServerProviderClient.SendMessage("join", metadata);
    }
    
    //-- Server -> Provider --
    public void ServerStarted()
    {
        var metadata = new Dictionary<object, object>();
        metadata.Add("lobbyCode", manager.lobbyCode);
        ServerProviderClient.SendMessage("serverStarted", metadata);
    }

    public void ServerInGame()
    {
        var metadata = new Dictionary<object, object>();
        metadata.Add("lobbyCode", manager.lobbyCode);
        ServerProviderClient.SendMessage("serverInGame", metadata);
    }

    public void ServerStopped()
    {
        var metadata = new Dictionary<object, object>();
        metadata.Add("lobbyCode", manager.lobbyCode);
        ServerProviderClient.SendMessage("serverStopped", metadata);
    }
    #endregion
    
    #region Incoming
    //-- Provider -> Client --
    public void ClientCanJoin(ushort _port, string _lobbyCode)
    {
        //set port
        transport.Port = _port;
        
        //set lobby code
        manager.lobbyCode = _lobbyCode;
        
        //start client
        manager.StartClient();
    }
    
    //-- Provider -> Server --
        
    #endregion
}
}