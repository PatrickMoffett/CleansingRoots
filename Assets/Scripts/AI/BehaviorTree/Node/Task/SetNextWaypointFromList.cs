using System.Collections.Generic;
using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Node.Task
{
    public class SetNextWaypointFromList : BaseNode
    {
        private readonly string _waypointListKey;
        private readonly string _waypointIndexKey;
        private readonly string _currentWaypointKey;
        public SetNextWaypointFromList(string waypointListKey, string waypointIndexKey, string currentWaypointKey)
        {
            _waypointListKey = waypointListKey;
            _waypointIndexKey = waypointIndexKey;
            _currentWaypointKey = currentWaypointKey;
        }

        public override NodeState Run()
        {
            if (!(owningTree.HasData(_waypointListKey) && owningTree.HasData(_waypointIndexKey)&&owningTree.HasData(_currentWaypointKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType());
                return NodeState.FAILURE;
            }

            List<BaseNavigationNode> waypoints = (List<BaseNavigationNode>)owningTree.GetData(_waypointListKey);
            int currentWaypointIndex = (int)owningTree.GetData(_waypointIndexKey);
            if (currentWaypointIndex+1 >= waypoints.Count)
            {
                return NodeState.FAILURE;
            }
            currentWaypointIndex++;
            owningTree.SetData(_waypointIndexKey,currentWaypointIndex);
            owningTree.SetData(_currentWaypointKey,waypoints[currentWaypointIndex]);
            return NodeState.SUCCESS;
        }

        public override void Reset()
        {
            
        }
    }
}