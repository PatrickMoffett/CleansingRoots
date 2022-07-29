using System;
using UnityEngine;

namespace Interactables.MovingObjects
{
    public class StickyPlatform : MonoBehaviour
    {
        private GameObject _connectedPlayer;

        private Vector3 _prevPosition;

        private void Start()
        {
            _prevPosition = transform.position;
        }

        private void Update()
        {
            if (_connectedPlayer != null)
            {
                _connectedPlayer.transform.position += transform.position-_prevPosition;
            }
            _prevPosition = transform.position;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _connectedPlayer = col.gameObject;
            }
            else
            {
                col.transform.parent = transform;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _connectedPlayer = null;
            }
            else
            {
                col.transform.parent = null;
            }
        }
    }
}