using System;
using System.Collections.Generic;
using Systems.AudioManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactables.Misc
{
    public class DestructableContainer : MonoBehaviour, IDamageable
    {
        [SerializeField] private List<GameObject> drops = new List<GameObject>();
        [SerializeField] private float dropChance = .5f;
        [SerializeField] private Vector3 dropSpawnOffset = Vector3.zero;
        [SerializeField] private GameObject BrokenVersion;
        [SerializeField] private GameObject VFX;
        [SerializeField] private Vector3 VFXOffset = Vector3.zero;
        [SerializeField] private AudioClip breakSFX;
        public void TakeDamage(int damage)
        {
            Destroy(gameObject);
            
            //Spawn Drop
            if (drops.Count > 0 && Random.value <= dropChance)
            {
                int index = Random.Range(0, drops.Count);
                Instantiate(drops[index], transform.position+ dropSpawnOffset, Quaternion.identity);
            }

            if (VFX != null)
            {
                Instantiate(VFX, transform.position + VFXOffset, Quaternion.identity);
            }

            if (BrokenVersion != null)
            {
                Instantiate(BrokenVersion, transform.position, transform.rotation);
            }

            if (breakSFX != null)
            {
                ServiceLocator.Instance.Get<AudioManager>().PlaySFX(breakSFX);
            }
        }
    }
}