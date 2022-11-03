using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;
using Debug = UnityEngine.Debug;

namespace Doodlenite.ServerProvider {
internal class ServerProviderClient
{
    public static bool Connected => _Client.Connected;
    
    private static string _ServerIp = "";
    private static int _ServerPort = 0;
    private static bool _DebugMessages = true;
    private static WatsonTcpClient _Client = null;
    private static string _PresharedKey = null;
    
    public static Action OnCouldNotConnectToServerProvider = delegate { };
    public static Action<string> OnLobbyJoinFailed = delegate(string _reason) {  };

    public ServerProviderClient(string _serverIp, int _serverPort)
    {
        _ServerIp = _serverIp;
        _ServerPort = _serverPort;
        
        ConnectClient();
    }

    public static void ConnectClient()
    { 
        if (_Client != null) _Client.Dispose();

        _Client = new WatsonTcpClient(_ServerIp, _ServerPort);

        //_Client.Events.AuthenticationFailure += AuthenticationFailure;
        //_Client.Events.AuthenticationSucceeded += AuthenticationSucceeded;
        _Client.Events.ServerConnected += ServerConnected;
        _Client.Events.ServerDisconnected += ServerDisconnected;
        _Client.Events.MessageReceived += MessageReceived;

        //_Client.Callbacks.SyncRequestReceived = SyncRequestReceived;
        //_Client.Callbacks.AuthenticationRequested = AuthenticationRequested;

        // _Client.Settings.IdleServerTimeoutMs = 5000;
        _Client.Settings.DebugMessages = _DebugMessages;
        _Client.Settings.Logger = Logger;
        _Client.Settings.NoDelay = true;

        _Client.Keepalive.EnableTcpKeepAlives = true;
        _Client.Keepalive.TcpKeepAliveInterval = 1;
        _Client.Keepalive.TcpKeepAliveTime = 1;
        _Client.Keepalive.TcpKeepAliveRetryCount = 3;

        _Client.Connect();
    }

    public static void DisconnectClient()
    {
        if(_Client.Connected) _Client.Disconnect();
    }

    public static void SendMessage(string _message, Dictionary<object, object> _metadata)
    {
        _Client.Send(_message, _metadata);
    }

    private static void MessageReceived(object sender, MessageReceivedEventArgs args)
    {
        string receivedString = (args.Data != null) ? Encoding.UTF8.GetString(args.Data) : "[null]";
        Debug.Log($"Received message from server provider ({args.IpPort}): {receivedString}");

        if (args.Metadata != null && args.Metadata.Count > 0)
        {
            Console.WriteLine("Metadata:");
            foreach (KeyValuePair<object, object> curr in args.Metadata)
            {
                Console.WriteLine("  " + curr.Key.ToString() + ": " + curr.Value.ToString());
            }
        }
        
        //-- Doodlenite --
        if (receivedString == "clientCanJoin")
        {
            string portString = (string)args.Metadata["port"];
            string lobbyCode = (string)args.Metadata["lobbyCode"];

            //join doodlenite server
            ushort port = ushort.Parse(portString);
            UnityThread.executeInUpdate( () => ServerProviderCommunication.Instance.ClientCanJoin(port, lobbyCode));
        }

        if (receivedString == "lobbyJoinFailed")
        {
            string reason = "[unknown error]";
            if(args.Metadata != null && args.Metadata.ContainsKey("reason")) reason = (string)args.Metadata["reason"];
            UnityThread.executeInUpdate(()=> OnLobbyJoinFailed?.Invoke(reason));
        }
    }

    private static void ServerConnected(object sender, ConnectionEventArgs args) 
    {
        Console.WriteLine(args.IpPort + " connected"); 
    }

    private static void ServerDisconnected(object sender, DisconnectionEventArgs args)
    {
        Console.WriteLine(args.IpPort + " disconnected: " + args.Reason.ToString());
    }

    private static void Logger(Severity sev, string msg)
    {
        if (sev is Severity.Debug or Severity.Info)
            Debug.Log(msg);

        else if (sev is Severity.Warn or Severity.Alert)
            Debug.LogWarning(msg);

        else if(sev is Severity.Critical or Severity.Emergency or Severity.Error)
            Debug.LogError(msg);
    }
}
}