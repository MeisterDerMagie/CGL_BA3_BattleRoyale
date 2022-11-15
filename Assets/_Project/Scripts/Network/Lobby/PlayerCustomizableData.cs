//(c) copyright by Martin M. Klöckener

using UnityEngine;

namespace Doodlenite {
public struct PlayerCustomizableData
{
    public string PlayerName;
    public Color PlayerColor;

    public PlayerCustomizableData(string playerName, Color playerColor)
    {
        PlayerName = playerName;
        PlayerColor = playerColor;
    }
}
}