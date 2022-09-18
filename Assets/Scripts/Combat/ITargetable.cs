using System;
using UnityEngine;

namespace Combat
{
    public interface ITargetable
    {
        public event Action TargetDestroyed;
        public bool Targetable { get; }

        void ShowTargetedImage(bool show);
        public Transform TargetTransform { get; }
    }
}