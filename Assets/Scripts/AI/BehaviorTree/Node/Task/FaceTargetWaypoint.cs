using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Node.Task
{
    public class FaceTargetWaypoint : BaseNode
    {
        private readonly string _selfGameObjectKey;
        private readonly string _targetWaypointKey;

        public FaceTargetWaypoint(string selfGameObjectKey,string targetWaypointKey)
        {
            _selfGameObjectKey = selfGameObjectKey;
            _targetWaypointKey = targetWaypointKey;
        }
        public override NodeState Run()
        {
            if (!(owningTree.HasData(_targetWaypointKey) && owningTree.HasData(_selfGameObjectKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType() );
                return NodeState.FAILURE;
            }

            BaseNavigationNode targetNode = (BaseNavigationNode)owningTree.GetData(_targetWaypointKey);
            GameObject self = (GameObject)owningTree.GetData(_selfGameObjectKey);
            Vector3 direction = targetNode.gameObject.transform.position - self.transform.position;
            self.transform.forward = direction;
            return NodeState.SUCCESS;
        }

        public override void Reset()
        {

        }
    }
}