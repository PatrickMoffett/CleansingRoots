using System;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class TestTargetable : MonoBehaviour, ITargetable
    {
        [Header("Targetable")] 
        [SerializeField]
        private bool _targetable = true;
        [SerializeField]
        private Transform _targetTransform;

        [SerializeField] private Image TargetedImage;

        public event Action TargetDestroyed;
        public bool Targetable => _targetable;

        private void Start()
        {
            TargetedImage.enabled = false;
        }

        public void ShowTargetedImage(bool show)
        {
            TargetedImage.enabled = show;
        }

        private void Update()
        {
            Vector3 v = Camera.main.transform.position - TargetedImage.transform.position;
            v.x = v.z = 0.0f;
            TargetedImage.transform.LookAt( Camera.main.transform.position - v ); 
            TargetedImage.transform.Rotate(0,180,0);
        }

        public Transform TargetTransform => _targetTransform;

        private void OnDestroy()
        {
            TargetDestroyed?.Invoke();
        }
    }
}