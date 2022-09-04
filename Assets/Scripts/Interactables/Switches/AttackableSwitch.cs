using UnityEngine;

namespace Interactables.Switches
{
    public class AttackableSwitch : MonoBehaviour, IDamageable
    {
        public RespondsToSwitch objectToSwitch;

        public void TakeDamage(int damage)
        {
            objectToSwitch.ToggleSwitch();
        }
    }
}
