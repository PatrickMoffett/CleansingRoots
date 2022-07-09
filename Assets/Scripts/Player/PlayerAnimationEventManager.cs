using System;
using UnityEngine;

public class PlayerAnimationEventManager : MonoBehaviour
{
    public Action swordAttackDealDamageAnimationEvent;
    public Action swordAttackEndedAnimationEvent;
    
    public void SwordAttackDealDamage()
    {
        swordAttackDealDamageAnimationEvent?.Invoke();
    }

    public void SwordAnimationEnded()
    {
        swordAttackEndedAnimationEvent?.Invoke();
    }
}
