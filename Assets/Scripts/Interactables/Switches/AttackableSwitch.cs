using Systems.AudioManager;
using UnityEngine;

namespace Interactables.Switches
{
    public class AttackableSwitch : MonoBehaviour, IDamageable
    {
        public RespondsToSwitch objectToSwitch;

        public AudioClip hitSFX;
        public void TakeDamage(int damage)
        {
            objectToSwitch.ToggleSwitch();
            ServiceLocator.Instance.Get<AudioManager>().PlaySFX(hitSFX);
        }
    }
}
