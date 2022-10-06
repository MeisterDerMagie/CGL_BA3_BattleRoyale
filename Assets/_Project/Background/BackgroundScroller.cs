using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Required]
    [SerializeField] private Camera mainCamera;
    [SerializeField, AssetsOnly] private GameObject backgroundTile;
    
    void Update()
    {
        
    }
    
    #if UNITY_EDITOR
    private void OnValidate() => mainCamera = Camera.main;
    #endif
}
