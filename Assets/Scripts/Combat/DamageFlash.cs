using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField] private int numberOfFlashes;
        [SerializeField] private float flashSpeed;
        
        private MeshRenderer[] _renderers;
        private Color[] originalColor;
        private Coroutine _flashCoroutine;
        private void Start()
        {
            _renderers = GetComponentsInChildren<MeshRenderer>();
            originalColor = new Color[_renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                originalColor[i] = _renderers[i].material.color;
            }
        }

        public void FlashRed()
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }

            _flashCoroutine = StartCoroutine(FlashCoroutine());
        }

        IEnumerator FlashCoroutine()
        {
            int flashesCompleted = 0;
            while (flashesCompleted < numberOfFlashes)
            {
                float time = 0f;
                while (time < flashSpeed)
                {
                    time += Time.deltaTime;
                    for (int i = 0; i < _renderers.Length; i++)
                    {
                        _renderers[i].material.color =Color.Lerp(originalColor[i], Color.red, time / flashSpeed);
                    }
                    yield return null;
                }

                time = 0f;
                while (time < flashSpeed)
                {
                    time += Time.deltaTime;
                    for (int i = 0; i < _renderers.Length; i++)
                    {
                        _renderers[i].material.color =Color.Lerp( Color.red,originalColor[i], time / flashSpeed);
                    }
                    yield return null;
                }
                flashesCompleted++;
            }
        }
    }
}