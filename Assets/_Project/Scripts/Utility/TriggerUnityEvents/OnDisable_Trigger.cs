using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Doodlenite{
public class OnDisable_Trigger : MonoBehaviour
{
    [SerializeField] public bool isEnabled = true;
    [Serializable] public class OnPressedEscape : UnityEvent { }
    [SerializeField] private OnPressedEscape pressedEscape = new OnPressedEscape();

    private void OnDisable()
    {
        if(isEnabled) pressedEscape.Invoke();
    }
}
}