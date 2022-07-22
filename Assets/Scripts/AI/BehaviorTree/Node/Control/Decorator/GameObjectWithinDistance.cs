using UnityEngine;
using UnityEngine.Rendering.UI;

namespace AI.BehaviorTree.Control.Decorator
{
    public class GameObjectWithinDistance : BaseDecorator
    {
        private readonly string _distanceKey;
        private readonly string _selfGameObjectKey;
        private readonly string _targetGameObjectKey;
        
        public GameObjectWithinDistance(string distanceKey, string selfGameObjectKey, string targetGameObjectKey,AbortType abortType, BaseNode childNode) : base(abortType,childNode)
        {
            _distanceKey = distanceKey;
            _selfGameObjectKey = selfGameObjectKey;
            _targetGameObjectKey = targetGameObjectKey;
            this.abortType = abortType;
            this.childNode = childNode;
        }

        protected override bool EvaluateCondition()
        {
            //make sure data is exists
            if(!(owningTree.HasData(_selfGameObjectKey) && owningTree.HasData(_targetGameObjectKey) && owningTree.HasData(_distanceKey)))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType() );
                return false;
            }
            //get data from tree
            GameObject selfGameObject = (GameObject)owningTree.GetData(_selfGameObjectKey);
            GameObject targetGameObject = (GameObject)owningTree.GetData(_targetGameObjectKey);
            float maxDistance = (float)owningTree.GetData(_distanceKey);
            
            float sqrDistanceToGameObject = (targetGameObject.transform.position - selfGameObject.transform.position).sqrMagnitude;
            return sqrDistanceToGameObject <= maxDistance *maxDistance;
        }
    }
}