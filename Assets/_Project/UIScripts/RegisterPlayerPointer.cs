//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodlenite {
public class RegisterPlayerPointer : MonoBehaviour
{
    private void Start()
    {
        var player = GetComponent<Player>();
        PlayerPointerManager.Instance.RegisterPlayer(player);
    }
}
}