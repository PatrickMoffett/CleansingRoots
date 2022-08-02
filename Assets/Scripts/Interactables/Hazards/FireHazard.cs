using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FireHazard : RespondsToSwitch
{
    public ParticleSystem particleSystem;
    private Collider _collider;
    private bool _isSwitchedOn;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public override void SwitchOn()
    {
        _isSwitchedOn = true;
        _collider.enabled = true;
        particleSystem.Play();
    }

    public override void SwitchOff()
    {
        _isSwitchedOn = false;
        _collider.enabled = false;
        particleSystem.Stop();
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
