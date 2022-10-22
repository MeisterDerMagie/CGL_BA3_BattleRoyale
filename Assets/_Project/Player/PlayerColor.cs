using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Spine.Unity;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation anim;
    [SerializeField] private string slot = "playerColor";
    [SerializeField] private Color color;
    [SerializeField] private bool setColorOnStart = true;

    public void SetColor(Color color)
    {
        if (anim == null) return;
        anim.skeleton?.FindSlot(slot)?.SetColor(color);
    }

    public Color GetColor() => color;

    private void Start()
    {
        if(setColorOnStart) SetColor(color);
    }

    private void OnValidate() => SetColor(color);
}
