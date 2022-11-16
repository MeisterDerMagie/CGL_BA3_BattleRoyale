//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Experimental;
using UnityEngine;

namespace Doodlenite {
public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPlayerAliveStateChanged))]
    public bool isAlive = true;
    
    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    public string playerName;
    
    [SyncVar(hook = nameof(OnPlayerColorChanged))]
    public Color playerColor;
    
    public Action OnPlayerSettingsChanged = delegate{};
    public static Action<Player> OnPlayerDied = delegate(Player _player) {  };
    public static Action<Player> OnPlayerWon = delegate(Player _player) {  };
    
    public PlayerAnimations anim;
    public PlayerMovement playerMovement;

    private void Start()
    {
        NetworkRoomManagerExt.OnNewPlayerSpawned?.Invoke();
        OnPlayerWon += RemoveMotionOnWin;
    }

    private void OnDestroy()
    {
        OnPlayerWon -= RemoveMotionOnWin;

    }

    //inform the UI about changes to the player settings
    public void ApplyPlayerSettings() => OnPlayerSettingsChanged?.Invoke();
    private void OnPlayerNameChanged(string _oldValue, string _newValue) => OnPlayerSettingsChanged?.Invoke();
    private void OnPlayerColorChanged(Color _oldValue, Color _newValue) => OnPlayerSettingsChanged?.Invoke();
    public void OnPlayerAliveStateChanged(bool _oldValue, bool _newValue)
    {
        Debug.Log($"Player alive state changed. oldValue: {_oldValue}, newValue: {_newValue}");
        
        OnPlayerSettingsChanged?.Invoke();
        OnPlayerDied?.Invoke(this);
        
        //play death animation
        anim.PlayDeathAnimation();
        
        //remove all components that are no longer needed (like movement)
        Destroy(GetComponent<PlayerMovement>());
        Destroy(GetComponentInChildren<PlayerAnimationsController>());
    }

    private void RemoveMotionOnWin(Player winner)
    {
        if (!winner.isLocalPlayer) return;
        
        //remove all components that are no longer needed (like movement)
        Destroy(GetComponent<PlayerMovement>());
        Destroy(GetComponentInChildren<PlayerAnimationsController>());
    }
}
}