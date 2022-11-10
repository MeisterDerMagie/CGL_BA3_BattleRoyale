//(c) copyright by by Patrick Handwerk, CGL Th Koeln, Matrikelnummer 11135936

using Mirror;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// General game Manager and synchronous game state replication to all clients. Determines the current state of the game, spawns level platforms and
// determines the current cameras and dead zone positions Y as SyncVars to all clients.
public class GameManager : NetworkBehaviour
{
    // GameState enum for the different stages of the game play loop
    public enum GameState {Preparation, Warmup, EarlyGame, MidGame, EndGame};
    
    // Singleton GameManager to access syncVars and general game state in all areas of the code
    public static GameManager Instance;
    
    // Reference to the platform prefab to spawn in the level
    public GameObject platformPrefab;
    
    // Reference to scriptable objects gameplay setting data
    public GameplayData gameplaySetting;

    #region Properties

    public GameState CurrentState
    {
        get => currentState;
        set => currentState = value;
    }
    
    public float GameTime
    {
        get => gameTime;
        set => gameTime = value;
    }
    
    public float DeadZoneDistance
    {
        get => deadZoneDistance;
        set => deadZoneDistance = value;
    }
    
    public float CurrentCameraPositionY
    {
        get => currentCameraPositionY;
        set => currentCameraPositionY = value;
    }

    public float CurrentDeadZonePositionY
    {
        get => currentDeadZonePositionY;
        set => currentDeadZonePositionY = value;
    }

    public float StartDeadzoneDistance
    {
        get => startDeadzoneDistance;
        set => startDeadzoneDistance = value;
    }

    #endregion

    #region Private members

    // Stores the current phase of the game play loop
    [SyncVar] private GameState currentState;
    
    // Timer counting upwards as soon as the level starts. used to change the game State after predetermined times
    [SyncVar] private float gameTime;
    
    // Sync Var to store the current distance of the dead zone to the camera. Needs to be the same on all clients.
    [SyncVar] private float deadZoneDistance;
    
    // SyncVar for the current position of the camera to synchronize the position of the view to all clients
    [SyncVar] private float currentCameraPositionY;
    
    // Synchronizes the current Y position of the dead zone to all clients
    [SyncVar] private float currentDeadZonePositionY;
    // Synchronizes the initial dead zone position Y to all clients
    [SyncVar] private float startDeadzoneDistance = -17.0f;
    

    // Difficulty maps an integer value to "currentGameState"
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

    // Total duration of the match and total interpolation time between initial camera position Y and final camera position Y
    private float movingDuration = 240.0f;
    // Tracks time between platform spawns
    private float spawnTimeTracker;
    // Tracks time between individual game sates
    private float stateTimeTracker;
    // Game Time * difficulty for speed up of the movement of the camera
    private float gameTimeDifficulty;
    // Determines the height distance from the center of the view at which platforms will be spawned
    private float spawnHeight = 15.0f;
    
    // Stores the different times of each game state
    private readonly Dictionary<GameState, float> stateDurations = new Dictionary<GameState, float>();
    
    // Maps a spawn time for spawning platforms to the current difficulty of the game
    private readonly Dictionary<int, float> difficultySpawnTimeMapping = new Dictionary<int, float>();
    
    // Target position Y for the camera to rise towards unitl the end of a match
    private float targetCameraPositionY;
    // Target distance to interpolate the position Y of the dead zone towards
    private float targetDeadzoneDistance;
    // Initial position Y of the camera
    private float startCameraPositionY = 2.0f;
    
    #endregion

    #region Functions

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        targetCameraPositionY = 200.0f;
        targetDeadzoneDistance = 10.0f;
        float startingSpawnTime = 2.0f;
        deadZoneDistance = startDeadzoneDistance;
        
        stateDurations.Add(GameState.Preparation, 10.0f);
        stateDurations.Add(GameState.Warmup, 60.0f);
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
    
    // Checks the current "stateTimeTracker" value against the current game states duration stored in "stateDurations"
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
    
    // Depending on the current game state moves the dead zone upwards towards the players
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
    
    // Checks the current "spawnTimeTracker" value against the current time to spawn mapped in "difficultySpawnTimeMapping" depending on the current difficulty
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
    
    #endregion
}
