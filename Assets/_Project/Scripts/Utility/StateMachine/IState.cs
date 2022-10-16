//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=V75hgcsCGOM
//(https://forum.unity.com/threads/c-proper-state-machine.380612/)
namespace Wichtel.StateMachine {
public interface IState
{
    public void OnEnter();
    public void Tick();
    public void OnExit();
}
}