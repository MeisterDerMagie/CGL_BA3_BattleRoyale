using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using UnityEngine;
using UnityEngine.Audio;
using Wichtel;

public class SetAudioMixerVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string parameterName;

    /// <param name="volume">volume between 0 and 1</param>
    public void SetVolume(float volume)
    {
        float decibel = AudioUtilities.LinearToDecibel(volume);
        audioMixer.SetFloat(parameterName, decibel);
    }
}
