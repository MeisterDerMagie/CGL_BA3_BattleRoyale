//(c) copyright by Martin M. Klöckener
using FullSerializer;

namespace Doodlenite {
public static class EventSerializer
{
    private static fsSerializer serializer = new fsSerializer();
    
    public static string Serialize(string _eventName, object[] _params)
    {
        string serializedParams = string.Empty;
        
        //FS Serializer
        serializer.TrySerialize(_params, out fsData data);
        serializedParams = fsJsonPrinter.PrettyJson(data);

        //generate network string, bestehend aus eventName und params
        string networkString = _eventName + ";" + serializedParams;
        
        return networkString;
    }

    public static (string eventName, object[] parameters) Deserialize(string _serializedEvent)
    {
        string[] splitEventNameAndParams = _serializedEvent.Split(new[] { ';' }, 2);
        string eventName = splitEventNameAndParams[0];
        string serializedParams = splitEventNameAndParams[1];
        
        serializer = new fsSerializer();
        fsData parsedData = fsJsonParser.Parse(serializedParams);
        object[] deserializedParams = new object[]{};
        serializer.TryDeserialize(parsedData, ref deserializedParams);
        
        return (eventName, deserializedParams);
    } 
}
}