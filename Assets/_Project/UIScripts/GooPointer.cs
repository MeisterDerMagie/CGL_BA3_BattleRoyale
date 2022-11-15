using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Mirror;
using TMPro;
using UnityEngine;

public class GooPointer : NetworkBehaviour
{
    [SerializeField] private Transform gooTop;
    [SerializeField] private Transform gooPointer;
    [SerializeField] private TextMeshProUGUI gooDistance;
    
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isServer) return;

        float cameraHeight = mainCamera.orthographicSize * 2.0f;
        float cameraLowerBorderY = mainCamera.transform.position.y - cameraHeight / 2f;

        if (gooTop.transform.position.y > cameraLowerBorderY)
        {
            if(gooPointer.gameObject.activeSelf) gooPointer.gameObject.SetActive(false);
            return;
        }

        if (!gooPointer.gameObject.activeSelf) gooPointer.gameObject.SetActive(true);

        float distance = cameraLowerBorderY - gooTop.transform.position.y;
        int distanceRounded = Mathf.RoundToInt(distance);
        gooDistance.SetText(distanceRounded.ToString() + " m");
    }
}
