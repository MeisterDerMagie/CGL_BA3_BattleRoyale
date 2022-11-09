//(c) copyright by Martin M. Klöckener
using System;
using TMPro;
using UnityEngine;

namespace Doodlenite {
public class WinningScreen : MonoBehaviour
{
    [SerializeField] private Transform winningScreen;
    [SerializeField] private TextMeshProUGUI winningText;
    [SerializeField] private Transform leaveMatchScreen;
    
    private void Start()
    {
        Player.OnPlayerWon += ShowWinningScreen;
        winningScreen.gameObject.SetActive(false);
        leaveMatchScreen.gameObject.SetActive(false);
    }

    private void OnDestroy() => Player.OnPlayerWon -= ShowWinningScreen;

    private void ShowWinningScreen(Player _player)
    {
        //show winning screen
        winningScreen.gameObject.SetActive(true);
        
        //show leave match screen
        if(!leaveMatchScreen.gameObject.activeSelf) leaveMatchScreen.gameObject.SetActive(true);
        
        //set winning text depending on whether the local or another player has won.
        winningText.SetText(_player.isLocalPlayer ? "You win!" : $"{_player.playerName} wins!");
    }
}
}