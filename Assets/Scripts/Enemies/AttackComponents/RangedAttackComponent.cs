using System;
using System.Collections;
using UnityEngine;

namespace Enemies.AttackComponents
{
    public class RangedAttackComponent : BaseAttackComponent
    {
        public float attackCooldownTime = 1f;
        public float projectileSpeed = 30f;
        public GameObject attackProjectilePrefab;
        public Transform projectileSpawnTransform;
        private AudioSource _audioSource;
        public AudioClip shootSound;
        static private Transform trailBucket;


        private bool _attackOnCooldown = false;
        private Coroutine _attackCoroutine;
        private void OnDisable()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
        }

        private void Start() {
            if (trailBucket == null) {
                trailBucket = GameObject.Find("TemporaryBucket").transform;
            }

            _audioSource = GetComponent<AudioSource>();
        }

        public override bool CanAttack()
        {
            return !_attackOnCooldown;
        }

        public override bool Attack()
        {
            if (!CanAttack()) {return false;}

            GameObject projectile = Instantiate(attackProjectilePrefab, projectileSpawnTransform.position, projectileSpawnTransform.rotation);
            projectile.transform.SetParent(trailBucket);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;
            _attackCoroutine = StartCoroutine(AttackCooldown());
            _audioSource.clip = shootSound;
            _audioSource.Play();
            
            return true;
        }

        IEnumerator AttackCooldown()
        {
            _attackOnCooldown = true;
            yield return new WaitForSeconds(attackCooldownTime);
            _attackOnCooldown = false;
        }
    }
}