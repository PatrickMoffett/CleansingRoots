using System.Collections.Generic;
using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Node.Task
{
    public class SetWaypointToStartFromList : BaseNode
    {
        private readonly string _waypointListKey;
        private readonly string _waypointIndexKey;
        private readonly string _currentWaypointKey;
        
        public SetWaypointToStartFromList(string waypointListKey, string waypointIndexKey, string currentWaypointKey)
        {
            _waypointListKey = waypointListKey;
            _waypointIndexKey = waypointIndexKey;
            _currentWaypointKey = currentWaypointKey;
        }

        public override NodeState Run()
        {
            if (!(owningTree.HasData(_waypointListKey) && owningTree.HasData(_waypointIndexKey) && owningTree.HasData(_currentWaypointKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType());
                return NodeState.FAILURE;
            }

            List<BaseNavigationNode> waypoints = (List<BaseNavigationNode>)owningTree.GetData(_waypointListKey);
            if (waypoints.Count == 0)
            {
                return NodeState.FAILURE;
            }
            owningTree.SetData(_waypointIndexKey,0);
            owningTree.SetData(_currentWaypointKey,waypoints[0]);
            return NodeState.SUCCESS;
        }

        public override void Reset()
        {
            
        }
    }
}