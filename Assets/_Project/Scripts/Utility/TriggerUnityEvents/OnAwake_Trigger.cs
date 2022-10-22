using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Doodlenite{
    [HideMonoScript]
    public class OnAwake_Trigger : MonoBehaviour
    {
        [SerializeField] public bool isEnabled = true;
        [Serializable] public class OnAwake : UnityEvent { }
        [SerializeField] private OnAwake onAwake = new OnAwake();

        private void Awake()
        {
            if(isEnabled) onAwake.Invoke();
        }
    }
}
