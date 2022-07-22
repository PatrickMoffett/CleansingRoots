using AI.WaypointNavigation;
using UnityEngine;

namespace AI.BehaviorTree.Control.Decorator
{
    public class CurrentWaypointIsPlayer : BaseDecorator
    {
        private readonly string _currentTargetWaypoint;

        public CurrentWaypointIsPlayer(string currentTargetWaypoint, AbortType abortType,BaseNode childNode) : base(abortType,childNode)
        {
            _currentTargetWaypoint = currentTargetWaypoint;
        }

        protected override bool EvaluateCondition()
        {
            if (!owningTree.HasData(_currentTargetWaypoint))
            {
                Debug.Log(this.owningTree.name+ ": Missing Data for node type: " + GetType() );
                return false;
            }
            BaseNavigationNode currentTarget = (BaseNavigationNode)owningTree.GetData(_currentTargetWaypoint);
            
            //if the target isn't null and the nodes to player is 0, the node is the player node
            return currentTarget != null && currentTarget.nodesToPlayer == 0;
        }
    }
}