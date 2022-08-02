using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FireHazard : RespondsToSwitch
{
    public ParticleSystem _particleSystem;
    private bool isSwitchedOn;
    
    public override void SwitchOn()
    {
        isSwitchedOn = true;
        _particleSystem.Play();
    }

    public override void SwitchOff()
    {
        isSwitchedOn = false;
        _particleSystem.Stop();
    }

    public override void ToggleSwitch()
    {
        if (isSwitchedOn)
        {
            SwitchOff();
        }
        else
        {
            SwitchOn();
        }
    }
}
