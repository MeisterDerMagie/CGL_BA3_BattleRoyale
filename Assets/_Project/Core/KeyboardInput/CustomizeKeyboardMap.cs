//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Doodlenite {
//https://forum.unity.com/threads/find-out-which-key-was-pressed.385250/#post-6072834
public class CustomizeKeyboardMap : MonoBehaviour
{
    [SerializeField] private KeypressListener keypressListener;
    [SerializeField] private KeyboardMap keyboardMap;
    [SerializeField] private Transform pressKeyMessage;
    [SerializeField] private SaveLoadKeyMaps keyMapSaveLoader;
    
    private string action;
    private PrimarySecondary primarySecondary;

    private void OnKeyWasPressed(KeyCode keyCode)
    {
        //if the user pressed escape
        if ((keyCode == KeyCode.Escape))
        {
            //don't change the button
            Cancel();
            
            //and do nothing
            return;
        }
        
        //otherwise set the new key
        keyboardMap.SetKey(action, keyCode, primarySecondary);
        
        //hide message
        pressKeyMessage.gameObject.SetActive(false);
        
        //save UserPrefs
        keyMapSaveLoader.SaveKeyMaps();
    }

    public void PollKey(string action, PrimarySecondary primarySecondary)
    {
        //cache the action and if it's primary or secondary key
        this.action = action;
        this.primarySecondary = primarySecondary;

        //wait until the user presses a key
        keypressListener.keyDownListener += OnKeyWasPressed;
        
        //show message
        pressKeyMessage.gameObject.SetActive(true);
    }

    private void Cancel()
    {
        //stop listening for keyPresses
        keypressListener.keyDownListener = null;
        //hide message
        pressKeyMessage.gameObject.SetActive(false);
    }

    private void OnDisable() => Cancel();

    public enum PrimarySecondary
    {
        Primary,
        Secondary
    }
}
}