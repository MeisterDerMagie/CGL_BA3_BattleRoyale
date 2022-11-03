using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using Doodlenite.ServerProvider;
using MEC;
using Mirror;
using UnityEngine;

public class ShutdownServerIfNoPlayersAreConnected : MonoBehaviour
{
    [SerializeField] private float disconnectTimeout = 10f;
    private NetworkRoomManager manager;

    private int activePlayersBefore = -1;
    
    #if UNITY_SERVER
    private void Start()
    {
        //only run this, if we're connected to the server provider. Otherwise we probably are a local server and don't want to shutdown
        if (!ServerProviderClient.Connected) return;

        manager = FindObjectOfType<NetworkRoomManager>();
        DontDestroyOnLoad(this);
        Timing.RunCoroutine(_DisconnectAfterXSecondsWithoutPlayers());
    }

    private IEnumerator<float> _DisconnectAfterXSecondsWithoutPlayers()
    {
        while (true)
        {
            if (manager.numPlayers > 0)
            {
                activePlayersBefore = manager.numPlayers;
                yield return Timing.WaitForSeconds(disconnectTimeout);
            }
            else
            {
                if (activePlayersBefore == 0)
                {
                    //stop server if after 10 seconds still no players are connected
                    Debug.Log("No players connected for more than 10 seconds: shutdown server.");
                    ServerProviderCommunication.Instance.ServerStopped();
                    ServerProviderClient.DisconnectClient();
                    Application.Quit();
                    yield break;
                }
                activePlayersBefore = manager.numPlayers;
                yield return Timing.WaitForSeconds(disconnectTimeout);
            }
        }
    }
    #endif
}
