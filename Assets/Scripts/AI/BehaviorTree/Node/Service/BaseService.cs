using System.Collections;
using UnityEngine;

namespace AI.BehaviorTree.Service
{
    public abstract class BaseService : BaseNode
    {
        protected readonly BaseNode childNode;
        protected readonly float serviceTickRate;
        protected Coroutine serviceCoroutine;

        protected BaseService(float serviceTickRate, BaseNode childNode)
        {
            this.childNode = childNode;
            this.serviceTickRate = serviceTickRate;
        }

        ~BaseService()
        {
            if (serviceCoroutine != null)
            {
                owningTree.StopCoroutine(serviceCoroutine);
            }
        }
        public override NodeState Run()
        {
            if (state != NodeState.RUNNING)
            {
                serviceCoroutine = owningTree.StartCoroutine(Service());
            }

            state = childNode.Run();
            if (state != NodeState.RUNNING)
            {
                owningTree.StopCoroutine(serviceCoroutine);
            }
            return state;
        }

        public override void Reset()
        {
            state = NodeState.FAILURE;
        }

        private IEnumerator Service()
        {
            while (true)
            {
                ServiceFunction();
                yield return new WaitForSeconds(serviceTickRate);
            }
        }

        protected abstract void ServiceFunction();

        public override void SetOwningTree(BehaviorTree tree)
        {
            base.SetOwningTree(tree);
            childNode?.SetOwningTree(tree);
        }
    }
}