//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Doodlenite.ServerProvider;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Doodlenite {
//attempt to reconnect to the server provider if the previous connection attemt wasn't successfull
public class AttemptServerProviderReconnect : MonoBehaviour
{
    [SerializeField] private float attemptInterval = 3f;
    private Thread connectionThread;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        ServerProviderClient.OnCouldNotConnectToServerProvider += OnConnectionFailed;
    }

    private void OnConnectionFailed()
    {
        //Timing.RunCoroutine(_WaitThenStartNewAttempt());
    }

    [Button]
    private void StartConnectionAttempt()
    {
        Debug.Log("Attempt reconnect to server provider.");
        
        //do nothing if the previous attempt is still waiting for a server response
        if (connectionThread != null && connectionThread.IsAlive)
            return;

        //start connection attempt in new thread
        var threadStart = new ThreadStart(ServerProviderClient.ConnectClient);
        connectionThread = new Thread(threadStart);
        connectionThread.Start();
    }

    private IEnumerator<float> _WaitThenStartNewAttempt()
    {
        yield return Timing.WaitForSeconds(attemptInterval);
        
        StartConnectionAttempt();
    }
}
}