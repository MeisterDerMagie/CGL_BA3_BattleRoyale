using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceRandomizer : MonoBehaviour
{
    [SerializeField, Range(-3, 3)]
    private float minPitch = 0.9f, maxPitch = 1.1f;

    [SerializeField, Range(0f, 1f)]
    private float minVolume = 0.9f, maxVolume = 1f;

    [SerializeField, HideInInspector]
    private AudioSource audioSource;
    
    [Button]
    public void Play()
    {
        float pitch = Random.Range(minPitch, maxPitch);
        float volume = Random.Range(minVolume, maxVolume);
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.Play();
    }
    
    #if UNITY_EDITOR
    private void OnValidate() => audioSource = GetComponent<AudioSource>();
    #endif
}
