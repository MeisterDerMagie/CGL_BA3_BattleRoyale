using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Doodlenite{
    [HideMonoScript]
public class OnEnable_Trigger : MonoBehaviour
{
    [SerializeField] public bool isEnabled = true;
    [Serializable] public class OnEnableTrigger : UnityEvent { }
    [SerializeField, LabelText("On Enable")] private OnEnableTrigger onEnableTrigger = new OnEnableTrigger();

    private void OnEnable()
    {
        if(isEnabled) onEnableTrigger.Invoke();
    }
}
}