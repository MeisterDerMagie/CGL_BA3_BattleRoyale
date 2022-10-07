using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Wichtel.Extensions;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private List<Transform> cameraTargets;
    [SerializeField] private float transitionDuration = 0.75f;
    [SerializeField] private Ease ease = Ease.InOutExpo;
    [SerializeField] private Transform currentScene;
    [SerializeField] private AudioSourceRandomizer woosh;

    private Tween currentTween;

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
        
        //move camera to first scene
        cam.position = currentScene.position.With(z: cam.position.z);
    }

    private void Update()
    {
        //This is a quick implementation. Esc should not always go back to menu 0, but to the previous one...
        if(Input.GetKeyDown(KeyCode.Escape)) SwitchScene(0);
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
        
        //kill running tween if there is one
        currentTween?.Kill();

        //activate new scene
        newScene.gameObject.SetActive(true);
        
        //move camera to new scene and deactivate old scene then
        Transform sceneToDeactivate = currentScene;
        currentTween = cam.DOMove(newScene.position.With(z: cam.position.z), transitionDuration).SetEase(ease).OnComplete(()=> {sceneToDeactivate.gameObject.SetActive(false);});
        
        //play woosh sound
        woosh.Play();

        //set new scene as currentScene
        currentScene = newScene;
    }
}
