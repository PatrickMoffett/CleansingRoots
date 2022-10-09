using System;
using System.Collections;
using System.Collections.Generic;
using Systems.AudioManager;
using UnityEngine;

public class SoundSwitch : RespondsToSwitch
{
    public AudioClip clipToPlay;
    private AudioSource _audioSource;
    private bool _isSwitched = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = clipToPlay;
    }

    public override void SwitchOn()
    {
        _audioSource.Play();
        _isSwitched = true;
    }

    public override void SwitchOff()
    {
        _audioSource.Play();
        _isSwitched = false;
    }

    public override void ToggleSwitch()
    {
        if (_isSwitched)
        {
            SwitchOff();
        }
        else
        {
            SwitchOn();
        }

    }
}
