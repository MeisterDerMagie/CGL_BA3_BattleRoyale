//(c) copyright by Martin M. Klöckener
using System;
using System.Collections.Generic;
using System.IO;
using Doodlenite.ServerProvider;
using FullSerializer;
using kcp2k;
using Mirror;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Doodlenite {
public class ServerProviderCommunication : MonoBehaviour
{
    [Title("Change IP and Port in serverConfig.json in StreamingAssets folder of built game.")]
    [SerializeField, ReadOnly] private string serverIp = "127.0.0.1";
    [SerializeField, ReadOnly] private int serverPort = 50880;
    
    private static ServerProviderCommunication instance;
    public static ServerProviderCommunication Instance => instance;

    private NetworkRoomManagerExt manager;
    private NetworkRoomManagerExt Manager
    {
        get
        {
            if (manager != null) return manager;
            manager = FindObjectOfType<NetworkRoomManagerExt>();
            if(manager == null) Debug.LogError("manager is null!");
            return manager;
        }
    }

    private KcpTransport transport;
    private KcpTransport Transport
    {
        get
        {
            if (transport != null) return transport;
            transport = FindObjectOfType<KcpTransport>();
            if(transport == null) Debug.LogError("transport is null!");
            return transport;
        }
    }

    private ServerProviderClient client;
    private bool disconnectClientOnDestroy = true;

    private void Awake()
    {
       LoadServerConfig();

       if(instance == null) instance = this;
        else
        {
            disconnectClientOnDestroy = false;
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        
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
        metadata.Add("lobbyCode", Manager.lobbyCode);
        ServerProviderClient.SendMessage("serverStarted", metadata);
    }

    public void ServerInGame()
    {
        var metadata = new Dictionary<object, object>();
        metadata.Add("lobbyCode", Manager.lobbyCode);
        ServerProviderClient.SendMessage("serverInGame", metadata);
    }

    public void ServerStopped()
    {
        var metadata = new Dictionary<object, object>();
        metadata.Add("lobbyCode", Manager.lobbyCode);
        ServerProviderClient.SendMessage("serverStopped", metadata);
    }
    #endregion
    
    #region Incoming
    //-- Provider -> Client --
    public void ClientCanJoin(ushort _port, string _lobbyCode)
    {
        //set ip
        Manager.networkAddress = serverIp;
        
        //set port
        Transport.Port = _port;
        
        //set lobby code
        Manager.lobbyCode = _lobbyCode;
        
        //start client
        Manager.StartClient();
    }
    
    //-- Provider -> Server --
        
    #endregion

    private void LoadServerConfig()
    {
        //serverConfig.json should look like this: {"IP":"127.0.0.1","Port":"50880"}
        string encryptedJson = File.ReadAllText(Application.dataPath + "/StreamingAssets/serverConfig.json");
        
        Dictionary<string, string>? content = JsonConvert.DeserializeObject<Dictionary<string, string>>(encryptedJson);
        if(content == null)
        {
            Debug.LogWarning("Could not load serverConfig. Will use localhost instead.");
            return;
        }

        if (content.ContainsKey("IP") && content.ContainsKey("Port"))
        {
            serverIp = content["IP"];
            serverPort = int.Parse(content["Port"]);

            Debug.Log($"Loaded serverProvider IP: {serverIp}");
            Debug.Log($"Loaded serverProvider Port: {serverPort.ToString()}");
        }
        else
        {
            Debug.LogWarning("Could not load serverConfig. Will use localhost instead.");
            return;
        }
    }
}
}