using System;
using System.Collections;
using UnityEngine;

namespace Interactables.Switches
{
    public class TimerToggleSwitch : MonoBehaviour
    {
        public float initialDelay = 0f;
        public float toggleTime = 1f;
        public RespondsToSwitch switchableObject;

        private Coroutine _timedSwitchCoroutine;
        public void Start()
        {
            if (switchableObject != null)
            {
                _timedSwitchCoroutine = StartCoroutine(TimedSwitch());
            }
        }

        private void OnDestroy()
        {
            if (_timedSwitchCoroutine != null)
            {
                StopCoroutine(_timedSwitchCoroutine);
            }
        }

        IEnumerator TimedSwitch()
        {
            yield return new WaitForSeconds(initialDelay);
            while (true)
            {
                switchableObject.ToggleSwitch();
                yield return new WaitForSeconds(toggleTime);
            }
        }
    }
}