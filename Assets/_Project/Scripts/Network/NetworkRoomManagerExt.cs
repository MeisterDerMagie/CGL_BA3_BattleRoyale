//(c) copyright by Martin M. Klöckener
using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using Wichtel.Extensions;

namespace Doodlenite {
public class NetworkRoomManagerExt : NetworkRoomManager
{
    public string lobbyCode = string.Empty;
    [HideInInspector] public List<Player> players = new List<Player>();
    public List<Player> LivingPlayers => players.Where(player => player.isAlive).ToList();
    public List<Player> DeadPlayers => players.Where(player => !player.isAlive).ToList();

    public override void Start()
    {
        base.Start();
        Player.OnPlayerDied += OnPlayerDied;
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

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        UpdatePlayerList();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        UpdatePlayerList();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Player.OnPlayerDied -= OnPlayerDied;

    }

    private void OnPlayerDied(Player _player)
    {
        Debug.Log($"{_player.playerName} died.");
        UpdatePlayerList();
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
    }
}
}