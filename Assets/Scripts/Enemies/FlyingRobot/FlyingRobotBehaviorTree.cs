using System;
using System.Collections.Generic;
using AI.BehaviorTree;
using AI.BehaviorTree.Control.Decorator;
using AI.BehaviorTree.Node.Control.Composite;
using AI.BehaviorTree.Node.Task;
using AI.BehaviorTree.Service;
using AI.BehaviorTree.Task;
using AI.WaypointNavigation;
using Enemies.AttackComponents;
using UnityEngine;

namespace Enemies.FlyingRobot
{
    public class FlyingRobotBehaviorTree : BehaviorTree
    {
        public float moveSpeed = 5f;
        public float aggroRange = 40f;
        public float attackDistance = 30f;
        public float patrolDistanceTolerance = .2f;
        public float navigationUpdateRate = 1f;
        public GameObject playerGameObject;
        public List<BaseNavigationNode> patrolWaypoints;
        public float turnSpeed = 360f;
        
        private readonly string _selfKey = "Self";
        private readonly string _playerKey = "Player";
        private readonly string _moveSpeedKey = "MoveSpeed";
        private readonly string _aggroRangeKey = "AggroRange";
        private readonly string _attackDistanceKey = "AttackDistance";
        private readonly string _patrolMinimumDistanceKey = "PatrolMinimumDistance";
        private readonly string _waypointIndexKey = "WaypointIndexKey";
        private readonly string _patrolWaypointsKey = "PatrolWaypoints";
        private readonly string _targetWaypointKey = "TargetWaypoint";
        private readonly string _navPathKey = "NavPathList";
        private readonly string _attackComponentKey = "AttackComponent";
        private readonly string _turnSpeedKey = "TurnSpeedKey";

        protected override BaseNode SetupTree()
        {
            if (patrolWaypoints.Count == 0)
            {
                Debug.LogError("No Waypoints Set For Behavior Tree on " + gameObject.name);
                this.enabled = false;
                return null;
            }
            else
            {
                SetData(_patrolWaypointsKey,patrolWaypoints);
                SetData(_targetWaypointKey,patrolWaypoints[0]);
            }

            BaseAttackComponent attackComponent = GetComponent<BaseAttackComponent>();
            SetData(_attackComponentKey,attackComponent);
            SetData(_waypointIndexKey,0);
            SetData(_selfKey,gameObject);
            SetData(_playerKey, playerGameObject);
            SetData(_moveSpeedKey, moveSpeed);
            SetData(_aggroRangeKey, aggroRange);
            SetData(_attackDistanceKey, attackDistance);
            SetData(_patrolMinimumDistanceKey,patrolDistanceTolerance);
            SetData(_turnSpeedKey,turnSpeed);
            return new Selector(new List<BaseNode>
            {
                new GameObjectWithinDistance(_aggroRangeKey,_selfKey,_playerKey,AbortType.LOWER_PRIORITY,
                new SetPathToPlayerService(_selfKey,_targetWaypointKey,_waypointIndexKey,_navPathKey,navigationUpdateRate,
                        new Selector(new List<BaseNode>{
                                    new CurrentWaypointIsPlayer(_targetWaypointKey,AbortType.BOTH,
                                        new Sequence(new List<BaseNode> {
                                                    new GameObjectWithinDistance(_attackDistanceKey,_selfKey,_playerKey,AbortType.BOTH,
                                                        new RepeatUntilFail(
                                                            new Sequence(new List<BaseNode> {
                                                                        new FaceTargetWaypoint(_selfKey,_targetWaypointKey,_turnSpeedKey),
                                                                        new PerformAttack(_attackComponentKey),
                                                                        new IdleTask(1f)
                                                                })
                                                            )
                                                        ),
                                                    new MoveToWaypoint(_selfKey,_moveSpeedKey,_patrolMinimumDistanceKey,_targetWaypointKey),
                                            })
                                        ),
                                    new Sequence(new List<BaseNode> {
                                        new MoveToWaypoint(_selfKey, _moveSpeedKey,_patrolMinimumDistanceKey,_targetWaypointKey),
                                        new SetNextWaypointFromList(_navPathKey,_waypointIndexKey,_targetWaypointKey)
                                    })
                                })
                        )
                ),
                new Sequence(new List<BaseNode>{
                    new SetWaypointToStartFromList(_patrolWaypointsKey,_waypointIndexKey,_targetWaypointKey),
                    new RepeatUntilFail(
                        new Sequence(new List<BaseNode>
                        {
                                    new MoveToWaypoint(_selfKey, _moveSpeedKey,_patrolMinimumDistanceKey,_targetWaypointKey),
                                    new IdleTask(2f),
                                    new SetNextWaypointFromList(_patrolWaypointsKey,_waypointIndexKey,_targetWaypointKey),
                        })
                    )
                })
            });
        }

        private void OnDrawGizmos()
        {
            //draw patrol path
            Gizmos.color = Color.red;
            Gizmos.DrawLine(patrolWaypoints[0].transform.position, patrolWaypoints[patrolWaypoints.Count-1].transform.position);
            for (int i = 1; i < patrolWaypoints.Count; i++)
            {
                Gizmos.DrawLine(patrolWaypoints[i-1].transform.position, patrolWaypoints[i].transform.position);
            }
            
            //draw nav to player path
            Gizmos.color = Color.green;
            List<BaseNavigationNode> path = (List<BaseNavigationNode>)GetData(_navPathKey);
            if (path != null)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    Gizmos.DrawLine(path[i-1].transform.position, path[i].transform.position);
                }
            }
            
            //draw current Target Node path
            Gizmos.color = Color.magenta;
            BaseNavigationNode targetNode = (BaseNavigationNode)GetData(_targetWaypointKey);
            if (targetNode != null)
            {
                Gizmos.DrawLine(transform.position,targetNode.transform.position);
            }
        }
    }
}