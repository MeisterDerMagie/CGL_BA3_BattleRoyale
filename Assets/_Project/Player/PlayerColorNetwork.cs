//(c) copyright by Martin M. Klöckener
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class PlayerColorNetwork : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public void CmdSetColor(Color _color)
    {
        GetComponentInChildren<PlayerColor>().SetColor(_color);
        RpcSetColor(_color);
    }

    [ClientRpc]
    private void RpcSetColor(Color _color)
    {
        GetComponentInChildren<PlayerColor>().SetColor(_color);
    }
}
}