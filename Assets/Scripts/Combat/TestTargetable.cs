using UnityEngine;

namespace Combat
{
    public class TestTargetable : MonoBehaviour, ITargetable
    {
        [Header("Targetable")] 
        [SerializeField]
        private bool _targetable = true;
        [SerializeField]
        private Transform _targetTransform;
        public bool Targetable => _targetable;

        public Transform TargetTransform => _targetTransform;
    }
}