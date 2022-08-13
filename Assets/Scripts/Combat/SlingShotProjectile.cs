using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayers = -1;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject hitSound;
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
            trail.transform.SetParent(trailBucket);
            hitSound.transform.SetParent(trailBucket);
            hitSound.SetActive(true);
            Destroy(gameObject);
        }
    }
}
