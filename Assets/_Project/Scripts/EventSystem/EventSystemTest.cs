//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sectile {
public class EventSystemTest : MonoBehaviour
{
    [Button]
    public void Test()
    {
        EventReferences.Initialize();
        
        float score = 4.5f;
        
        Events.TestEvent.Invoke(1.1f, 3, "asdf", invokeNetworkEvent: true);
        
        Events.OnPlayerConnected.Subscribe(OnPlayerConnected);
        Events.OnPlayerConnected.Invoke(score);
        
        Events.OnPlayerDied.Subscribe(OnPlayerDied);
        
        //Call event by string
        NetworkEvents.Release("OnPlayerDied", new object[]{});
        NetworkEvents.Release("OnPlayerConnected", new object[]{2.6f});
    }

    private static void OnPlayerConnected(float _float) => Debug.Log(_float);
    private static void OnPlayerDied() => Debug.Log("player died");

}
}