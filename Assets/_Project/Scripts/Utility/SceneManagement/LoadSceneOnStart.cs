using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wichtel.SceneManagement;

public class LoadSceneOnStart : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneToLoad;
    
    void Start()
    {
        sceneToLoad.Load();
        Destroy(gameObject);
    }
}
