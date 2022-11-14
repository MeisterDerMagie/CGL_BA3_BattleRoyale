//(c) copyright by Martin M. Klöckener

using System;
using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite.GameState {
public class GameStateWarmup : IState
{
    public float Countdown { get; private set; }
    private float countdownStartTime;

    public Action OnCountdownEnded = delegate {  };
    
    public GameStateWarmup(float countdownStartTime)
    {
        Debug.Log("Entered warmup state.");
        this.countdownStartTime = countdownStartTime;
    }
    
    public void OnEnter()
    {
        Countdown = countdownStartTime;
    }

    public void Tick()
    {
        Countdown -= Time.deltaTime;
        if (Countdown < 0f)
        {
            Countdown = 0f;
            OnCountdownEnded?.Invoke();
        }
    }

    public void OnExit()
    {
        
    }
}
}