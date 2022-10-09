using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGameObject : MonoBehaviour
{
    public Vector3 scale;
    private Vector3 originalScale;

    public void ScaleUp() => Scale(scale);
    public void ScaleDown() => Scale(originalScale);

    private void Start() => originalScale = transform.localScale;

    public void Scale(Vector3 targetScale) => transform.localScale = targetScale;
}
