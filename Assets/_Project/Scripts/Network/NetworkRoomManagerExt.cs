//(c) copyright by Martin M. Klöckener
using System;
using Mirror;

namespace Doodlenite {
public class NetworkRoomManagerExt : NetworkRoomManager
{
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
}
}