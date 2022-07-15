using System.Collections.Generic;
using AI.BehaviorTree;
using AI.BehaviorTree.Control.Decorator;
using AI.BehaviorTree.Task;
using UnityEngine;

namespace Enemies.FlyingRobot
{
    public class FlyingRobotBehaviorTree : BehaviorTree
    {
        public GameObject playerGameObject;
        public List<Transform> patrolWaypoints;
        protected override BaseNode SetupTree()
        {
            if (patrolWaypoints.Count == 0)
            {
                Debug.LogError("No Waypoints Set For Behavior Tree on " + gameObject.name);
            }
            else
            {
                SetData("PatrolWaypoints",patrolWaypoints);
                SetData("CurrentPatrolTargetTransform",patrolWaypoints[0]);
            }
            
            SetData("Self",gameObject);
            SetData("Player", playerGameObject);
            SetData("PlayerTransform", playerGameObject.transform);
            SetData("MoveSpeed", 5f);
            SetData("AggroRange", 20f);
            SetData("DeaggroRange", 40f);
            SetData("AttackDistance", 10f);
            SetData("PatrolPointMinimumDistance",.2f);
            return new Selector(new List<BaseNode>
            {
                new GameObjectWithinDistance("AggroRange","Self","Player",AbortType.LOWER_PRIORITY, 
                    new GameObjectWithinDistance("DeaggroRange","Self","Player",AbortType.SELF,
                        new Sequence(new List<BaseNode>
                                {
                                    //Find Waypoint In Range of Player with LOS
                                    new MoveToTransform("Self", "MoveSpeed","AttackDistance","PlayerTransform")
                                    //Look at Player
                                    //Perform Range Attack
                                }))),
                new Sequence(new List<BaseNode>
                {
                    new MoveToTransform("Self", "MoveSpeed","PatrolPointMinimumDistance","CurrentPatrolTargetTransform"),
                    new SetNextPatrolWaypoint("PatrolWaypoints","CurrentPatrolTargetTransform"),
                    new IdleTask(2f)
                })
            });
        }
    }
}