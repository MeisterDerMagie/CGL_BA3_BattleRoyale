//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using UnityEngine;
using UnityEngine.Serialization;

namespace Doodlenite {
public class InputManager : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;

    private void Update()
    {
        inputHandler.HandleInput();
    }

    public void SetInputProvider(InputHandler _inputHandler)
    {
        this.inputHandler = _inputHandler;
    }
}
}