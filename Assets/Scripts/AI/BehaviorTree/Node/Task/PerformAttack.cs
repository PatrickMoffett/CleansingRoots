using Enemies.AttackComponents;
using UnityEngine;

namespace AI.BehaviorTree.Node.Task
{
    public class PerformAttack : BaseNode
    {
        private readonly string _attackComponentKey;

        public PerformAttack(string attackComponentKey)
        {
            _attackComponentKey = attackComponentKey;
        }

        public override NodeState Run()
        {
            if (!owningTree.HasData(_attackComponentKey))
            {

                return NodeState.FAILURE;
            }
            ((BaseAttackComponent)owningTree.GetData(_attackComponentKey))?.Attack();
            return NodeState.SUCCESS;
        }

        public override void Reset()
        {
        }
    }
}