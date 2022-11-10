//(c) copyright by by Patrick Handwerk, CGL Th Koeln, Matrikelnummer 11135936

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new gameplay settings", menuName = "GameplaySetting")]
public class GameplayData : ScriptableObject
{
    public int totalGameDuration;
    public List<KeyValuePair<string, int>> StateDurations;
}
