using System;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private Health _health;

        private void Start()
        {
            _health = GetComponent<Health>();

            _health.OnHealthIsZero += Die;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}