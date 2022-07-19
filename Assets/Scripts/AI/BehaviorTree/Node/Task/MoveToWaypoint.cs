using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Task
{
    public class MoveToWaypoint : BaseNode
    {
        private readonly string _selfGameObjectKey;
        private readonly string _moveSpeedKey;
        private readonly string _minimumDistanceKey;
        private readonly string _waypointKey;

        public MoveToWaypoint(string selfGameObjectKey, string moveSpeedKey, string minimumDistanceKey, string waypointKey)
        {
            _selfGameObjectKey = selfGameObjectKey;
            _moveSpeedKey = moveSpeedKey;
            _minimumDistanceKey = minimumDistanceKey;
            _waypointKey = waypointKey;
        }
        
        public override NodeState Run()
        {
            if (!(owningTree.HasData(_moveSpeedKey) && owningTree.HasData(_minimumDistanceKey) &&
                owningTree.HasData(_waypointKey) && owningTree.HasData(_selfGameObjectKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType() );
                return NodeState.FAILURE;
            }

            GameObject selfGameObject = (GameObject)owningTree.GetData(_selfGameObjectKey);
            WaypointNode targetTransform = (WaypointNode)owningTree.GetData(_waypointKey);
            float speed = (float)owningTree.GetData(_moveSpeedKey);
            float minimumDistance = (float)owningTree.GetData(_minimumDistanceKey);
            
            Vector3 direction = targetTransform.transform.position - selfGameObject.transform.position;
            if (direction.sqrMagnitude <= minimumDistance*minimumDistance)
            {
                return NodeState.SUCCESS;
            }
            else
            {
                selfGameObject.transform.position += direction.normalized * (speed * Time.deltaTime);
                selfGameObject.transform.rotation = Quaternion.LookRotation(direction);
                return NodeState.RUNNING;
            }
        }

        public override void Reset()
        {

        }
    }
}