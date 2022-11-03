using System;
using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using UnityEngine;

public class InformServerProviderAboutStartedGame : MonoBehaviour
{
    private void Awake()
    {
        ServerProviderCommunication.Instance.ServerInGame();
    }
}
