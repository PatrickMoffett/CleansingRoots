using System.Collections.Generic;
using Cinemachine;
using Systems.AudioManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat
{
    public class DamageProjectile : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayers = -1;
        [SerializeField] private int damage = 1;
        [SerializeField] private GameObject trail;
        [SerializeField] private List<AudioClip> hitSounds = new List<AudioClip>();
        static private Transform trailBucket;
        private bool _hasCollided = false;

        void Start() {
            if (trailBucket == null) {
                trailBucket = GameObject.Find("TemporaryBucket").transform;
            }
            transform.SetParent(trailBucket);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!_hasCollided)
            {
                _hasCollided = true;
            }
            else
            {
                return;
            }
            Destroy(gameObject);
            
            if (gameObject.CompareTag("EnemyProjectile") && collision.gameObject.CompareTag("Boss"))
            {
                PlayHitSound();
                Destroy(gameObject);
                return;
            }
            IDamageable damageableComponent = collision.gameObject.GetComponent<IDamageable>();
            if (damageableComponent != null)
            {
                damageableComponent.TakeDamage(damage);
            }

            PlayHitSound();
        }

        private void PlayHitSound()
        {
            if (hitSounds.Count == 0) return; //no sounds to play
        
            //TODO:Switch to play hit at position
            //ServiceLocator.Instance.Get<AudioManager>().PlaySFXAtLocation(hitSounds[Random.Range(0,hitSounds.Count)],transform.position);
        
            ServiceLocator.Instance.Get<AudioManager>().PlaySFX(hitSounds[Random.Range(0,hitSounds.Count)]);
        }
    }
}
