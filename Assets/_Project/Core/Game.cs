//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite.GameState;
using Mirror;
using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite {
public class Game : NetworkBehaviour
{
    public static Game Instance { get; private set; }

    [SyncVar][ShowInInspector]
    private float difficulty = 1f;
    public float Difficulty => difficulty;

    [SerializeField] private float preGameCountdownDuration;
    [SerializeField] private float maxDifficulty;
    [SerializeField] private AnimationCurve difficultyCurve;
    [SerializeField] private float difficultyIncrease;
    [SerializeField] private Deadzone deadzone;

    private bool gameOver;
    private StateMachine gameStateMachine;
    
    
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("There should never be two Game scripts at once!");
        
        Player.OnPlayerWon += OnPlayerWon;
    }

    public void Start()
    {
        //state machine
        gameStateMachine = new StateMachine();
        
        //states
        var warmupState = new GameStateWarmup(preGameCountdownDuration);
        var mainState = new GameStateMain(deadzone);
        var gameOverState = new GameStateGameOver(deadzone);
        
        //transitions
        gameStateMachine.AddTransition(warmupState, mainState, ref warmupState.OnCountdownEnded);
        gameStateMachine.AddTransition(mainState, gameOverState, () => gameOver);
        
        //initial state
        gameStateMachine.SetState(warmupState);
    }

    private void Update()
    {
        //tick statemachine
        gameStateMachine.Tick();
    }

    public void OnDestroy()
    {
        Player.OnPlayerWon -= OnPlayerWon;
    }

    private void OnPlayerWon(Player winner) => gameOver = true;
}
}