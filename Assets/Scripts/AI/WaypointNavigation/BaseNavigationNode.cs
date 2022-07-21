using System;
using UnityEngine;

namespace AI.WaypointNavigation
{
    public abstract class BaseNavigationNode : MonoBehaviour
    {
        [NonSerialized] public int nodesToPlayer = Int32.MaxValue;
    }
}