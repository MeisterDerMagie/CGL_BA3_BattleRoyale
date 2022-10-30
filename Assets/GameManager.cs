using System;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : NetworkBehaviour
{
    public enum GameState {Preparation, Warmup, EarlyGame, MidGame, EndGame};

    
    public static GameManager Instance;
    [SerializeField, SyncVar] public GameState currentState;
    [SerializeField, SyncVar] public float gameTime;

    [SerializeField]
    private int Difficulty
    {
        get
        {
            switch (currentState)
            {
                case GameState.Preparation:
                    return 0;
                case GameState.Warmup:
                    return 1;
                case GameState.EarlyGame:
                    return 2;
                case GameState.MidGame:
                    return 4;
                case GameState.EndGame:
                    return 6;
            }
            return Difficulty;
        }
    }
    
    [SerializeField] public GameObject platformPrefab;
    [SerializeField, SyncVar] public float deadZoneDistance;
    [SerializeField, SyncVar] public float currentCameraPositionY;
    [SerializeField, SyncVar] public float currentDeadZonePositionY;
    [SerializeField, SyncVar] public float startDeadzoneDistance = -17.0f;
    
    [SerializeField] private float movingDuration = 240.0f;
    
    private float spawnTimeTracker;
    private float stateTimeTracker;
    private float gameTimeDifficulty;
    private float spawnHeight = 15.0f;
    private readonly Dictionary<GameState, float> stateDurations = new Dictionary<GameState, float>();

    private readonly Dictionary<int, float> difficultySpawnTimeMapping = new Dictionary<int, float>();

    private float targetCameraPositionY;
    private float targetDeadzoneDistance;
    private float startCameraPositionY = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        targetCameraPositionY = 200.0f;
        targetDeadzoneDistance = 10.0f;
        float startingSpawnTime = 2.0f;
        deadZoneDistance = startDeadzoneDistance;
        
        stateDurations.Add(GameState.Preparation, 10.0f);
        stateDurations.Add(GameState.Warmup, 20.0f);
        stateDurations.Add(GameState.EarlyGame, 30.0f);
        stateDurations.Add(GameState.MidGame, 40.0f);
        stateDurations.Add(GameState.EndGame, 60.0f);
        
        
        difficultySpawnTimeMapping.Add(1, startingSpawnTime);
        difficultySpawnTimeMapping.Add(2, startingSpawnTime + 0.0f);
        difficultySpawnTimeMapping.Add(3, startingSpawnTime + 0.0f);
        difficultySpawnTimeMapping.Add(4, startingSpawnTime + 0.0f);
        difficultySpawnTimeMapping.Add(5, startingSpawnTime + 0.0f);
        difficultySpawnTimeMapping.Add(6, startingSpawnTime + 0.0f);
        difficultySpawnTimeMapping.Add(7, startingSpawnTime + 0.0f);
        difficultySpawnTimeMapping.Add(8, startingSpawnTime + 0.0f);
        difficultySpawnTimeMapping.Add(9, startingSpawnTime + 0.0f);
        difficultySpawnTimeMapping.Add(10, startingSpawnTime + 0.0f);
        
        // difficultySpawnTimeVarianceMapping.Add(1, 0.15f);
        // difficultySpawnTimeVarianceMapping.Add(2, 0.15f);
        // difficultySpawnTimeVarianceMapping.Add(3, 0.0f);
        // difficultySpawnTimeVarianceMapping.Add(4, 0.15f);
        // difficultySpawnTimeVarianceMapping.Add(5, 0.25f);
        // difficultySpawnTimeVarianceMapping.Add(6, 0.25f);
        // difficultySpawnTimeVarianceMapping.Add(7, 0.5f);
        // difficultySpawnTimeVarianceMapping.Add(8, 0.5f);
        // difficultySpawnTimeVarianceMapping.Add(9, 0.5f);
        // difficultySpawnTimeVarianceMapping.Add(10, 0.75f);
        //
        // difficultySpawnDistanceMapping.Add(1, 3.0f);
        // difficultySpawnDistanceMapping.Add(2, 3.0f);
        // difficultySpawnDistanceMapping.Add(3, 4.0f);
        // difficultySpawnDistanceMapping.Add(4, 4.0f);
        // difficultySpawnDistanceMapping.Add(5, 4.0f);
        // difficultySpawnDistanceMapping.Add(6, 5.0f);
        // difficultySpawnDistanceMapping.Add(7, 5.0f);
        // difficultySpawnDistanceMapping.Add(8, 6.0f);
        // difficultySpawnDistanceMapping.Add(9, 7.0f);
        // difficultySpawnDistanceMapping.Add(10, 7.0f);

        var xx = GetComponent<BackgroundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        stateTimeTracker += Time.deltaTime;
        CheckGameState();
        
        if (currentState != GameState.Preparation)
        {
            if (isServer)
            {
                
                CheckPlatformSpawn();
                UpdateDeadzoneDistance();

                spawnTimeTracker += Time.deltaTime;
                gameTimeDifficulty += Time.deltaTime * Difficulty;
                currentCameraPositionY = Mathf.Lerp(startCameraPositionY, targetCameraPositionY, (gameTimeDifficulty) / movingDuration);
            }
            
        }
        
    }

    private void CheckGameState()
    {
        if (currentState == GameState.Preparation && stateTimeTracker >= stateDurations[GameState.Preparation])
        {
            currentState = GameState.Warmup;
            stateTimeTracker = 0.0f;
        }
        if (currentState == GameState.Warmup && stateTimeTracker >= stateDurations[GameState.Warmup])
        {
            currentState = GameState.EarlyGame;
            stateTimeTracker = 0.0f;
        }
        if (currentState == GameState.EarlyGame && stateTimeTracker >= stateDurations[GameState.EarlyGame])
        {
            currentState = GameState.MidGame;
            stateTimeTracker = 0.0f;
        }
        if (currentState == GameState.MidGame && stateTimeTracker >= stateDurations[GameState.MidGame])
        {
            currentState = GameState.EndGame;
            stateTimeTracker = 0.0f;
        }
    }

    private void UpdateDeadzoneDistance()
    {
        if (currentState == GameState.Warmup)
        {
            startDeadzoneDistance = Mathf.Lerp(startDeadzoneDistance, -50.0f, Time.deltaTime / stateDurations[GameState.Warmup]);
            deadZoneDistance = startDeadzoneDistance;
        }
        else if (currentState != GameState.Warmup && currentState != GameState.Preparation)
        {
            deadZoneDistance = Mathf.Lerp(startDeadzoneDistance, targetDeadzoneDistance, (gameTimeDifficulty) / movingDuration);
            currentDeadZonePositionY = currentCameraPositionY + deadZoneDistance;
        }
    }

    private void CheckPlatformSpawn()
    {
        if (spawnTimeTracker >= difficultySpawnTimeMapping[Difficulty] && platformPrefab)
        {
            float randomPlatformWidth = Random.Range(0.0f, 1.0f);
            float spawnX = Random.Range(-14.0f, 14.0f);
            spawnHeight = currentCameraPositionY + 20.0f;
            
            GameObject newPlatform = Instantiate(platformPrefab, new Vector3(spawnX, spawnHeight, 0), Quaternion.identity);
            newPlatform.GetComponent<PlatformWidth>().SetWidth(randomPlatformWidth);
            NetworkServer.Spawn(newPlatform);
            spawnTimeTracker = 0;
        }
    }
}
