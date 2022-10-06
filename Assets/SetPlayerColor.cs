using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]
public class SetPlayerColor : MonoBehaviour
{
    [SerializeField, HideInInspector] private SkeletonAnimation anim;
    [SerializeField] private string slot = "playerColor";
    [SerializeField] private Color color;

    public void SetColor(Color color)
    {
        if (anim == null) anim = GetComponent<SkeletonAnimation>();
        if (anim == null) return;
        anim.skeleton?.FindSlot(slot)?.SetColor(color);
    }

    private void Start() => SetColor(color);
    private void OnValidate() => SetColor(color);
}
