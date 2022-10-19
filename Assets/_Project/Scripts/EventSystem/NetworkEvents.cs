//(c) copyright by Martin M. Klöckener
using System;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;
using Wichtel;

namespace Doodlenite {
public static class NetworkEvents
{
    public static void Invoke(string _eventName, object[] _params)
    {
        //Serialize params
        EventSerializer.Serialize(_eventName, _params);

        //send over network
        
    }

    public static void Release(string _networkString)
    {
        //deserialize
        (string eventName, object[] parameters) deserializedEvent = EventSerializer.Deserialize(_networkString);
        
        //invoke events locally
        EventReferences.eventReferences[deserializedEvent.eventName].InvokeMethod("Invoke", deserializedEvent.parameters);
    }
}
}