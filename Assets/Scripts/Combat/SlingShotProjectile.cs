using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayers = -1;
    [SerializeField] private int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageableComponent = collision.gameObject.GetComponent<IDamageable>();
            if (damageableComponent != null)
            {
                damageableComponent.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
