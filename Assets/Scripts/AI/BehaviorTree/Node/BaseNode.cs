using System.Collections.Generic;

namespace AI.BehaviorTree
{
    public enum NodeState
    {
        FAILURE,
        SUCCESS,
        RUNNING
    }
    public abstract class BaseNode
    {
        protected NodeState state;
        protected BehaviorTree owningTree;
        public BaseNode(){}

        public abstract NodeState Run();
        public abstract void Reset();

        public virtual void SetOwningTree(BehaviorTree tree)
        {
            owningTree = tree;
        }
    }
}