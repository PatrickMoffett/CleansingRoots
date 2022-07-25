using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TriggerVolumeSwitch : MonoBehaviour
{
    [SerializeReference] RespondsToSwitch switchableObject;
    private List<GameObject> _overLappingGameObjects = new List<GameObject>();
    private bool _isOn = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!_overLappingGameObjects.Contains(other.gameObject))
        {
            _overLappingGameObjects.Add(other.gameObject);
        }

        if (!_isOn)
        {
            switchableObject.SwitchOn();
            _isOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        _overLappingGameObjects.Remove(other.gameObject);
        if (_overLappingGameObjects.Count == 0)
        {
            switchableObject.SwitchOff();
            _isOn = false;
        }
    }
}
