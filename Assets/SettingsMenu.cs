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

    public AudioMixer MusicMixer;
    public GameObject MusicLabel;

    public AudioMixer SoundFXMixer;
    public GameObject SoundFXLabel;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

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
    }

    public void SetMusic (float music)
    {
        MusicMixer.SetFloat("Volume", music);
        MusicLabel.GetComponent<TMP_Text>().text = Math.Round(music * 100) + "%";
    }
    public void SetSoundFX(float soundFX)
    {
        SoundFXMixer.SetFloat("Volume", soundFX);
        SoundFXLabel.GetComponent<TMP_Text>().text = Math.Round(soundFX * 100) + "%";
    }

    public void Toggle()
    {
        bool toggle = !SettingsMenuUI.activeSelf;
        SettingsMenuUI.SetActive(toggle);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
