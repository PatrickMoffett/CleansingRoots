using System;
using Systems.AudioManager;
using UnityEngine;

public class PlayerAnimationEventManager : MonoBehaviour
{
    public Action swordAttackDealDamageAnimationEvent;
    public Action swordAttackEndedAnimationEvent;
    public ParticleSystem SwordTrail;
    public AudioClip footstep;

    public void SwordAttackDealDamage()
    {
        swordAttackDealDamageAnimationEvent?.Invoke();
    }

    public void SwordAnimationEnded()
    {
        swordAttackEndedAnimationEvent?.Invoke();
    }

    public void StartSwordTrail()
    {
        SwordTrail.Play();
    }

    public void EndSwordTrail()
    {
        SwordTrail.Stop();
    }
    public void FootStep()
    {
        ServiceLocator.Instance.Get<AudioManager>().PlaySFX(footstep);
    }
}
