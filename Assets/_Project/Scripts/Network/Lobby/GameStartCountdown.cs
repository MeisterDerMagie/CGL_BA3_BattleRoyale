//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using MEC;
using TMPro;
using UnityEngine;

namespace Doodlenite {
public class GameStartCountdown : MonoBehaviour
{
    [SerializeField] private float countdownDuration = 3.99f;
    [SerializeField] private Transform countdownUI;
    [SerializeField] private TextMeshProUGUI countdownNumberText;
    
    public static GameStartCountdown Instance;

    private float countdown;
    private Action callback;
    private CoroutineHandle coroutine;

    private void Awake()
    {
        Instance = this;
        HideCountdownUI();
    }

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
    }

    public void StopCountdown()
    {
        Timing.KillCoroutines(coroutine);

        HideCountdownUI();
    }

    private IEnumerator<float> _TickCountdown()
    {
        //update countdown
        countdown -= Time.deltaTime;
        
        //update UI
        countdownNumberText.SetText(Mathf.Floor(countdown).ToString(CultureInfo.InvariantCulture));
        
        //keep going as long as the counter is bigger than 0
        if(countdown > 0f) yield return Timing.WaitForOneFrame;
        
        //if countdown reached 0
        //hide countdown
        HideCountdownUI();
        
        //call callback to start the game
        callback?.Invoke();
    }

    private void HideCountdownUI() => countdownUI.gameObject.SetActive(false);
    private void ShowCountdownUI() => countdownUI.gameObject.SetActive(true);
}
}