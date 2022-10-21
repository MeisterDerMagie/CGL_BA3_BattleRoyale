//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class JoinGame : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenPrefab;

    private NetworkManager manager;
    
    private void Awake() => manager = GetComponent<NetworkManager>();
    public void Join()
    {
        Timing.RunCoroutine(_Join());
    }

    private IEnumerator<float> _Join()
    {
        //Show loading screen for immediate UI feedback
        var loadingScreen = Instantiate(loadingScreenPrefab, Vector3.zero, Quaternion.identity, transform);
        
        //wait
        yield return Timing.WaitForSeconds(0.5f);
        
        //start client
        manager.StartClient();
        
        //wait
        yield return Timing.WaitForSeconds(1.5f);
        
        //hide loading screen
        Destroy(loadingScreen);
    } 
}
}