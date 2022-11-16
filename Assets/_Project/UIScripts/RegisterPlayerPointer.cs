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
        // There is some bug.
        //Don't use this
        return;
        
        var player = GetComponent<Player>();
        PlayerPointerManager.Instance.RegisterPlayer(player);
    }
}
}