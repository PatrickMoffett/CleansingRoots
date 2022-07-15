using System.Collections.Generic;
using UnityEngine;

namespace AI.BehaviorTree.Task
{
    public class SetNextPatrolWaypoint : BaseNode
    {
        private readonly string _waypointsKey;
        private readonly string _currentWaypointTransformKey;
        private int _currentWaypointIndex = 0;
        public SetNextPatrolWaypoint(string waypointsKey, string currentWaypointTransformKey)
        {
            _waypointsKey = waypointsKey;
            _currentWaypointTransformKey = currentWaypointTransformKey;
        }

        public override NodeState Run()
        {
            if (!(owningTree.HasData(_waypointsKey) && owningTree.HasData(_currentWaypointTransformKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType());
                return NodeState.FAILURE;
            }

            List<Transform> waypoints = (List<Transform>)owningTree.GetData(_waypointsKey);

            _currentWaypointIndex++;
            if (_currentWaypointIndex >= waypoints.Count)
            {
                _currentWaypointIndex = 0;
            }
            owningTree.SetData(_currentWaypointTransformKey,waypoints[_currentWaypointIndex]);
            return NodeState.SUCCESS;
        }

        public override void Reset()
        {

        }
    }
}