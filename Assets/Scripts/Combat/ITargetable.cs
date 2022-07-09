using System;
using UnityEngine;

namespace Combat
{
    public interface ITargetable
    {
        public bool Targetable { get; }
        public Transform TargetTransform { get; }
    }
}