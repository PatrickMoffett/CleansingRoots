using System;
using UnityEngine;

namespace Interactables.Misc
{
    public class PlayerDetector : MonoBehaviour
    {
        public event Action OnPlayerDetected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerDetected?.Invoke();
            }
        }
    }
}