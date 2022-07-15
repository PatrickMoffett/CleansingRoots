using UnityEngine;

namespace AI.BehaviorTree.Task
{
    public class MoveToTransform : BaseNode
    {
        private readonly string _selfGameObjectKey;
        private readonly string _moveSpeedKey;
        private readonly string _minimumDistanceKey;
        private readonly string _transformKey;

        public MoveToTransform(string selfGameObjectKey, string moveSpeedKey, string minimumDistanceKey, string transformKey)
        {
            _selfGameObjectKey = selfGameObjectKey;
            _moveSpeedKey = moveSpeedKey;
            _minimumDistanceKey = minimumDistanceKey;
            _transformKey = transformKey;
        }
        
        public override NodeState Run()
        {
            if (!(owningTree.HasData(_moveSpeedKey) && owningTree.HasData(_minimumDistanceKey) &&
                owningTree.HasData(_transformKey) && owningTree.HasData(_selfGameObjectKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType() );
                return NodeState.FAILURE;
            }

            GameObject selfGameObject = (GameObject)owningTree.GetData(_selfGameObjectKey);
            Transform targetTransform = (Transform)owningTree.GetData(_transformKey);
            float speed = (float)owningTree.GetData(_moveSpeedKey);
            float minimumDistance = (float)owningTree.GetData(_minimumDistanceKey);
            
            Vector3 direction = targetTransform.position - selfGameObject.transform.position;
            if (direction.sqrMagnitude <= minimumDistance*minimumDistance)
            {
                return NodeState.SUCCESS;
            }
            else
            {
                selfGameObject.transform.position += direction.normalized * (speed * Time.deltaTime);
                return NodeState.RUNNING;
            }
        }

        public override void Reset()
        {

        }
    }
}