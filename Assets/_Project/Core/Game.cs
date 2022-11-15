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
    public float DifficultyNormalized => 1f / maxDifficulty * (difficulty - 1f);

    [SerializeField] private float preGameCountdownDuration;
    [SerializeField] private float maxDifficulty;
    [SerializeField] private float difficultyIncrease;
    [SerializeField] private Deadzone deadzone;
    [SerializeField] private Transform warmupScreen, goScreen;

    private bool gameOver = false;
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
        var warmupState = new GameStateWarmup(preGameCountdownDuration, warmupScreen, goScreen);
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
        
        //increase difficulty
        if (!isServer) return;
        difficulty += difficultyIncrease * Time.deltaTime;
        difficulty = Mathf.Clamp(difficulty, 1f, maxDifficulty);
    }

    public void OnDestroy()
    {
        Player.OnPlayerWon -= OnPlayerWon;
    }

    private void OnPlayerWon(Player winner) => gameOver = true;
}
}