using System.Collections.Generic;
using UnityEngine;

namespace Interactables.Switches
{
    public class MultiSwitch : RespondsToSwitch
    {
        public List<RespondsToSwitch> objectsToSwitch = new List<RespondsToSwitch>();
        public override void SwitchOn()
        {
            foreach (var _switch in objectsToSwitch)
            {
                _switch.SwitchOn();
            }
        }

        public override void SwitchOff()
        {
            foreach (var _switch in objectsToSwitch)
            {
                _switch.SwitchOff();
            }
        }

        public override void ToggleSwitch()
        {
            foreach (var _switch in objectsToSwitch)
            {
                _switch.ToggleSwitch();
            }
        }
    }
}