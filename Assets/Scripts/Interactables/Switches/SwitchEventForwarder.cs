using System;
using UnityEngine;

namespace Interactables.Switches
{
    public class SwitchEventForwarder : RespondsToSwitch
    {
        public event Action<bool> NewSwitchState;
        [SerializeField]private bool initialSwitchState = false;
        
        private bool _switchState;

        private void Start()
        {
            _switchState = initialSwitchState;
        }

        public bool GetSwitchState()
        {
            return _switchState;
        }
        public override void SwitchOn()
        {
            if (_switchState) return;
            _switchState = true;
            NewSwitchState?.Invoke(_switchState);
        }

        public override void SwitchOff()
        {
            if (!_switchState) return;
            _switchState = false;
            NewSwitchState?.Invoke(_switchState);
        }

        public override void ToggleSwitch()
        {
            _switchState = !_switchState;
            NewSwitchState?.Invoke(_switchState);
        }
    }
}