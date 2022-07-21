using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Node.Task
{
    /// <summary>
    /// Moves the character to the waypoint until it is within the value stored by minimumDistanceKey
    /// returns NodeState.Running while moving towards waypoint
    /// returns NodeState.Failure if Keys don't have data
    /// returns NodeState.Success when gameobject is within minimumDistanceKey
    /// </summary>
    public class MoveToWaypoint : BaseNode
    {
        private readonly string _selfGameObjectKey;
        private readonly string _moveSpeedKey;
        private readonly string _minimumDistanceKey;
        private readonly string _waypointKey;

        /// <summary>
        /// Create a MoveToWaypoint Node
        /// </summary>
        /// <param name="selfGameObjectKey">(GameObject)Key of the GameObject To Move</param>
        /// <param name="moveSpeedKey">(float)Key of the speed to move</param>
        /// <param name="minimumDistanceKey">(float)Key of the distance to consider having arrived at the waypoint</param>
        /// <param name="waypointKey">(WaypointNode)Key of the waypoint to move towards</param>
        public MoveToWaypoint(string selfGameObjectKey, string moveSpeedKey, string minimumDistanceKey, string waypointKey)
        {
            _selfGameObjectKey = selfGameObjectKey;
            _moveSpeedKey = moveSpeedKey;
            _minimumDistanceKey = minimumDistanceKey;
            _waypointKey = waypointKey;
        }
        
        /// <summary>
        /// Move GameObject towards the waypoint
        /// </summary>
        /// <returns>
        /// @NodeState.Running if SelfGameObject was moved, but has not yet arrived at waypoint
        /// @NodeState.Failure if Keys don't have data
        /// @NodeState.Success when gameobject is within minimumDistanceKey
        /// </returns>
        public override NodeState Run()
        {
            if (!(owningTree.HasData(_moveSpeedKey) && owningTree.HasData(_minimumDistanceKey) &&
                owningTree.HasData(_waypointKey) && owningTree.HasData(_selfGameObjectKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType() );
                return NodeState.FAILURE;
            }

            GameObject selfGameObject = (GameObject)owningTree.GetData(_selfGameObjectKey);
            BaseNavigationNode targetTransform = (BaseNavigationNode)owningTree.GetData(_waypointKey);
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