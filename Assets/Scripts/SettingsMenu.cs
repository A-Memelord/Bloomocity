using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsMenu : MonoBehaviour
{
    public GameObject SettingsMenuUI;

    public AudioMixer musicMixer;
    public GameObject musicLabel;

    public AudioMixer soundFXMixer;
    public GameObject soundFXLabel;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public float musicVolume = 0.5f;
    public Slider musicSlider;

    public float soundFXVolume = 0.5f;
    public Slider soundFXSlider;

    public int quality;
    public TMP_Dropdown qualityDropdown;

    public int fullscreen;
    public Toggle fullscreenToggle;

    public void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundFXSlider.value = PlayerPrefs.GetFloat("soundFXVolume");
        qualityDropdown.value = PlayerPrefs.GetInt("quality");
        fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") == 0;

    }

    private void Update()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        musicLabel.GetComponent<TMP_Text>().text = Math.Round(musicVolume * 100) + "%";
        musicMixer.SetFloat("Volume", musicVolume);

        PlayerPrefs.SetFloat("soundFXVolume", soundFXVolume);
        soundFXLabel.GetComponent<TMP_Text>().text = Math.Round(soundFXVolume * 100) + "%";
        soundFXMixer.SetFloat("Volume", soundFXVolume);

        PlayerPrefs.SetInt("quality", quality);
        QualitySettings.SetQualityLevel(quality);

        PlayerPrefs.SetInt("fullscreen", fullscreen);

        if (fullscreen == 0)
        {
            fullscreenToggle.isOn = true;
        }
        else if (fullscreen == 1)
        {
            fullscreenToggle.isOn = false;
        }
        Screen.fullScreen = fullscreenToggle;
    }

    public void SetMusic (float music)
    {
        musicVolume = music;
    }
    public void SetSoundFX(float soundFX)
    {
        soundFXVolume = soundFX;
    }

    public void Toggle()
    {
        bool toggle = !SettingsMenuUI.activeSelf;
        SettingsMenuUI.SetActive(toggle);
    }

    public void SetQuality(int qualityIndex)
    {
        quality = qualityIndex;
    }

    public void SetFullscreen(bool isFullscreen)
    {

        if (isFullscreen)
        {
            fullscreen = 0;
        }
        else if(!isFullscreen)
        {
            fullscreen = 1;
        }
            Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}