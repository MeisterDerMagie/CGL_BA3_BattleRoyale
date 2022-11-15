//(c) copyright by Martin M. Klöckener

using System;
using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite.GameState {
public class GameStateWarmup : IState
{
    public float Countdown { get; private set; }
    private float countdownStartTime;

    private Transform warmupScreen, goScreen;

    public Action OnCountdownEnded = delegate {  };
    
    public GameStateWarmup(float countdownStartTime, Transform warmupScreen, Transform goScreen)
    {
        this.countdownStartTime = countdownStartTime;

        this.warmupScreen = warmupScreen;
        this.goScreen = goScreen;
    }
    
    public void OnEnter()
    {
        Debug.Log("Entered warmup state.");
        
        Countdown = countdownStartTime;
        
        goScreen.gameObject.SetActive(false);
        warmupScreen.gameObject.SetActive(true);
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
        goScreen.gameObject.SetActive(true);
        warmupScreen.gameObject.SetActive(false);
    }
}
}