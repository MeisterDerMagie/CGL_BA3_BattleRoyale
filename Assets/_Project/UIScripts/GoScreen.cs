//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodlenite {
public class GoScreen : MonoBehaviour
{
    [SerializeField] private float countdownDuration = 3f;
    private float countdown;
    
    private void OnEnable()
    {
        countdown = countdownDuration;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown < 0f) gameObject.SetActive(false);
    }
}
}