using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Doodlenite{
[HideMonoScript]
public class OnPressedKey_Trigger : MonoBehaviour
{
    [SerializeField] public bool isEnabled = true;
    [SerializeField] public KeyCode key;
    [SerializeField] public KeyType keyType;
    [Serializable] public class OnPressedKey : UnityEvent { }
    [SerializeField] private OnPressedKey pressedKey = new OnPressedKey();

    private void Update()
    {
        switch (keyType)
        {
            case KeyType.KeyDown:
                if(Input.GetKeyDown(key) && isEnabled) pressedKey.Invoke();
                break;
            case KeyType.KeyUp:
                if(Input.GetKeyUp(key) && isEnabled) pressedKey.Invoke();
                break;
            case KeyType.KeyHold:
                if(Input.GetKey(key) && isEnabled) pressedKey.Invoke();
                break;
        }
    }
    
    public enum KeyType{KeyDown, KeyUp, KeyHold}
}
}