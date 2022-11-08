//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Wichtel;

namespace Doodlenite {
//Das hier ist von Interesse für den Doodlenite Server, für die Clients ist ServerSettings.cs 
public class ServerSetup : MonoBehaviour
{
    private NetworkRoomManagerExt manager;
    private KcpTransport transport;
    
    private void Awake()
    {
        #if UNITY_SERVER
        manager = FindObjectOfType<NetworkRoomManagerExt>();
        transport = FindObjectOfType<KcpTransport>();

        Dictionary<string, string> args = CommandLineController.CommandLineArguments;
        
        //port settings
        if (args.ContainsKey("port"))
        {
            bool couldParse = ushort.TryParse(args["port"], out ushort port);
            transport.Port = couldParse ? port : transport.Port;

            if(couldParse) Debug.Log($"Set server port to {port}");
        }
        
        //lobyCode
        if (args.ContainsKey("lobbyCode"))
        {
            manager.lobbyCode = args["lobbyCode"];
            Debug.Log($"Set lobby code to {args["lobbyCode"]}");
        }
        #endif
    }
}
}