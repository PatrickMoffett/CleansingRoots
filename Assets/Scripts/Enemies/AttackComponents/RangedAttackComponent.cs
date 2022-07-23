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
        

        private bool _attackOnCooldown = false;
        private Coroutine _attackCoroutine;
        private void OnDisable()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
        }

        public override bool CanAttack()
        {
            return !_attackOnCooldown;
        }

        public override bool Attack()
        {
            if (!CanAttack()) {return false;}

            GameObject projectile = Instantiate(attackProjectilePrefab, projectileSpawnTransform.position, projectileSpawnTransform.rotation);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;
            _attackCoroutine = StartCoroutine(AttackCooldown());
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