using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables.Switches
{
    public class AttackableSwitchTimed : MonoBehaviour, IDamageable
    {
        public RespondsToSwitch objectToSwitch;
        public float timeBeforeSwitchingBack;

        private Coroutine _coroutine;
        private bool _timerRunning = false;

        public void TakeDamage(int damage)
        {
            objectToSwitch.ToggleSwitch();
            if (_timerRunning)
            {
                StopCoroutine(_coroutine);
            }
            else
            {
                StartCoroutine(SwitchBack(timeBeforeSwitchingBack));
            }
        }

        IEnumerator SwitchBack(float timeToWait)
        {
            _timerRunning = true;
            yield return new WaitForSeconds(timeToWait);
            objectToSwitch.ToggleSwitch();
            _timerRunning = false;
        }
    }
}