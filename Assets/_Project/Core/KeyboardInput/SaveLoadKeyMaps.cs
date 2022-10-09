//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Doodlenite {
public class SaveLoadKeyMaps : MonoBehaviour
{
    public KeyboardMap keyboardMap;

    private void Start() => LoadKeyMaps();

    [Button]
    public void SaveKeyMaps()
    {
        foreach (KeyMap keyMap in keyboardMap.keyMaps)
        {
            PlayerPrefs.SetInt(keyMap.action + "Primary", (int)keyMap.keyCodePrimary);
            PlayerPrefs.SetInt(keyMap.action + "Secondary", (int)keyMap.keyCodeSecondary);
        }
        PlayerPrefs.Save();
    }

    [Button]
    public void LoadKeyMaps()
    {
        foreach (KeyMap keyMap in keyboardMap.keyMaps)
        {
            keyMap.keyCodePrimary = (KeyCode)PlayerPrefs.GetInt(keyMap.action + "Primary", (int)keyMap.keyCodePrimary);
            keyMap.keyCodeSecondary = (KeyCode)PlayerPrefs.GetInt(keyMap.action + "Secondary", (int)keyMap.keyCodeSecondary);
        }
    }
}
}