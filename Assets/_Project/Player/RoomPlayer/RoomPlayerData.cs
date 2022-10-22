//(c) copyright by Martin M. Klöckener
using System;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Doodlenite {
//This class stores all custom data of the room players and informs views if the data was changed
public class RoomPlayerData : NetworkBehaviour
{
    //SyncVars
    [SyncVar(hook = nameof(OnColorChanged))]
    private Color playerColor = new Color(0.913725f, 0.090196f, 0.074510f, 1);
    
    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    private string playerName = "";
    
    
    //public accessors
    public Color PlayerColor => playerColor;
    public string PlayerName => playerName;
    
    
    //sncVar hooks
    private void OnColorChanged(Color _oldValue, Color _newValue) => OnValuesChanged?.Invoke();
    private void OnPlayerNameChanged(string _oldValue, string _newValue) => OnValuesChanged?.Invoke();
    
    
    //on values chagend event (could be an own event for each value, but this is easier and this context is not performance critical)
    public Action OnValuesChanged = delegate { };
    
    
    //gets called when the player clicks on a color in the ui
    [Command(requiresAuthority = false)]
    public void CmdSetColor(Color _color)
    {
        playerColor = _color;
    }
    
    //gets called when the player enters a new player name
    [Command(requiresAuthority = false)]
    public void CmdSetPlayerName(string _name)
    {
        playerName = _name;
    }
}
}