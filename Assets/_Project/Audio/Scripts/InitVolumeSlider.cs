//(c) copyright by Martin M. Klöckener
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Doodlenite {
public class InitVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string paramName;

    private void OnEnable()
    {
        bool getParamSuccess = audioMixer.GetFloat(paramName, out float value);

        if (!getParamSuccess)
        {
            Debug.LogWarning($"Could not get audio mixer param {paramName}");
            return;
        }

        slider.value = AudioUtilities.DecibelToLinear(value);
    }
}
}