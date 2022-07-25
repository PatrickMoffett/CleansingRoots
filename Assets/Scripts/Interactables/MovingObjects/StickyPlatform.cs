using System;
using UnityEngine;

namespace Interactables.MovingObjects
{
    public class StickyPlatform : MonoBehaviour
    {
        private GameObject _connectedObject;

        private Vector3 prevPosition;

        private void Start()
        {
            prevPosition = transform.position;
        }

        private void Update()
        {
            if (_connectedObject != null)
            {
                _connectedObject.transform.position += transform.position-prevPosition;
            }
            prevPosition = transform.position;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _connectedObject = col.gameObject;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _connectedObject = null;
            }
        }
    }
}