//(c) copyright by Martin M. Klöckener

using Doodlenite.ServerProvider;
using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite.GameState {
public class GameStateGameOver : IState
{
    private float timeSinceGameEnded;
    private Deadzone deadzone;

    public GameStateGameOver(Deadzone deadzone)
    {
        Debug.Log("Entered gameOver state.");

        this.deadzone = deadzone;
    }
    
    public void OnEnter()
    {
        timeSinceGameEnded = 0f;
        deadzone.StopAndDestroyCollider();
    }

    public void Tick()
    {
        //shutdown server 1 minute after the game finished, even if there are still players connected
        #if UNITY_SERVER
        timeSinceGameEnded += Time.deltaTime;
        if (timeSinceGameEnded > 60f)
        {
            Debug.Log("The game has ended one minute ago, shut down server.");
            ServerProviderCommunication.Instance.ServerStopped();
            ServerProviderClient.DisconnectClient();
            Application.Quit();
        }
        #endif
    }

    public void OnExit()
    {
        
    }
}
}