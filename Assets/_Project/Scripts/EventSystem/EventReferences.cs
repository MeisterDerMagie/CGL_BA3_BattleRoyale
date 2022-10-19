//(c) copyright by Martin M. Klöckener
using System.Collections.Generic;
using System.Reflection;

namespace Doodlenite {
public static class EventReferences
{
    public static Dictionary<string, object> eventReferences = new Dictionary<string, object>();

    //needs to be initialized on game start
    public static void Initialize()
    {
        eventReferences.Clear();

        foreach (FieldInfo fieldInfo in typeof(Events).GetFields())
        {
            eventReferences.Add(fieldInfo.Name, fieldInfo.GetValue(null));
        }
    }
}
}