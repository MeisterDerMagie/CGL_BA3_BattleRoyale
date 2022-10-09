//(c) copyright by Martin M. Klöckener
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Wichtel.TimeControl {
public class GameTime : SerializedMonoBehaviour
{
    [ShowInInspector] public static float TimeScale = 1f;
    [ShowInInspector, ReadOnly] private static float targetTimeScale = -1f;
    
    [ShowInInspector, ReadOnly] public static bool GameIsPaused;

    private static IEnumerable<IPauseable> allPauseables;
    
    private static event Action OnPause = delegate { };
    private static event Action OnUnpause = delegate { };
    
    [Button, DisableInEditorMode]
    public static void SetTimeScale(float _newValue) //Does not unpause the game!
    {
        if (GameIsPaused)
        {
            targetTimeScale = _newValue;
            return;
        }
        
        TimeScale = _newValue;
    }

    [Button, DisableInEditorMode]
    public static void SetTimeScaleAndUnpause(float _newValue)
    {
        targetTimeScale = _newValue;
        UnpauseGame();
    }
    
    [Button, DisableInEditorMode]
    public static void PauseGame()
    {
        GameIsPaused = true;
        targetTimeScale = TimeScale; //cache vorherige TimeScale
        TimeScale = 0f;
        FindAllIPauseables();
        foreach (var iPauseable in allPauseables) { iPauseable.OnPause(); }
        
        OnPause?.Invoke();
    }

    [Button, DisableInEditorMode]
    public static void UnpauseGame()
    {
        GameIsPaused = false;
        TimeScale = targetTimeScale; //gecachte oder zwischenzeitlich geänderte TimeScale wieder setzen
        foreach (var iPauseable in allPauseables) { iPauseable.OnUnpause(); }
        
        OnUnpause?.Invoke();
    }

    private static void FindAllIPauseables()
    {
        allPauseables = FindObjectsOfType<MonoBehaviour>().OfType<IPauseable>();
    }
}

public interface IPauseable
{
    void OnPause();
    void OnUnpause();
}
}