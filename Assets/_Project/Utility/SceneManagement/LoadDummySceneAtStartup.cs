using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wichtel.SceneManagement;

public class LoadDummySceneAtStartup : MonoBehaviour
{
    public SceneReference dummyScene;

    private void Awake() => SceneManager.LoadScene(dummyScene, LoadSceneMode.Additive);
}
