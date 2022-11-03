//(c) copyright by Martin M. Klöckener
using System;
using Mirror;
using UnityEngine;
using Wichtel;

namespace Doodlenite {
public class NetworkRoomManagerExt : NetworkRoomManager
{
    public string lobbyCode = string.Empty;
    
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

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);
        
        //inform server provider about the started game
        //IS THIS THE CORRECT METHOD TO OVERRIDE??
        //ServerProviderCommunication.Instance.ServerInGame();
    }
}
}