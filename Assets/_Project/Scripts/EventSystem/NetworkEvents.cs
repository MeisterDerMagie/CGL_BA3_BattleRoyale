//(c) copyright by Martin M. Klöckener
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Wichtel;

namespace Doodlenite {
public static class NetworkEvents
{
    public static void Invoke(string _eventName, object[] _params)
    {
        //Serialize params
        string serializedParams = string.Empty;
        
        //XML - very verbose
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(object[]));
        using(StringWriter textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, _params);
            serializedParams = textWriter.ToString();
        }
        
        //Json
        serializedParams = JsonUtility.ToJson(_params[0]);

        Debug.Log("Serialized parameters: " + serializedParams);
        
        //send over network
    }
    
    //DONT KEEP THIS METHOD IN THIS CLASS. HANDLE SERIALIZATION IN ITS OWN CLASS
    public static string SerializeObject<T>(this T toSerialize)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

        using(StringWriter textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
    }

    public static void Release(string _eventName, object[] _params)
    {
        //params need to be deserialized before calling this method
        
        //invoke events locally
        EventReferences.eventReferences[_eventName].InvokeMethod("Invoke", _params);
    }
}
}