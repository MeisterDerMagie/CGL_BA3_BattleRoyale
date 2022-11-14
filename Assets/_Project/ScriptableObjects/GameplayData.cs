//(c) copyright by by Patrick Handwerk, CGL Th Koeln, Matrikelnummer 11135936

using System;
using System.Collections.Generic;
using UnityEngine;

// Struct that stores data regarding various settings per game phase. different phases have different values
[Serializable]
public struct StateSettings {
    public GameManager.GameState state;
    public int duration;
    public float platformSpawnTime;
    public float platformMaxSpawnDistance;
    [Range(0f, 1f)]public float platformMinWidth;
}

[CreateAssetMenu(fileName = "new gameplay settings", menuName = "GameplaySetting")]
public class GameplayData : ScriptableObject
{
    public int totalGameDuration = 180;
    public StateSettings[] stateSettingsList;

    // Initializes the array to feature all 5 states in the editor right away
    public GameplayData()
    {
        stateSettingsList = new StateSettings[5];
        stateSettingsList[0] = new StateSettings()
        {
            state = GameManager.GameState.Preparation,
            duration = 10,
            platformSpawnTime = 5.0f,
            platformMaxSpawnDistance = 4.0f,
            platformMinWidth = 1.0f
        };
        
        stateSettingsList[1] = new StateSettings()
        {
            state = GameManager.GameState.Warmup,
            duration = 25,
            platformSpawnTime = 5.0f,
            platformMaxSpawnDistance = 4.0f,
            platformMinWidth = .75f
        };
        
        stateSettingsList[2] = new StateSettings()
        {
            state = GameManager.GameState.EarlyGame,
            duration = 55,
            platformSpawnTime = 4.0f,
            platformMaxSpawnDistance = 5.0f,
            platformMinWidth = .5f
        };
        
        stateSettingsList[3] = new StateSettings()
        {
            state = GameManager.GameState.MidGame,
            duration = 45,
            platformSpawnTime = 3.0f,
            platformMaxSpawnDistance = 6.5f,
            platformMinWidth = .25f
            
        };
        
        stateSettingsList[4] = new StateSettings()
        {
            state = GameManager.GameState.EndGame,
            duration = 45,
            platformSpawnTime = 2.8f,
            platformMaxSpawnDistance = 8.0f,
            platformMinWidth = .0f
        };
    }

    // Returns the duration for the given game state
    public int GetStateDuration(GameManager.GameState state)
    {
        for (int i = 0; i <= stateSettingsList.Length; i++)
        {
            if (stateSettingsList[i].state == state)
            {
                return stateSettingsList[i].duration;
            }
        }

        return 0;
    }

    // Returns the platform spawn time for the given game state
    public float GetStateSpawnTime(GameManager.GameState state)
    {
        for (int i = 0; i <= stateSettingsList.Length; i++)
        {
            if (stateSettingsList[i].state == state)
            {
                return stateSettingsList[i].platformSpawnTime;
            }
        }

        return 0.0f;
    }

    // Returns the platform max spawn distance for the given game state
    public float GetStateSpawnmaxDistance(GameManager.GameState state)
    {
        for (int i = 0; i <= stateSettingsList.Length; i++)
        {
            if (stateSettingsList[i].state == state)
            {
                return stateSettingsList[i].platformMaxSpawnDistance;
            }
        }

        return 0.0f;
    }

    // Returns the platform min width distance for the given game state
    public float GetStateSpawnMinWidth(GameManager.GameState state)
    {
        for (int i = 0; i <= stateSettingsList.Length; i++)
        {
            if (stateSettingsList[i].state == state)
            {
                return stateSettingsList[i].platformMinWidth;
            }
        }

        return 0.0f;
    }
}
