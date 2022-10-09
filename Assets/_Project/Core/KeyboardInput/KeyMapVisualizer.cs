//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Doodlenite {
public class KeyMapVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private KeyboardMap keyboardMap;
    [SerializeField] private string action;
    [SerializeField] private CustomizeKeyboardMap.PrimarySecondary primarySecondary;
    
    private void Update()
    {
        //yes, this should only be update when the user made a change, but like this it's quick 'n dirty...
        foreach (KeyMap keyMap in keyboardMap.keyMaps)
        {
            if (keyMap.action == action)
            {
                switch (primarySecondary)
                {
                    case CustomizeKeyboardMap.PrimarySecondary.Primary:
                        text.text = keyMap.keyCodePrimary.ToString();
                        break;
                    case CustomizeKeyboardMap.PrimarySecondary.Secondary:
                        text.text = keyMap.keyCodeSecondary.ToString();
                        break;
                }
            }
        }
    }
}
}