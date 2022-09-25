using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {

        [SerializeField] private List<GameObject> drops;
        [SerializeField] private float dropChance = .5f;
        
        private Health _health;

        private void Start()
        {
            _health = GetComponent<Health>();

            _health.OnHealthIsZero += Die;
        }

        private void Die()
        {
            Destroy(gameObject);
            
            if (drops.Count > 0 && Random.value <= dropChance)
            {
                int index = Random.Range(0, drops.Count);
                Instantiate(drops[index], transform.position, Quaternion.identity);
            }

            
        }
    }
}