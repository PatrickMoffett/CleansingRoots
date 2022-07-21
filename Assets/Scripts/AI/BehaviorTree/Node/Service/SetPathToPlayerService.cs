using System.Collections.Generic;
using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Service
{
    public class SetPathToPlayerService : BaseService
    {
        private readonly string _pathOriginGameObject;
        private readonly string _targetWaypointKey;
        private readonly string _waypointIndexKey;
        private readonly string _navPathKey;

        public SetPathToPlayerService(string pathOriginGameObject,string targetWaypointKey, string waypointIndexKey, string navPathKey, float serviceTickRate, BaseNode childNode) : base(serviceTickRate, childNode)
        {
            _pathOriginGameObject = pathOriginGameObject;
            _targetWaypointKey = targetWaypointKey;
            _waypointIndexKey = waypointIndexKey;
            _navPathKey = navPathKey;
        }
        protected override void ServiceFunction()
        {
            WaypointNavigationSystem navService = ServiceLocator.Instance.Get<WaypointNavigationSystem>();
            GameObject pathOrigin = (GameObject)owningTree.GetData(_pathOriginGameObject);
            List<BaseNavigationNode> path= navService.GetPath(pathOrigin.transform.position);
            if (path != null)
            {
                owningTree.SetData(_targetWaypointKey, path[0]);
                owningTree.SetData(_waypointIndexKey, 0);
                owningTree.SetData(_navPathKey, path);
            }
        }


    }
}