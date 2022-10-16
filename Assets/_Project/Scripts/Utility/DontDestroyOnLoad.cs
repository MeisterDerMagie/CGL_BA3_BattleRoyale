//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodlenite {
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake() => DontDestroyOnLoad(gameObject);
}
}