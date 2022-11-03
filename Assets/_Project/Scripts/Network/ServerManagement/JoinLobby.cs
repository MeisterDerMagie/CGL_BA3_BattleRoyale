using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite.ServerProvider;
using TMPro;
using UnityEngine;

namespace Doodlenite {
public class JoinLobby : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private InputHelper inputHelper;
    
    [SerializeField] private Transform errorMessageUI;
    [SerializeField] private TextMeshProUGUI errorRasonText;
    
    private void Awake()
    {
        errorMessageUI.gameObject.SetActive(false);
        inputHelper.OnUserEnteredMessage += SendJoinRequest;
        ServerProviderClient.OnLobbyJoinFailed += OnLobbyJoinFailed;
    }

    private void OnDestroy()
    {
        inputHelper.OnUserEnteredMessage -= SendJoinRequest;
        ServerProviderClient.OnLobbyJoinFailed -= OnLobbyJoinFailed;
    }

    public void SendJoinRequest()
    {
        errorMessageUI.gameObject.SetActive(false);

        if (!ServerProviderClient.Connected)
        {
            Debug.LogWarning("Can't join lobby because the client is not connected to the server provider.");
            OnLobbyJoinFailed("Servers are not available.");
        }
        else
        {
            ServerProviderCommunication.Instance.JoinRequest(inputField.text.ToUpper());
        }
    }

    private void SendJoinRequest(string _lobbyCode)
    {
        errorMessageUI.gameObject.SetActive(false);
        
        if (!ServerProviderClient.Connected)
        {
            Debug.LogWarning("Can't join lobby because the client is not connected to the server provider.");
            OnLobbyJoinFailed("Servers are not available.");
        }
        else
        {
            ServerProviderCommunication.Instance.JoinRequest(inputField.text.ToUpper());
        }
    }

    private void OnLobbyJoinFailed(string _reason)
    {
        errorMessageUI.gameObject.SetActive(true);
        errorRasonText.SetText(_reason);
    }
}
}