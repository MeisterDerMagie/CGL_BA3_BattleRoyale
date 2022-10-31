//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    public string playerName;
    
    [SyncVar(hook = nameof(OnPlayerColorChanged))]
    public Color playerColor;
    
    public Action OnPlayerSettingsChanged = delegate{};
    
    public PlayerAnimations anim;
    public PlayerMovement playerMovement;

    //inform the UI about changes to the player settings
    private void OnPlayerNameChanged(string _oldValue, string _newValue) => OnPlayerSettingsChanged?.Invoke();
    private void OnPlayerColorChanged(Color _oldValue, Color _newValue) => OnPlayerSettingsChanged?.Invoke();
}
}