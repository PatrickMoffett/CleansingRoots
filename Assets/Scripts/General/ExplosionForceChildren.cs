using System;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class ExplosionForceChildren : MonoBehaviour
    {
        public float force = 1f;
        public float explosionRadius = 1f;
        public Vector3 explosionOffset = Vector3.zero;
        
        
        private void Start()
        {
            Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].AddExplosionForce(force,transform.position + explosionOffset,explosionRadius);
            }
        }
    }
}