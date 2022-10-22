//(c) copyright by Martin M. Klöckener
using System;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Doodlenite {
public class RoomPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnColorChanged))] private Color playerColor = new Color(0.913725f, 0.090196f, 0.074510f, 1);

    private PlayerColor playerColorComponent;

    //public int playerIndex => GetComponent<NetworkRoomPlayer>().index;
    
    //sncVar hook
    private void OnColorChanged(Color _oldValue, Color _newValue) => UpdateColor();

    //init color on client
    public override void OnStartClient() => UpdateColor();

    //init color on server
    public override void OnStartServer() => UpdateColor();

    //gets called when a player clicks on a color in the ui
    [Command(requiresAuthority = false)]
    public void CmdSetColor(Color _color)
    {
        playerColor = _color;
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (playerColorComponent == null) playerColorComponent = GetComponentInChildren<PlayerColor>();
        playerColorComponent.SetColor(playerColor);
    }
}
}