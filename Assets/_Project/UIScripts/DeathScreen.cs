//(c) copyright by Martin M. Klöckener
using System;
using UnityEngine;

namespace Doodlenite {
public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Transform deathScreen;
    
    private void Start()
    {
        Player.OnPlayerDied += OnPlayerDied;
        deathScreen.gameObject.SetActive(false);
    }

    private void OnDestroy() => Player.OnPlayerDied -= OnPlayerDied;

    private void OnPlayerDied(Player _player)
    {
        if(_player.isLocalPlayer) ShowDeathScreen();
    }

    private void ShowDeathScreen()
    {
        deathScreen.gameObject.SetActive(true);
    }

}
}