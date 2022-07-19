using System.Collections.Generic;
using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Task
{
    public class SetNextWaypointFromList : BaseNode
    {
        private readonly string _waypointListKey;
        private readonly string _currentWaypointKey;
        private int _currentWaypointIndex = 0;
        public SetNextWaypointFromList(string waypointListKey, string currentWaypointKey)
        {
            _waypointListKey = waypointListKey;
            _currentWaypointKey = currentWaypointKey;
        }

        public override NodeState Run()
        {
            if (!(owningTree.HasData(_waypointListKey) && owningTree.HasData(_currentWaypointKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType());
                return NodeState.FAILURE;
            }

            List<WaypointNode> waypoints = (List<WaypointNode>)owningTree.GetData(_waypointListKey);
            
            _currentWaypointIndex++;
            if (_currentWaypointIndex >= waypoints.Count)
            {
                _currentWaypointIndex = 0;
            }
            owningTree.SetData(_currentWaypointKey,waypoints[_currentWaypointIndex]);
            return NodeState.SUCCESS;
        }

        public override void Reset()
        {

        }
    }
}