using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField, Range(1, 10)] public int difficulty = 1;
    [SerializeField] public bool gameHasStarted = false;
    [SerializeField] public float gameTime = 0.0f;
    [SerializeField] public GameObject platformPrefab;
    
    private float spawnTimeTracker = 0.0f;
    private float spawnHeight = 15.0f;
    private float spawnWidthBoundary = 14.0f;
    private float lastSpawnXlocation = 0.0f;
    private Dictionary<int, float> difficultySpawnTimeMapping = new Dictionary<int, float>();
    private Dictionary<int, float> difficultySpawnTimeVarianceMapping = new Dictionary<int, float>();
    private Dictionary<int, float> difficultySpawnDistanceMapping = new Dictionary<int, float>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        difficultySpawnTimeMapping.Add(1, 2.0f);
        difficultySpawnTimeMapping.Add(2, 2.0f);
        difficultySpawnTimeMapping.Add(3, 2.0f);
        difficultySpawnTimeMapping.Add(4, 2.0f);
        difficultySpawnTimeMapping.Add(5, 2.0f);
        difficultySpawnTimeMapping.Add(6, 2.0f);
        difficultySpawnTimeMapping.Add(7, 2.0f);
        difficultySpawnTimeMapping.Add(8, 2.0f);
        difficultySpawnTimeMapping.Add(9, 2.0f);
        difficultySpawnTimeMapping.Add(10, 2.0f);
        
        difficultySpawnTimeVarianceMapping.Add(1, 0.0f);
        difficultySpawnTimeVarianceMapping.Add(2, 0.0f);
        difficultySpawnTimeVarianceMapping.Add(3, 0.0f);
        difficultySpawnTimeVarianceMapping.Add(4, 0.15f);
        difficultySpawnTimeVarianceMapping.Add(5, 0.25f);
        difficultySpawnTimeVarianceMapping.Add(6, 0.25f);
        difficultySpawnTimeVarianceMapping.Add(7, 0.5f);
        difficultySpawnTimeVarianceMapping.Add(8, 0.5f);
        difficultySpawnTimeVarianceMapping.Add(9, 0.5f);
        difficultySpawnTimeVarianceMapping.Add(10, 0.75f);
        
        difficultySpawnDistanceMapping.Add(1, 3.0f);
        difficultySpawnDistanceMapping.Add(2, 3.0f);
        difficultySpawnDistanceMapping.Add(3, 4.0f);
        difficultySpawnDistanceMapping.Add(4, 4.0f);
        difficultySpawnDistanceMapping.Add(5, 4.0f);
        difficultySpawnDistanceMapping.Add(6, 5.0f);
        difficultySpawnDistanceMapping.Add(7, 5.0f);
        difficultySpawnDistanceMapping.Add(8, 6.0f);
        difficultySpawnDistanceMapping.Add(9, 7.0f);
        difficultySpawnDistanceMapping.Add(10, 7.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHasStarted)
        {
            gameTime += Time.deltaTime;
            spawnTimeTracker += Time.deltaTime;
            
            CheckPlatformSpawn();
        }
        
    }

    private void CheckPlatformSpawn()
    {
        if (spawnTimeTracker >= difficultySpawnTimeMapping[difficulty] && platformPrefab)
        {
            float randomPlatformWidth = Random.Range(0.0f, 1.0f);
            float spawnX = Random.Range(-14.0f, 14.0f);
            
            
            GameObject newPlatform = Instantiate(platformPrefab, new Vector3(spawnX, spawnHeight, 0), Quaternion.identity);
            newPlatform.GetComponent<PlatformWidth>().SetWidth(randomPlatformWidth);
            NetworkPlatformMove platformMoveScript = newPlatform.GetComponent<NetworkPlatformMove>();
            platformMoveScript.SetManager(gameObject);
            NetworkServer.Spawn(newPlatform);
            spawnTimeTracker = 0;
            
            lastSpawnXlocation = spawnX;
        }
    }
}
