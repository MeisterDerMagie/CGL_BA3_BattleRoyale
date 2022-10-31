//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Doodlenite {
public class UpdatePlayerSettingsUI : MonoBehaviour
{
    [SerializeField] private PlayerColor playerColor;
    [SerializeField] private TextMeshProUGUI playerNameDisplay;

    private Player player;
    
    private void Start()
    {
        player = GetComponent<Player>();
        player.OnPlayerSettingsChanged += UpdateUI;
    }

    private void UpdateUI()
    {
        //update player color
        playerColor.SetColor(player.playerColor);
        
        //update player name
        playerNameDisplay.SetText(player.playerName);
    }

    private void OnDestroy()
    {
        //unsubscribe from event
        if(player != null) player.OnPlayerSettingsChanged -= UpdateUI;
    }
}
}