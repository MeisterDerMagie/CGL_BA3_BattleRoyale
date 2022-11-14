using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlatformWidth : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float width = 0.5f;
    [SerializeField, HideInInspector] private float widthBefore = -1f;
    [SerializeField] private GameObject[] platforms;

    private void Start() => SetWidth(width);

    public void SetWidth(float width)
    {
        this.width = Mathf.Clamp(width, 0f, 1f);
        
        int index = Mathf.RoundToInt((platforms.Length-1) * this.width);

        foreach (Transform child in transform)
        {
            if(Application.isPlaying) Destroy(child.gameObject);
            
            #if UNITY_EDITOR
            else StartCoroutine(DestroyImmediateSafe(child.gameObject));
            #endif
        }

        if(Application.isPlaying) Instantiate(platforms[index], transform);

        #if UNITY_EDITOR
        else StartCoroutine(InstantiateSafe(platforms[index], transform));
        #endif
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        //do nothing if you're not a scene object
        if (gameObject.scene.name == null)
            return;
        
        //do nothing if width didn't change
        if (width == widthBefore) return;
        
        //otherwise set width
        widthBefore = width;
        SetWidth(width);
    }

    private IEnumerator DestroyImmediateSafe(GameObject go)
    {
        yield return null;
        DestroyImmediate(go);
    }

    private IEnumerator InstantiateSafe(GameObject go, Transform parent)
    {
        yield return null;
        Instantiate(go, parent);
    }
    #endif
}
