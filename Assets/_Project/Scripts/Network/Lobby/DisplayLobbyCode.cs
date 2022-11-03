using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using TMPro;
using UnityEngine;

public class DisplayLobbyCode : MonoBehaviour
{
    [SerializeField] private Transform lobbyCodeUI;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    
    private void Start()
    {
        var manager = FindObjectOfType<NetworkRoomManagerExt>();
        
        //hide lobby code if none exists (this will happen if you play locally)
        lobbyCodeUI.gameObject.SetActive(manager.lobbyCode != string.Empty);

        lobbyCodeText.SetText(manager.lobbyCode);
    }
}
