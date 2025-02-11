﻿using System.Collections;
using UnityEngine;

public class MovableDoor : RespondsToSwitch
{
        [SerializeField] private bool startOpen = false;
        [SerializeField] private Vector3 openTranslation = Vector3.zero;
        [SerializeField] private Vector3 openRotation = Vector3.zero;
        private Vector3 closePosition = Vector3.zero;
        private Vector3 closeRotation = Vector3.zero;
        [SerializeField] private float openTime = 1f;
        [SerializeField] private float closeTime = 1f;
        private float _currentPercentOpen = 0f;
        private Coroutine _coroutine;
        private bool _switchOn = false;

        private void Start()
        {
                closePosition = transform.position;
                closeRotation = transform.rotation.eulerAngles;
                if (startOpen)
                { 
                        SetCurrentPercentOpen(1f);
                }
                else
                {
                        SetCurrentPercentOpen(0f);
                }
        }

        public bool IsOpen()
        { 
                return _switchOn;
        }
        public override void SwitchOn ()
        {
                if (_coroutine != null)
                {
                        StopCoroutine(_coroutine);
                }
                _coroutine = StartCoroutine(OpenDoor());
                _switchOn = true;
        }

        public override void SwitchOff()
        {
                if (_coroutine != null)
                {
                        StopCoroutine(_coroutine);
                }
                _coroutine = StartCoroutine(CloseDoor());
                _switchOn = false;
        }

        public override void ToggleSwitch()
        {
                if (_switchOn)
                {
                        SwitchOff();
                }
                else
                {
                        SwitchOn();
                }
        }

        private void SetCurrentPercentOpen(float percentOpen)
        {
                transform.position = closePosition + Vector3.Lerp(Vector3.zero, openTranslation, percentOpen);
                transform.rotation = Quaternion.Euler(closeRotation) * Quaternion.Euler(Vector3.Lerp(Vector3.zero, openRotation, percentOpen));
                _currentPercentOpen = percentOpen;
        }
        private IEnumerator OpenDoor()
        {
                while (_currentPercentOpen < 1f)
                {
                        SetCurrentPercentOpen(_currentPercentOpen);
                        yield return null;
                        _currentPercentOpen += Time.deltaTime * 1/openTime;
                }
                SetCurrentPercentOpen(1f);
        }
        private IEnumerator CloseDoor()
        {
                while (_currentPercentOpen > 0f)
                {
                        SetCurrentPercentOpen(_currentPercentOpen);
                        yield return null;
                        _currentPercentOpen -= Time.deltaTime * 1/openTime;
                }
                SetCurrentPercentOpen(0f);
        }
}
