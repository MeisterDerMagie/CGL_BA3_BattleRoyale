using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Wichtel.Extensions;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private List<Transform> cameraTargets;
    [SerializeField] private float transitionDuration = 0.15f;
    [SerializeField] private Ease ease = Ease.InOutExpo;
    [SerializeField] private Transform currentScene;

    private void Start()
    {
        currentScene = cameraTargets[0];
        
        //activate first scene
        cameraTargets[0].gameObject.SetActive(true);
        
        //deactivate all other scenes
        for (int i = 1; i < cameraTargets.Count; i++)
        {
            cameraTargets[i].gameObject.SetActive(false);
        }
    }

    [Button]
    public void SwitchScene(int sceneIndex)
    {
        //make sure the sceneIndex is valid
        if (sceneIndex < 0 || sceneIndex >= cameraTargets.Count)
            return;

        Transform newScene = cameraTargets[sceneIndex];
        
        //do nothing if the new scene is the current scene
        if (newScene == currentScene)
            return;
        
        //activate new scene
        newScene.gameObject.SetActive(true);
        
        //move camera to new scene and deactivate old scene then
        Transform sceneToDeactivate = currentScene;
        cam.DOMove(newScene.position.With(z: cam.position.z), transitionDuration).SetEase(ease).OnComplete(()=> {sceneToDeactivate.gameObject.SetActive(false);});
        
        //set new scene as currentScene
        currentScene = newScene;
    }
}
