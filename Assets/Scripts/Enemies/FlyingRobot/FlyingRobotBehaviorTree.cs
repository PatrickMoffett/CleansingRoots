using System;
using System.Collections.Generic;
using AI.BehaviorTree;
using AI.BehaviorTree.Control.Decorator;
using AI.BehaviorTree.Node.Control.Composite;
using AI.BehaviorTree.Node.Task;
using AI.BehaviorTree.Task;
using AI.WaypointNavigation;
using UnityEngine;

namespace Enemies.FlyingRobot
{
    public class FlyingRobotBehaviorTree : BehaviorTree
    {
        public float moveSpeed = 5f;
        public float aggroRange = 40f;
        public float attackDistance = 30f;
        public float patrolDistanceTolerance = .2f;
        public GameObject playerGameObject;
        public List<WaypointNode> patrolWaypoints;

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
            SetData(_waypointIndexKey,0);
            SetData(_selfKey,gameObject);
            SetData(_playerKey, playerGameObject);
            SetData(_moveSpeedKey, moveSpeed);
            SetData(_aggroRangeKey, aggroRange);
            SetData(_attackDistanceKey, attackDistance);
            SetData(_patrolMinimumDistanceKey,patrolDistanceTolerance);
            return new Selector(new List<BaseNode>
            {
                new GameObjectWithinDistance(_aggroRangeKey,_selfKey,_playerKey,AbortType.LOWER_PRIORITY,
                new Sequence(new List<BaseNode>
                            {
                                new SetWaypointPathToPlayer(_navPathKey,_waypointIndexKey,_targetWaypointKey),
                                new RepeatUntilFail(
                                    new Sequence(new List<BaseNode>{
                                                new MoveToWaypoint(_selfKey, _moveSpeedKey,_patrolMinimumDistanceKey,_targetWaypointKey),
                                                new SetNextWaypointFromList(_navPathKey,_waypointIndexKey,_targetWaypointKey)
                                            })
                                    )
                                //Move into range
                                //Face Player
                                //Perform Range Attack
                            })
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
            
            Gizmos.color = Color.green;
            List<WaypointNode> path = (List<WaypointNode>)GetData(_navPathKey);
            if (path != null)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    Gizmos.DrawLine(path[i-1].transform.position, path[i].transform.position);
                }
            }
            
            //draw current Target Node path
            Gizmos.color = Color.magenta;
            WaypointNode targetNode = (WaypointNode)GetData(_targetWaypointKey);
            if (targetNode != null)
            {
                Gizmos.DrawLine(transform.position,targetNode.transform.position);
            }
        }
    }
}