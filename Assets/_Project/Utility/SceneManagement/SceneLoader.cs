//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wichtel.SceneManagement{
[CreateAssetMenu(fileName = "LoadScenes", menuName = "Scriptable Objects/Scene Management/Load Scenes", order = 0)]
public class SceneLoader : ScriptableObject
{
    [SerializeField, BoxGroup("Settings")] private bool loadAdditively;
    [SerializeField, BoxGroup("Settings")] private bool unloadScenesBefore;//, unloadScenesAfter; 
    [SerializeField, BoxGroup("Settings")] public GameObject loadingScreen;
    [SerializeField, BoxGroup("Settings")] private float additionalFakeLoadingTime;

    [ShowIf("unloadScenesBefore")][SerializeField, BoxGroup("Scenes to unload before")] private List<SceneReference> scenesToUnloadBefore = new List<SceneReference>();
    [HideIf("loadAdditively")][SerializeField, BoxGroup("Scenes to load")] private SceneReference newScene;
    [SerializeField, BoxGroup("Scenes to load")] private List<SceneReference> scenesToLoadAdditively = new List<SceneReference>();

    private bool isLoading;
    private bool isUnloading;
    
    public void Load()
    {
        Timing.RunCoroutine(_LoadScenes());
    }

    public void Unload()
    {
        Timing.RunCoroutine(_UnloadScenes());
    }

    private IEnumerator<float> _LoadScenes()
    {
        //--- 0. Prevent multiple simultaneous loads/unloads
        if (isLoading || isUnloading) yield break;
        isLoading = true;
        
        //--- 1. Show loading screen ---
        int debugLogCounter = 1; //Kann gelöscht werden, wenn die ganzen ---- 1. Show Loading Screen ---- Debug.Logs entfernt werden
        GameObject loadingScreenInstance = null;
        if (loadingScreen != null)
        {
            Debug.Log($"---- {(debugLogCounter++)}. Show Loading Screen ----");
            loadingScreenInstance = Instantiate(loadingScreen);
            DontDestroyOnLoad(loadingScreenInstance);
            if (loadingScreenInstance.GetComponent<LoadingScreenBase>() != null)
            {
                yield return Timing.WaitUntilDone(loadingScreenInstance.GetComponent<LoadingScreenBase>()._ShowLoadingScreen());
            }
            Debug.Log($"---- {(debugLogCounter++)}. Loading Screen is ready ----");
        }
        
        //--- 2. Unload scenes ---
        if (unloadScenesBefore)
        {
            foreach (var scene in scenesToUnloadBefore)
            {
                if (SceneUtilities.IsSceneLoaded(scene))
                {
                    Debug.Log("unload Scene: " + scene.ScenePath);
                    yield return Timing.WaitUntilDone(SceneManager.UnloadSceneAsync(scene));
                }
                else
                {
                    Debug.LogWarning($"Can't unload scene \"{(string)scene}\" because it's not loaded. Make sure to unload only loaded scenes.");
                }
            }
        }
        
        //--- 2.1 Wait for one frame (not sure if necessary, just to make sure that all OnDestroy of the unloaded scene are finished)
        yield return Timing.WaitForOneFrame;
        //--- 2.2 Wait the additional fake loading time which can artificially extend the loading process
        if (additionalFakeLoadingTime != 0f) yield return Timing.WaitForSeconds(additionalFakeLoadingTime);
        
        //--- 3. Load single scene if not additively ---
        Debug.Log($"---- {(debugLogCounter++)}. Load Scenes ----");
        if (!loadAdditively)
        {
            Debug.Log("load Scene: " + newScene.ScenePath);
            yield return Timing.WaitUntilDone(SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single));
        }
        
        //--- 4. Load additive scenes ---
        foreach (var scene in scenesToLoadAdditively)
        {
            Debug.Log("load Scene additively: " + scene.ScenePath);
            yield return Timing.WaitUntilDone(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
        }
        
        //--- 5. After all scenes are loaded: search for all initializers and initialize all those who want to be initialized at the scene start ---
        Debug.Log($"---- {(debugLogCounter++)}. Initialize Scene ----");
        InitializeScenes();
        
        //--- 6. Hide loading screen ---
        Debug.Log($"---- {(debugLogCounter++)}. Hide LoadingScreen ----");

        if (loadingScreenInstance != null)
        {
            if(loadingScreenInstance.GetComponent<LoadingScreenBase>() != null) loadingScreenInstance.GetComponent<LoadingScreenBase>().HideLoadingScreen();
            else Destroy(loadingScreenInstance);
        }

        //--- 7. Loading finished ---
        isLoading = false;
    }
    
    //Unload the scenes that are loaded with this script
    private IEnumerator<float> _UnloadScenes()
    {
        //--- 0. Prevent multiple simultaneous unloads/loads
        if (isUnloading || isLoading) yield break;
        isUnloading = true;
        
        //--- 1. Show loading screen ---
        GameObject loadingScreenInstance = null;
        if (loadingScreen != null)
        {
            loadingScreenInstance = Instantiate(loadingScreen);
            DontDestroyOnLoad(loadingScreenInstance);
        }
        
        //--- 2. Unload scenes ---
        Debug.Log("unload Scene: " + newScene.ScenePath);
        yield return Timing.WaitUntilDone(SceneManager.UnloadSceneAsync(newScene));

        foreach (var scene in scenesToLoadAdditively)
        {
            Debug.Log("unload Scene: " + scene.ScenePath);
            yield return Timing.WaitUntilDone(SceneManager.UnloadSceneAsync(scene));
        }
        
        //--- 3. Hide loading screen ---
        if(loadingScreenInstance != null) loadingScreenInstance.GetComponent<LoadingScreenBase>()?.HideLoadingScreen();
        
        //--- 4. Unloading finished ---
        isUnloading = false;
    }
    
    private void InitializeScenes()
    {
        //In diesem Projekt arbeiten wir ohne Initializer...
        
        //get all initializers
        //var allInitializers = new List<INIT001_Initialize>(ComponentExtensions.FindAllComponentsOfType<INIT001_Initialize>());
        
        //initialize all initializers
        /*
        foreach (var initializer in allInitializers)
        {
            if(initializer.initOn == INIT001_Initialize.InitOn.SceneStart) initializer.StartInitialization();
        }
        */
    }
}
}