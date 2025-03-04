using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        // Update Audio Mixer based on PlayerPrefs
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            float masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20f);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            audioMixer.SetFloat("soundFXVolume", Mathf.Log10(sfxVolume) * 20f);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20f);
        }
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);

        PlayerPrefs.SetFloat("MasterVolume", level);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);

        PlayerPrefs.SetFloat("SFXVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);

        PlayerPrefs.SetFloat("MusicVolume", level);
    }
}
