//(c) copyright by Martin M. Klöckener
using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wichtel.Extensions;

namespace Doodlenite {
public class NetworkRoomManagerExt : NetworkRoomManager
{
    public string lobbyCode = string.Empty;
    
    
    public static List<Player> players = new List<Player>();
    public static List<Player> LivingPlayers => players.Where(player => player.isAlive).ToList();
    public static List<Player> DeadPlayers => players.Where(player => !player.isAlive).ToList();

    private static Player localPlayer;
    public static Player LocalPlayer
    {
        get
        {
            if (localPlayer != null) return localPlayer;
            
            var p = FindObjectsOfType<Player>();
            foreach (var player in p)
            {
                if (player.isLocalPlayer) localPlayer = player;
            }
            return localPlayer;
        }
    }

    private bool someoneWon;
    
    public static Action OnNewPlayerSpawned = delegate {  };

    //we need this due to the domain reloading being turned off
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ResetStaticFields()
    {
        players.Clear();
    }

    public override void Start()
    {
        base.Start();
        OnNewPlayerSpawned += UpdatePlayerList;
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
        OnNewPlayerSpawned -= UpdatePlayerList;
        
        players.Clear();

        Debug.Log("NetworkRoomManagerExt destroyed");
    }

    public override void OnRoomServerPlayersReady()
    {
        //start countdown when everyone is ready
        GameStartCountdown.Instance.StartCountdown(OnStartGameCountdownFinished);
    }

    public override void OnRoomServerPlayersNotReady()
    {
        //stop countdown when someone is not ready anymore
        GameStartCountdown.Instance.StopCountdown();
    }

    private void OnStartGameCountdownFinished()
    {
        //start the game
        ServerChangeScene(GameplayScene);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        //inform server provider about started server
        ServerProviderCommunication.Instance.ServerStarted();
    }

    /*
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        UpdatePlayerList();
        CheckForWin();
    }
    */

    //called on the server when a client disconnects
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        
        //Kill disconnected player
        foreach (Player player in players)
        {
            if (player.connectionToClient.identity != null) continue;

            Debug.Log("Kill disconnected player");
            player.Kill();
            break;
        }
        
        UpdatePlayerList();
        CheckForWin();
    }

    //called on the server when a client connects
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        UpdatePlayerList();
    }

    //called on the client after connecting
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        UpdatePlayerList();
    }

    public void OnPlayerDied(Player _player)
    {
        Debug.Log($"{_player.playerName} died.");
        UpdatePlayerList();
        CheckForWin();
    }

    private void UpdatePlayerList()
    {
        Player[] foundPlayers = FindObjectsOfType<Player>();

        players.RemoveEmptyEntries();
        
        foreach (Player player in foundPlayers)
        {
            if (players.Contains(player)) continue;
            players.Add(player);
        }
        
        players.RemoveEmptyEntries();
    }

    private void CheckForWin()
    {
        /*
        Debug.Log("Check for win.");
        Debug.Log("Living players: ");
        foreach (Player player in LivingPlayers)
        {
            if (player == null) Debug.Log("null");
            else
                Debug.Log(player.playerName);
        }
        */
        
        //if(LivingPlayers.Count <= 0) Debug.LogError("Oops, something went wrong. Nobody won. How did this happen?");
        if (LivingPlayers.Count == 1)
        {
            Debug.Log("Someone won!");
            someoneWon = true;
            Player winner = LivingPlayers[0];
            winner.thisPlayerWon = true;
            
            //call this because Syncvar hooks are not called on the server
            winner.OnThisPlayerWon(false, true);
        }
    }
}
}