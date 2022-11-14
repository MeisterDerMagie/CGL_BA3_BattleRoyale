using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Wichtel.Extensions;

[ExecuteInEditMode]
public class BackgroundScroller : MonoBehaviour
{
    [Required]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer tile;
    [SerializeField] private float gridSize;
    
    
    private void Update()
    {
        if (tile == null) return;
        
        Vector3 camPos = mainCamera.transform.position;
        float snappedYPos = Mathf.Round(camPos.y / gridSize) * gridSize;
        transform.position = transform.position.With(y: snappedYPos);
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        mainCamera = Camera.main;
        if(tile != null) gridSize = tile.bounds.size.y;
    }
    #endif
}
