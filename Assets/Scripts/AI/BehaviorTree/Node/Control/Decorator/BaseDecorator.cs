using System.Data;
using UnityEditor.Experimental.GraphView;

namespace AI.BehaviorTree.Control.Decorator
{
    public enum AbortType
    {
        NONE,
        SELF,
        LOWER_PRIORITY,
        BOTH,
    }
    public abstract class BaseDecorator : BaseNode
    {
        protected BaseNode childNode;

        public AbortType abortType = AbortType.NONE;

        protected BaseDecorator(AbortType abortType,BaseNode childNode)
        {
            this.childNode = childNode;
            this.abortType = abortType;
        }

        public override NodeState Run()
        {
            //if the state is already running, check if we should self abort, then return the child node
            if (state == NodeState.RUNNING)
            {
                if (ShouldAbortSelf())
                {
                    state = NodeState.FAILURE;
                    return state;
                }
                state = childNode.Run();
                return state;
            }
            //if condition is true, run and return child, else return failure state
            if (EvaluateCondition())
            {
                state = childNode.Run();
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
        protected virtual bool ShouldAbortSelf()
        {
            if (abortType is AbortType.NONE or AbortType.LOWER_PRIORITY)
            {
                return false;
            }
            return !EvaluateCondition();
        }
        public virtual bool ShouldAbortLowerPriority()
        {
            if (abortType is AbortType.NONE or AbortType.SELF)
            {
                return false;
            }
            return EvaluateCondition();
        }

        protected abstract bool EvaluateCondition();

        public override void SetOwningTree(BehaviorTree tree)
        {
            base.SetOwningTree(tree);
            childNode?.SetOwningTree(tree);
        }

        public override void Reset()
        {
            state = NodeState.FAILURE;
            childNode?.Reset();
        }
    }
}