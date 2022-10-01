using AI.WaypointNavigation;
using Unity.Mathematics;
using UnityEngine;

namespace AI.BehaviorTree.Node.Task
{
    public class FaceTargetWaypoint : BaseNode
    {
        private readonly string _selfGameObjectKey;
        private readonly string _targetWaypointKey;
        private readonly string _turnSpeedKey;
        private const float Tolerance = .02f;

        public FaceTargetWaypoint(string selfGameObjectKey,string targetWaypointKey, string turnSpeedKey)
        {
            _selfGameObjectKey = selfGameObjectKey;
            _targetWaypointKey = targetWaypointKey;
            _turnSpeedKey = turnSpeedKey;
        }
        public override NodeState Run()
        {
            if (!(owningTree.HasData(_targetWaypointKey) && owningTree.HasData(_selfGameObjectKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType() );
                return NodeState.FAILURE;
            }

            //get all data
            BaseNavigationNode targetNode = (BaseNavigationNode)owningTree.GetData(_targetWaypointKey);
            GameObject self = (GameObject)owningTree.GetData(_selfGameObjectKey);
            float turnSpeed = (float)owningTree.GetData(_turnSpeedKey);
            
            Vector3 direction = (targetNode.gameObject.transform.position - self.transform.position).normalized;
            
            //if the angle to the target is less than the amount we can turn this frame, turn directly at the target
            if(Mathf.Acos(Vector3.Dot(self.transform.forward,direction)) < turnSpeed*Time.deltaTime*Mathf.Deg2Rad)
            {
                self.transform.forward = direction;
                return NodeState.SUCCESS;
            }
            float dot = Vector3.Dot(self.transform.right, direction);

            if (dot > 0)
            {
                self.transform.forward = Quaternion.AngleAxis(turnSpeed * Time.deltaTime, Vector3.up)*self.transform.forward;
            }
            else
            {
                self.transform.forward = Quaternion.AngleAxis(turnSpeed * Time.deltaTime, -Vector3.up)*self.transform.forward;
            }

            return NodeState.RUNNING;
        }

        public override void Reset()
        {

        }
    }
}