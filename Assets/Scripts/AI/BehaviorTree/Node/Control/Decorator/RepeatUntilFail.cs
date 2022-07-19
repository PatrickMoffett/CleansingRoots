namespace AI.BehaviorTree.Control.Decorator
{
    public class RepeatUntilFail : BaseDecorator
    {
        public RepeatUntilFail(BaseNode childNode)
        {
            base.childNode = childNode;
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

        public override void Reset()
        {
            childNode.Reset();
        }

        public override bool ShouldAbort()
        {
            return false;
        }
    }
}