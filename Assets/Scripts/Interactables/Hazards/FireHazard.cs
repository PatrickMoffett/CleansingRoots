using System;
using System.Collections;
using System.Collections.Generic;
using Systems.AudioManager;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FireHazard : RespondsToSwitch
{
    public ParticleSystem particleSystem;
    private AudioSource flameThrowerSFX;
    private Collider _collider;
    private bool _isSwitchedOn;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        flameThrowerSFX = GetComponent<AudioSource>();
    }

    public override void SwitchOn()
    {
        _isSwitchedOn = true;
        _collider.enabled = true;
        particleSystem.Play();
        flameThrowerSFX.Play();
        //ServiceLocator.Instance.Get<AudioManager>().PlaySFXAtLocation(flameThrowerSFX,transform.position);
    }

    public override void SwitchOff()
    {
        _isSwitchedOn = false;
        _collider.enabled = false;
        particleSystem.Stop();
        flameThrowerSFX.Stop();
    }

    public override void ToggleSwitch()
    {
        if (_isSwitchedOn)
        {
            SwitchOff();
        }
        else
        {
            SwitchOn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Hit Fire");
        }
    }
}
