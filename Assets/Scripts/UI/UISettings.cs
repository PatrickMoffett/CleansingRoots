using System;
using Systems.AudioManager;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public void Setup()
    {
        AudioManager audioManager = ServiceLocator.Instance.Get<AudioManager>();
        masterVolumeSlider.value = audioManager.GetMasterVolume();
        musicVolumeSlider.value = audioManager.GetMusicVolume();
        sfxVolumeSlider.value = audioManager.GetSFXVolume();
        
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetMasterVolume(float value)
    {
        ServiceLocator.Instance.Get<AudioManager>().SetMasterVolume(value);
    }

    void SetMusicVolume(float value)
    {
        ServiceLocator.Instance.Get<AudioManager>().SetMusicVolume(value);
    }

    void SetSFXVolume(float value)
    {
        ServiceLocator.Instance.Get<AudioManager>().SetSFXVolume(value);
    }

    public void ExitSettingsMenuClicked()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
    }
}