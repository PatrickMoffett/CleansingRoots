using System;
using UnityEngine;

namespace Interactables.Switches
{
    public class ColorSwitch : RespondsToSwitch
    {
        [SerializeField] private bool startSwitchedOn = false;
        [SerializeField] private Color onColor = Color.green;
        [SerializeField] private Color offColor = Color.red;
        private bool _isSwitched = false;
        private MeshRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            if (startSwitchedOn)
            {
                SwitchOn();
            }
            else
            {
                SwitchOff();
            }
        }

        public override void SwitchOn()
        {
            _renderer.material.color = onColor;
            _isSwitched = true;
        }

        public override void SwitchOff()
        {
            _renderer.material.color = offColor;
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
}