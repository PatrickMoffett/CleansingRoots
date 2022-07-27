using System.Collections.Generic;


namespace AI.BehaviorTree
{
    public abstract class BaseComposite : BaseNode
    {
        protected int currentChildIndex = 0;
        protected List<BaseNode> childNodes;

        protected BaseComposite(){}
        protected BaseComposite(List<BaseNode> childNodes)
        {
            this.childNodes = childNodes;
        }

        public override void SetOwningTree(BehaviorTree tree)
        {
            base.SetOwningTree(tree);
            foreach (var node in childNodes)
            {
                node.SetOwningTree(tree);
            }
        }

    }
}