using UnityEngine;

namespace Enemies.AttackComponents
{
    public abstract class BaseAttackComponent : MonoBehaviour
    {
        public abstract bool CanAttack();
        public abstract bool Attack();
    }
}