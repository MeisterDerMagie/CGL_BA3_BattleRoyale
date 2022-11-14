//(c) copyright by Martin M. Klöckener

using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite.GameState {
public class GameStateMain : IState
{
    private Deadzone deadzone;
    
    public GameStateMain(Deadzone deadzone)
    {
        Debug.Log("Entered main state.");

        this.deadzone = deadzone;
    }
    
    public void OnEnter()
    {
        deadzone.BeginMovement();
    }

    public void Tick()
    {
        
    }

    public void OnExit()
    {
        
    }
}
}