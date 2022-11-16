//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using MEC;
using Mirror;
using TMPro;
using UnityEngine;

namespace Doodlenite {
public class GameStartCountdown : NetworkBehaviour
{
    [SerializeField] private float countdownDuration = 4.99f;
    [SerializeField] private Transform countdownUI;
    [SerializeField] private TextMeshProUGUI countdownNumberText;
    
    public static GameStartCountdown Instance;

    [SyncVar] private float countdown;
    private Action callback;
    private CoroutineHandle coroutine;

    private void Awake()
    {
        Instance = this;
        HideCountdownUI();
    }

    [ClientRpc]
    private void StartCountdownOnClient() => ShowCountdownUI();

    [ClientRpc]
    private void StopCountdownOnClient() => HideCountdownUI();

    private void Update()
    {
        //only run on client
        if (isServer) return;
        //update ui
        countdownNumberText.SetText(CountdownToString());
    }

    [Server]
    public void StartCountdown(Action _callback)
    {
        //cache callback
        callback = _callback;
        
        //reset countdown
        countdown = countdownDuration;
        
        //show UI
        ShowCountdownUI();

        //run coroutine
        coroutine = Timing.RunCoroutine(_TickCountdown());
        
        //show countdown on clients
        StartCountdownOnClient();
    }

    [Server]
    public void StopCountdown()
    {
        Timing.KillCoroutines(coroutine);

        HideCountdownUI();
        
        //stop countdown on clients
        StopCountdownOnClient();
    }

    private IEnumerator<float> _TickCountdown()
    {
        //keep going as long as the counter is bigger than 1
        while (countdown > 1f)
        {
            //update countdown
            countdown -= 1f;
        
            //update UI
            countdownNumberText.SetText(CountdownToString());
            
            yield return Timing.WaitForSeconds(1);
        }
        
        //if countdown reached 0
        //hide countdown
        HideCountdownUI();
        
        //inform server provider about the started game
        ServerProviderCommunication.Instance.ServerInGame();
        
        //call callback to start the game
        callback?.Invoke();
    }

    private void HideCountdownUI()
    {
        if (countdownUI == null) return;
        countdownUI.gameObject.SetActive(false);
    }

    private void ShowCountdownUI() => countdownUI.gameObject.SetActive(true);
    private string CountdownToString() => Mathf.Floor(countdown).ToString(CultureInfo.InvariantCulture);
}
}