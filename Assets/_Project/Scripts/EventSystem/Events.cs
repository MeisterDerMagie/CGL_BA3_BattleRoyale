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
    public static Event<float, double, int, string, Color> TestEvent = new Event<float, double, int, string, Color>(nameof(TestEvent));
    #endregion
}
}