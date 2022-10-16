using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Doodlenite{
public class SaveLoadAudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<string> audioMixerParametersToSave;

    private void Start() => LoadAudioSettings();

    [Button]
    public void SaveAudioSettings()
    {
        foreach (string param in audioMixerParametersToSave)
        {
            bool getParamSuccess = audioMixer.GetFloat(param, out float value);

            if (!getParamSuccess)
            {
                Debug.LogWarning($"Could not get audio mixer param \"{param}\"!");
                return;
            }
            
            PlayerPrefs.SetFloat(param, value);
        }
        
        PlayerPrefs.Save();
    }

    [Button]
    public void LoadAudioSettings()
    {
        foreach (string param in audioMixerParametersToSave)
        {
            float value = PlayerPrefs.GetFloat(param, 0f);
            bool setParamSuccess = audioMixer.SetFloat(param, value);

            if (!setParamSuccess)
            {
                Debug.LogWarning($"Could not set audio mixer param \"{param}\"!");
                return;
            }

            audioMixer.SetFloat(param, value);
        }
    }
}
}
