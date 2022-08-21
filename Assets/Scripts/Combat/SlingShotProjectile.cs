using System;
using System.Collections;
using System.Collections.Generic;
using Systems.AudioManager;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlingShotProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayers = -1;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject trail;
    [SerializeField] private List<AudioClip> hitSounds;
    static private Transform trailBucket;

    void Start() {
        if (trailBucket == null) {
            trailBucket = GameObject.Find("TemporaryBucket").transform;
        }
        transform.SetParent(trailBucket);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageableComponent = collision.gameObject.GetComponent<IDamageable>();
            if (damageableComponent != null)
            {
                damageableComponent.TakeDamage(damage);
            }

            PlayHitSound();
            Destroy(gameObject);
        }
    }

    private void PlayHitSound()
    {
        //TODO:Switch to play hit at position
        //ServiceLocator.Instance.Get<AudioManager>().PlaySFXAtLocation(hitSounds[Random.Range(0,hitSounds.Count)],transform.position);
        
        ServiceLocator.Instance.Get<AudioManager>().PlaySFX(hitSounds[Random.Range(0,hitSounds.Count)]);
    }
}
