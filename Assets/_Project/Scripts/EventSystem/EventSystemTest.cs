//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Doodlenite {
public class EventSystemTest : MonoBehaviour
{
    private void Start()
    {
        Events.OnPlayerConnected.Subscribe(OnPlayerConnected);
        Events.OnPlayerDied.Subscribe(OnPlayerDied);
        Events.TestEvent.Subscribe(OnTestEvent);
    }

    [Button]
    public void Test()
    {
        float score = 4.5f;
        
        
        Events.OnPlayerConnected.Invoke(score, true);
        Events.OnPlayerDied.Invoke(true);
        Events.TestEvent.Invoke(3, 3, 3, "asdf", new Color(0.4f, 0.2f, 0.5f), invokeNetworkEvent: true);
    }

    private static void OnPlayerConnected(float _float) => Debug.Log(_float);
    private static void OnPlayerDied() => Debug.Log("player died");

    private static void OnTestEvent(float _float, double _double, int _int, string _string, Color _color) => Debug.Log($"float: {_float}; double: {_double}; int: {_int}; string: {_string}; Color: {_color}");
}
}