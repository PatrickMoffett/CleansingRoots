using System;
using UnityEngine;

namespace Combat
{
    public interface ITargetable
    {
        public event Action TargetDestroyed;
        public bool Targetable { get; }
        public Transform TargetTransform { get; }
    }
}