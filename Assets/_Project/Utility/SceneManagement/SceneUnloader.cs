//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wichtel.SceneManagement{
[CreateAssetMenu(fileName = "UnloadScenes", menuName = "Scriptable Objects/Scene Management/Unload Scenes", order = 0)]
public class SceneUnloader : ScriptableObject
{
    [SerializeField, BoxGroup("Scenes to unload")] private List<SceneReference> scenesToUnload = new List<SceneReference>();

    private bool isUnloading;
    
    public void Unload()
    {
        Timing.RunCoroutine(_UnloadScenes());
    }
    
    //Unload the scenes that are loaded with this script
    private IEnumerator<float> _UnloadScenes()
    {
        //--- 0. Prevent multiple simultaneous unloads/loads
        if (isUnloading) yield break;
        isUnloading = true;
        
        //--- 1. Unload scenes ---
        foreach (var scene in scenesToUnload)
        {
            Debug.Log("unload Scene: " + scene.ScenePath);
            yield return Timing.WaitUntilDone(SceneManager.UnloadSceneAsync(scene));
        }
        
        //--- 2. Unloading finished ---
        isUnloading = false;
    }
}
}