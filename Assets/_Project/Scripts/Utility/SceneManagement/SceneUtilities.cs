using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wichtel{
public class SceneUtilities
{
    public static List<Scene> GetAllLoadedScenes()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];
 
        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }
        
        return new List<Scene>(loadedScenes);
    }

    public static bool IsSceneLoaded(Scene _scene)
    {
        var loadedScenes = GetAllLoadedScenes();
        return loadedScenes.Contains(_scene);
    }

    public static bool IsSceneLoaded(string _sceneNameOrPath)
    {
        var loadedScenes = GetAllLoadedScenes();
        foreach (var scene in loadedScenes) if (scene.name == _sceneNameOrPath || scene.path == _sceneNameOrPath) return true;
        return false;
    }
}
}