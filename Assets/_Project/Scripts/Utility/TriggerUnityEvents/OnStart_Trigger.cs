using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Doodlenite{
    [HideMonoScript]
    public class OnStart_Trigger : MonoBehaviour
    {
        [SerializeField] public bool isEnabled = true;
        [Serializable] public class OnStart : UnityEvent { }
        [SerializeField] private OnStart onStart = new OnStart();

        private void Start()
        {
            if(isEnabled) onStart.Invoke();
        }
    }
}
