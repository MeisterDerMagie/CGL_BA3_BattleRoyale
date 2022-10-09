//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodlenite {
[CreateAssetMenu(fileName = "KeyboardMap", menuName = "Scriptable Object/Keyboard Map", order = 0)]
public class KeyboardMap : ScriptableObject
{
    public List<KeyMap> keyMaps = new List<KeyMap>();

    public bool JumpKey => GetKey("jump");
    public float Axis => GetAxis();

    public void SetKey(string action, KeyCode keyCode, CustomizeKeyboardMap.PrimarySecondary primarySecondary)
    {
        foreach (KeyMap keyMap in keyMaps)
        {
            if (keyMap.action == action)
            {
                switch (primarySecondary)
                {
                    case CustomizeKeyboardMap.PrimarySecondary.Primary:
                        keyMap.keyCodePrimary = keyCode;
                        break;
                    case CustomizeKeyboardMap.PrimarySecondary.Secondary:
                        keyMap.keyCodeSecondary = keyCode;
                        break;
                }
            }
        }
    }
    
    private bool GetKey(string action)
    {
        foreach (KeyMap keyMap in keyMaps)
        {
            if(action != keyMap.action) continue;
            return (Input.GetKeyDown(keyMap.keyCodePrimary) || Input.GetKeyDown(keyMap.keyCodeSecondary));
        }

        Debug.LogError($"Can't find action \"{action}\"!");
        return false;
    }

    private float GetAxis()
    {
        float axis = 0f;
        
        foreach (KeyMap keyMap in keyMaps)
        {
            if (keyMap.action == "moveLeft")
            {
                //if the player pressed the button for "moveLeft", subtract 1 from the axis
                if (Input.GetKey(keyMap.keyCodePrimary) || Input.GetKeyDown(keyMap.keyCodeSecondary)) axis += -1f;
            }

            if (keyMap.action == "moveRight")
            {
                //if the player pressed the button for "moveLeft", add 1 to the axis
                if (Input.GetKey(keyMap.keyCodePrimary) || Input.GetKeyDown(keyMap.keyCodeSecondary)) axis += 1f;
            }
        }

        return axis;
    }
}

[System.Serializable]
public class KeyMap
{
    public string action;
    public KeyCode keyCodePrimary;
    public KeyCode keyCodeSecondary;

    public KeyMap(string action, KeyCode keyCodePrimary, KeyCode keyCodeSecondary)
    {
        this.action = action;
        this.keyCodePrimary = keyCodePrimary;
        this.keyCodeSecondary = keyCodeSecondary;
    }
}
}