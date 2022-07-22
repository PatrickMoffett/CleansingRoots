namespace AI.BehaviorTree.Control.Decorator
{
    public class RepeatUntilFail : BaseDecorator
    {
        public RepeatUntilFail(BaseNode childNode) : base(AbortType.NONE,childNode)
        {
        }
        public override NodeState Run()
        {
            NodeState childState = childNode.Run();
            if (childState == NodeState.FAILURE)
            {
                return childState;
            }
            else
            {
                return NodeState.RUNNING;
            }
        }

        protected override bool EvaluateCondition()
        {
            return false;
        }
    }
}