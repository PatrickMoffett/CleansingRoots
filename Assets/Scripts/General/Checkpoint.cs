using System;
using Globals;
using UnityEngine;

namespace General
{
    public class Checkpoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Checkpoint Hit: " + name);
                GlobalVariables.checkpointName = name;
            }
        }
    }
}