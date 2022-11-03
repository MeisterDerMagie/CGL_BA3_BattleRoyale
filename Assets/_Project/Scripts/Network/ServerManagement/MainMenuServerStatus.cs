using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite.ServerProvider;
using MEC;
using UnityEngine;

public class MainMenuServerStatus : MonoBehaviour
{
    [SerializeField] private List<Transform> objectsOnlyVisibleIfServersOnline = new List<Transform>();
    [SerializeField] private List<Transform> objectsOnlyVisibleIfServersOffline = new List<Transform>();

    private CoroutineHandle coroutine;
    
    private void Start()
    {
        coroutine = Timing.RunCoroutine(_CheckServerStatusAndUpdateUI());
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines(coroutine);
    }

    private IEnumerator<float> _CheckServerStatusAndUpdateUI()
    {
        while (true)
        {
            bool isConnected = ServerProviderClient.Connected;

            foreach (Transform t in objectsOnlyVisibleIfServersOnline)
            {
                t.gameObject.SetActive(isConnected);
            }

            foreach (Transform t in objectsOnlyVisibleIfServersOffline)
            {
                t.gameObject.SetActive(!isConnected);
            }

            yield return Timing.WaitForSeconds(1f);
        }
    }
}
