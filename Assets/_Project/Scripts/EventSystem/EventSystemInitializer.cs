//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodlenite {
public class EventSystemInitializer : MonoBehaviour
{
    private void Start() => EventReferences.Initialize();
}
}