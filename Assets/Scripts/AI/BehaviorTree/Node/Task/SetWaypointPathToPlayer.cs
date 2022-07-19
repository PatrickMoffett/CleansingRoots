using System.Collections.Generic;
using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Node.Task
{
    /// <summary>
    /// Generates a Waypoint List that is a path to the player
    /// Stores list in the tree data with the navPathKey
    /// Sets the First Waypoint
    /// </summary>
    public class SetWaypointPathToPlayer : BaseNode
    {
        private readonly string _navPathKey;
        private readonly string _waypointIndexKey;
        private readonly string _targetWaypointKey;
        public SetWaypointPathToPlayer(string navPathKey, string waypointIndexKey,string targetWaypointKey)
        {
            _navPathKey = navPathKey;
            _waypointIndexKey = waypointIndexKey;
            _targetWaypointKey = targetWaypointKey;
        }
        public override NodeState Run()
        {
            WaypointNode currentTargetNode =(WaypointNode)owningTree.GetData(_targetWaypointKey);
            List<WaypointNode> path= ServiceLocator.Instance.Get<WaypointNavigationSystem>().GetPath(currentTargetNode);
            if (path != null)
            {
                owningTree.SetData(_targetWaypointKey, path[0]);
                owningTree.SetData(_waypointIndexKey,0);
                owningTree.SetData(_navPathKey, path);
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }

        public override void Reset()
        {
            
        }
    }
}