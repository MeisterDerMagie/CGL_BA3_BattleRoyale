using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Utility;
using Mirror;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Wichtel.Extensions;
using Random = UnityEngine.Random;

namespace Doodlenite {
public class PlatformSpawner : NetworkBehaviour
{
    [SerializeField] private float minPlatformYDistance = 2f;
    [SerializeField] private float maxPlatformYDistance = 6.1f;
    [SerializeField] private float minPlatformXDistance = 4f;
    [SerializeField] private float maxPlatformXDistance = 11f;
    [SerializeField, Range(0f, 1f)] private float positionRandomness = 0f;
    [SerializeField, Range(0f, 1f)] private float widthRandomness = 0f;
    [SerializeField] private float headroom = 15f;

    [SerializeField, AssetsOnly] private PlatformWidth platformPrefab;
    [SerializeField] private Transform platformParent;
    private float maxPlayerHeight;
    private float spawnedHeight;
    private Vector2 lastPlatformPosition;

    private SyncDictionary<Vector3, float> platforms = new SyncDictionary<Vector3, float>(); //Vector3 = position, float = platformWidth

    private void Awake()
    {
        lastPlatformPosition = new Vector2(0f, platformParent.transform.position.y);
    }

    public override void OnStartClient()
    {
        platforms.Callback += UpdatePlatforms;
        
        //process initial platforms
        foreach (var platform in platforms)
        {
            SpawnPlatform(platform.Key, platform.Value);
        }
    }

    private void Update()
    {
        if (!isServer) return;
        
        UpdateMaxPlayerHeight();
        SpawnNewPlatforms();
    }

    [Client]
    private void UpdatePlatforms(SyncDictionary<Vector3, float>.Operation op, Vector3 platformPosition, float platformWidth)
    {
        SpawnPlatform(platformPosition, platformWidth);
    }

    private void SpawnNewPlatforms()
    {
        float difficulty = Game.Instance.DifficultyNormalized;
        
        //float cameraHeight = camera.orthographicSize * 2.0f;
        //float cameraWidth = cameraHeight * camera.aspect;
        float cameraWidth = 37; //this is hardcoded because the server runs headlessly without graphics enabled. No idea how this will behave with a camera...
        float cameraLeftBorder = -cameraWidth / 2f;
        float cameraRightBorder = cameraWidth / 2f;
        
        while (spawnedHeight < maxPlayerHeight)
        {
            //calculate position of next platform
            float yDistance = RemapFloat.Remap(difficulty, 0f, 1f, minPlatformYDistance, maxPlatformYDistance);
            float yDistanceRandomized = yDistance - (Random.Range(0f, yDistance) * positionRandomness);

            float xDistance = RemapFloat.Remap(difficulty, 0f, 1f, minPlatformXDistance, maxPlatformXDistance);
            float xDistanceRandomized = xDistance - (Random.Range(0f, xDistance) * positionRandomness);

            var spawnPosition = new Vector3(lastPlatformPosition.x + (xDistanceRandomized * RandomPositiveOrNegative.Get()), lastPlatformPosition.y + yDistanceRandomized, 0f);

            //prevent platform from being outside the camera view
            if (spawnPosition.x < cameraLeftBorder)
                spawnPosition = spawnPosition.With(x: spawnPosition.x + cameraWidth);

            if (spawnPosition.x > cameraRightBorder)
                spawnPosition = spawnPosition.With(spawnPosition.x - cameraWidth);

            //calculate platform width
            float platformWidth = 1f - difficulty;
            float platformWidthRandomized = platformWidth + Random.Range(0f, widthRandomness) * RandomPositiveOrNegative.Get();

            //spawn platform on server
            SpawnPlatform(spawnPosition, platformWidthRandomized);
            
            //add platform to syncDictionary, in order to spawn it on the clients
            platforms.Add(spawnPosition, platformWidthRandomized);
            
            //cache results
            spawnedHeight += yDistanceRandomized;
            lastPlatformPosition = spawnPosition;
        }
    }

    private void UpdateMaxPlayerHeight()
    {
        foreach (Player player in NetworkRoomManagerExt.LivingPlayers)
        {
            if(player == null) continue;
            if (player.transform.position.y + headroom > maxPlayerHeight) maxPlayerHeight = player.transform.position.y + headroom;
        }
    }
    
    private void SpawnPlatform(Vector3 spawnPosition, float width)
    {
        PlatformWidth newPlatform = Instantiate(platformPrefab, spawnPosition, quaternion.identity, platformParent);
        newPlatform.SetWidth(width);
    }
}
}