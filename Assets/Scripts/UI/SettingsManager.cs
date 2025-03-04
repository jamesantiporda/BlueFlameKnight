using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSliders();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliders();
    }

    private void UpdateSliders()
    {
        // Update Master Slider
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            if (PlayerPrefs.GetFloat("MasterVolume") != masterSlider.value)
            {
                masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            }
        }

        // Udate Music Slider
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            if (PlayerPrefs.GetFloat("MusicVolume") != musicSlider.value)
            {
                musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            }
        }

        // Update SFX Slider
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            if (PlayerPrefs.GetFloat("SFXVolume") != sfxSlider.value)
            {
                sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            }
        }
    }
}
