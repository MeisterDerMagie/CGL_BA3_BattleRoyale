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

    public List<GameObject> platforms = new List<GameObject>();
    
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
    [SerializeField, SyncVar] private GameState currentState;
    
    // Timer counting upwards as soon as the level starts. used to change the game State after predetermined times
    [SerializeField,SyncVar] private float gameTime;
    
    // Sync Var to store the current distance of the dead zone to the camera. Needs to be the same on all clients.
    [SyncVar] private float deadZoneDistance;
    
    // SyncVar for the current position of the camera to synchronize the position of the view to all clients
    [SyncVar] private float currentCameraPositionY;
    
    // Synchronizes the current Y position of the dead zone to all clients
    [SyncVar] private float currentDeadZonePositionY;
    // Synchronizes the initial dead zone position Y to all clients
    [SyncVar] private float startDeadzoneDistance = -17.0f;
    

    // Difficulty maps an integer value to "currentGameState"
    private float Difficulty
    {
        get
        {
            switch (currentState)
            {
                case GameState.Preparation:
                    return 0;
                case GameState.Warmup:
                    return .6f;
                case GameState.EarlyGame:
                    return .8f;
                case GameState.MidGame:
                    return 1.0f;
                case GameState.EndGame:
                    return 1.4f;
            }
            return Difficulty;
        }
    }
    // Tracks time between platform spawns
    private float spawnTimeTracker;
    // Tracks time between individual game sates
    [SerializeField] private float stateTimeTracker;
    // Game Time * difficulty for speed up of the movement of the camera
    private float gameTimeDifficulty;
    // Determines the height distance from the center of the view at which platforms will be spawned
    private float spawnHeight = 15.0f;

    // Target position Y for the camera to rise towards unitl the end of a match
    private float targetCameraPositionY;
    // Target distance to interpolate the position Y of the dead zone towards
    private float targetDeadzoneDistance;
    // Initial position Y of the camera
    private float startCameraPositionY = 2.0f;
    // Stores the last spawn location X to determine max distance of new spawn platform from last spawn
    private float lastSpawnX;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        if (isServer)
        {
            stateTimeTracker += Time.deltaTime;
            CheckGameState();

            if (currentState != GameState.Preparation)
            {
                CheckPlatformSpawn();
                UpdateDeadzoneDistance();

                spawnTimeTracker += Time.deltaTime;
                gameTimeDifficulty += Time.deltaTime * Difficulty;
                currentCameraPositionY = Mathf.Lerp(startCameraPositionY, targetCameraPositionY,
                    (gameTimeDifficulty) / gameplaySetting.totalGameDuration);
            }
        }
    }
    
    // Checks the current "stateTimeTracker" value against the current game states duration stored in "stateDurations"
    private void CheckGameState()
    {
        
        if (currentState == GameState.Preparation && stateTimeTracker >= gameplaySetting.GetStateDuration(GameState.Preparation))
        {
            currentState = GameState.Warmup;
            stateTimeTracker = 0.0f;
            Debug.Log("Changed gameplay state to: \"Warmup\"");
        }
        if (currentState == GameState.Warmup && stateTimeTracker >= gameplaySetting.GetStateDuration(GameState.Warmup))
        {
            currentState = GameState.EarlyGame;
            stateTimeTracker = 0.0f;
            Debug.Log("Changed gameplay state to: \"EarlyGame\"");
        }
        if (currentState == GameState.EarlyGame && stateTimeTracker >= gameplaySetting.GetStateDuration(GameState.EarlyGame))
        {
            currentState = GameState.MidGame;
            stateTimeTracker = 0.0f;
            Debug.Log("Changed gameplay state to: \"MidGame\"");
        }
        if (currentState == GameState.MidGame && stateTimeTracker >= gameplaySetting.GetStateDuration(GameState.MidGame))
        {
            currentState = GameState.EndGame;
            stateTimeTracker = 0.0f;
            Debug.Log("Changed gameplay state to: \"EndGame\"");
        }
    }
    
    // Depending on the current game state moves the dead zone upwards towards the players
    private void UpdateDeadzoneDistance()
    {
        if (currentState == GameState.Warmup)
        {
            startDeadzoneDistance = Mathf.Lerp(startDeadzoneDistance, -50.0f, Time.deltaTime / gameplaySetting.GetStateDuration(GameState.Warmup));
            deadZoneDistance = startDeadzoneDistance;
        }
        else if (currentState != GameState.Warmup && currentState != GameState.Preparation)
        {
            deadZoneDistance = Mathf.Lerp(startDeadzoneDistance, targetDeadzoneDistance, (gameTimeDifficulty) / gameplaySetting.totalGameDuration);
            currentDeadZonePositionY = currentCameraPositionY + deadZoneDistance;
        }
    }
    
    // Checks the current "spawnTimeTracker" value against the current time to spawn mapped in "difficultySpawnTimeMapping" depending on the current difficulty
    private void CheckPlatformSpawn()
    {
        if (spawnTimeTracker >= gameplaySetting.GetStateSpawnTime(currentState) && platformPrefab)
        {
            float randomPlatformWidth = Random.Range(gameplaySetting.GetStateSpawnMinWidth(currentState), 1.0f);
            float maxDistanceToLast = gameplaySetting.GetStateSpawnmaxDistance(currentState);
            float spawnX = lastSpawnX + maxDistanceToLast * (Random.value > .5 ? -1.0f : 1.0f) + Random.Range(-1.0f, 1.0f);
            
            // clamp value to boundaries of the screen
            if (spawnX > 14.0f)
            {
                spawnX = 14.0f;
            }
            if (spawnX < -14.0f)
            {
                spawnX = -14.0f;
            }

            spawnHeight = currentCameraPositionY + 20.0f;
            
            GameObject newPlatform = Instantiate(platformPrefab, new Vector3(spawnX, spawnHeight, 0), Quaternion.identity);
            newPlatform.GetComponent<PlatformWidth>().SetWidth(randomPlatformWidth);
            NetworkServer.Spawn(newPlatform);
            spawnTimeTracker = 0;

            lastSpawnX = spawnX;
        }
    }
    
    #endregion
}
