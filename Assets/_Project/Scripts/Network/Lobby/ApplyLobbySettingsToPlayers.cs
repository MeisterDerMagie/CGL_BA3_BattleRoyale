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
        Debug.Log($"OnDictionaryChanged: op = {op.ToString()}, key = {key}, playerName = {item.PlayerName}");

        ApplySettings();
    }

    private void ApplySettings()
    {
        Debug.Log("ApplySettings");
        
        List<Player> players = FindObjectsOfType<Player>().ToList();
        
        //Debug
        string playersDebug = string.Empty;
        foreach (var player in players)
        {
            playersDebug += $"player: netId = {player.netId}, name = {player.playerName} \n";

        }
        Debug.Log($"players: {playersDebug}");

        Debug.Log($"customizableDatas: Count = {playerCustomizableDatas.Count}");
        string datasDebug = string.Empty;
        foreach (KeyValuePair<uint, PlayerCustomizableData> data in playerCustomizableDatas)
        {
            datasDebug += $"data: netId = {data.Key}, name = {data.Value.PlayerName} \n";
        }
        Debug.Log(datasDebug);
        //

        foreach (Player player in players)
        {
            if (!playerCustomizableDatas.ContainsKey(player.netId)) continue;

            player.playerName = playerCustomizableDatas[player.netId].PlayerName;
            player.playerColor = playerCustomizableDatas[player.netId].PlayerColor;
            
            player.ApplyPlayerSettings();
        }
    }
}
}