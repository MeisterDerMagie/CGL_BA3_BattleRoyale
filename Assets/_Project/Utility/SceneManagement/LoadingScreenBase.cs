//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace Wichtel.SceneManagement {
public abstract class LoadingScreenBase : MonoBehaviour
{
    public void HideLoadingScreen()
    {
        Timing.RunCoroutine(_HideAndDestroyLoadingScreen());
    }

    public abstract IEnumerator<float> _ShowLoadingScreen();

    protected abstract IEnumerator<float> _HideAndDestroyLoadingScreen();
}
}