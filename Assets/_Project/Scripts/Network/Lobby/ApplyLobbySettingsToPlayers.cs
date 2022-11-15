//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class ApplyLobbySettingsToPlayers : NetworkBehaviour
{
    public SyncDictionary<uint, PlayerCustomizableData> playerCustomizableDatas = new SyncDictionary<uint, PlayerCustomizableData>(); //key: netId player

    public override void OnStartClient()
    {
        // subscribe to callback
        playerCustomizableDatas.Callback += OnDictionaryChanged;
        
        // Process initial SyncDictionary payload
        ApplySettings();
    }

    private void OnDictionaryChanged(SyncIDictionary<uint, PlayerCustomizableData>.Operation op, uint key, PlayerCustomizableData item)
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        List<Player> players = FindObjectsOfType<Player>().ToList();

        foreach (Player player in players)
        {
            if (!playerCustomizableDatas.ContainsKey(player.netId)) continue;

            player.playerName = playerCustomizableDatas[player.netId].PlayerName;
            player.playerColor = playerCustomizableDatas[player.netId].PlayerColor;
        }
    }
}
}