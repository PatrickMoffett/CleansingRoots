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

        public abstract bool ShouldAbort();

        public override void SetOwningTree(BehaviorTree tree)
        {
            base.SetOwningTree(tree);
            childNode.SetOwningTree(tree);
        }
    }
}