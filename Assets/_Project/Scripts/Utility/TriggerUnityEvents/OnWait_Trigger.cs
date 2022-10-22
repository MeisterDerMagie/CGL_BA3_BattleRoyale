using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using MEC;

namespace Doodlenite{
[HideMonoScript]
public class OnWait_Trigger : MonoBehaviour
{
    [SerializeField] public bool isEnabled = true;
    [SerializeField] public float waitTime;
    [SerializeField, ReadOnly] public float countdown;
    [Serializable] public class OnTriggerAfterWaitTime : UnityEvent { }
    [SerializeField] private OnTriggerAfterWaitTime triggerAfterWaitTime = new OnTriggerAfterWaitTime();

    public void WaitThenTrigger()
    {
        if (!isEnabled) return;
        
        countdown = waitTime;
        Timing.RunCoroutine(_startCoundown());
    }
    public void WaitThenTrigger(float _waitTime)
    {
        if(!isEnabled) return;

        countdown = _waitTime;
        Timing.RunCoroutine(_startCoundown());
    }

    private void TriggerEvents()
    {
        if(isEnabled) triggerAfterWaitTime.Invoke();
    }

    private IEnumerator<float> _startCoundown()
    {
        while (countdown > 0f)
        {
            countdown -= Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        
        TriggerEvents();
    }

    private void OnValidate()
    {
        countdown = waitTime;
    }
}
}