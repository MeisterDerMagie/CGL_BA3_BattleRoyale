//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodlenite {
public class UnparentAfterXSeconds : MonoBehaviour
{
    [SerializeField] private float delay;
    private float timer;
    
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > delay) transform.SetParent(null);
    }
}
}