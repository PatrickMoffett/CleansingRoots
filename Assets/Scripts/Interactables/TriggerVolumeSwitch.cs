using System;
using UnityEngine;


[Serializable]
public class TriggerVolumeSwitch : MonoBehaviour
{
    [SerializeReference] RespondsToSwitch switchableObject;
    private void OnTriggerEnter(Collider other)
    {
        switchableObject.SwitchOn();
    }

    private void OnTriggerExit(Collider other)
    {
        switchableObject.SwitchOff();
    }
}
