//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using UnityEngine;

namespace Doodlenite {
public class CustomizeKeyboardButton : MonoBehaviour
{
    [SerializeField] private string action;
    [SerializeField] private CustomizeKeyboardMap.PrimarySecondary primarySecondary;

    [SerializeField] private CustomizeKeyboardMap customizeKeyboardMap;
    
    public void PollKey()
    {
        customizeKeyboardMap.PollKey(action, primarySecondary);
    }
}
}