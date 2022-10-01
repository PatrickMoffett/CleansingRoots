using System.Collections.Generic;
using AI.BehaviorTree.Control.Decorator;
using UnityEngine;

namespace AI.BehaviorTree
{
    public class Selector : BaseComposite
    {
        public Selector ( ) : base(){}
        public Selector(List<BaseNode> children) : base (children){}

        public override NodeState Run()
        {
            for (int i = 0; i < currentChildIndex; i++)
            {
                if (childNodes[i] is not BaseDecorator) continue;
                
                BaseDecorator decorator = (BaseDecorator)childNodes[i];
                if (decorator.abortType is AbortType.LOWER_PRIORITY or AbortType.BOTH 
                    && decorator.ShouldAbortLowerPriority())
                {
                    Reset();
                    currentChildIndex = i;
                }
            }
            while (currentChildIndex < childNodes.Count)
            {
                switch (childNodes[currentChildIndex].Run())
                {
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        currentChildIndex++;
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        Debug.LogError("Unsupported State Reached");
                        break;
                }
            }
            Reset();
            state = NodeState.FAILURE;
            return state;
        }

        public override void Reset()
        {
            currentChildIndex = 0;

            foreach (var node in childNodes)
            {
                node.Reset();
            }
        }
    }
}