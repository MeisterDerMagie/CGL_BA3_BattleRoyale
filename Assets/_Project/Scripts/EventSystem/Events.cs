//(c) copyright by Martin M. Klöckener
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Doodlenite {
public static class Events
{
    #region PlayerEvents
    public static Event OnPlayerDied = new Event(nameof(OnPlayerDied));
    public static Event<float> OnPlayerConnected = new Event<float>(nameof(OnPlayerConnected));
    public static Event<float, int, string> TestEvent = new Event<float, int, string>(nameof(TestEvent));
    #endregion
}
}