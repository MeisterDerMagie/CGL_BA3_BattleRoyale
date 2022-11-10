//(c) copyright by by Patrick Handwerk, CGL Th Koeln, Matrikelnummer 11135936

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StateDurationSetting {
    public GameManager.GameState state;
    public int duration;
}

[CreateAssetMenu(fileName = "new gameplay settings", menuName = "GameplaySetting")]
public class GameplayData : ScriptableObject
{
    public int totalGameDuration = 240;
    public StateDurationSetting[] stateDurations;

    public GameplayData()
    {
        stateDurations = new StateDurationSetting[5];
        stateDurations[0] = new StateDurationSetting()
        {
            state = GameManager.GameState.Preparation,
            duration = 15
        };
        
        stateDurations[1] = new StateDurationSetting()
        {
            state = GameManager.GameState.Warmup,
            duration = 40
        };
        
        stateDurations[2] = new StateDurationSetting()
        {
            state = GameManager.GameState.EarlyGame,
            duration = 70
        };
        
        stateDurations[3] = new StateDurationSetting()
        {
            state = GameManager.GameState.MidGame,
            duration = 70
        };
        
        stateDurations[4] = new StateDurationSetting()
        {
            state = GameManager.GameState.EndGame,
            duration = 45
        };
    }
}
