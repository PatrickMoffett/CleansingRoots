using System.Collections.Generic;
using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Task
{
    public class SetWaypointPathToPlayer : BaseNode
    {
        private readonly string _navPathKey;
        private readonly string _targetWaypointKey;
        public SetWaypointPathToPlayer(string navPathKey, string targetWaypointKey)
        {
            _navPathKey = navPathKey;
            _targetWaypointKey = targetWaypointKey;
        }
        public override NodeState Run()
        {
            WaypointNode currentTargetNode =(WaypointNode)owningTree.GetData(_targetWaypointKey);
            List<WaypointNode> path= ServiceLocator.Instance.Get<WaypointNavigationSystem>().GetPath(currentTargetNode);
            if (path != null)
            {
                Debug.Log(path[0].name);
                owningTree.SetData(_targetWaypointKey, path[0]);
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