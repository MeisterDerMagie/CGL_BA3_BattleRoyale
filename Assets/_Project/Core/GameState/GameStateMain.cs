//(c) copyright by Martin M. Klöckener

using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite.GameState {
public class GameStateMain : IState
{
    private Deadzone deadzone;
    
    public GameStateMain(Deadzone deadzone)
    {
        this.deadzone = deadzone;
    }
    
    public void OnEnter()
    {
        Debug.Log("Entered main state.");
        
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