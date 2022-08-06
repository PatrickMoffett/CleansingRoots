using System;
using UnityEngine;

public class PlayerAnimationEventManager : MonoBehaviour
{
    public Action swordAttackDealDamageAnimationEvent;
    public Action swordAttackEndedAnimationEvent;
    public ParticleSystem SwordTrail;
    
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
}
