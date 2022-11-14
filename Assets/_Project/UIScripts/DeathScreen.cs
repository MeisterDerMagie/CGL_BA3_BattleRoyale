//(c) copyright by Martin M. Klöckener
using System;
using UnityEngine;

namespace Doodlenite {
public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Transform deathScreen;
    [SerializeField] private Transform leaveMatchScreen;
    
    private void Start()
    {
        Player.OnPlayerDied += OnPlayerDied;
        deathScreen.gameObject.SetActive(false);
        leaveMatchScreen.gameObject.SetActive(false);

    }

    private void OnDestroy() => Player.OnPlayerDied -= OnPlayerDied;

    private void OnPlayerDied(Player _player)
    {
        if (!_player.isLocalPlayer) return;
    
        //show death screen
        ShowDeathScreen();
        
        //show leave match screen
        if(!leaveMatchScreen.gameObject.activeSelf) leaveMatchScreen.gameObject.SetActive(true);
    }

    private void ShowDeathScreen()
    {
        deathScreen.gameObject.SetActive(true);
    }

}
}