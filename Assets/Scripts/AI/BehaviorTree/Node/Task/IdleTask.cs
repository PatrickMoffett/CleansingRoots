using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.BehaviorTree.Task
{
    public class IdleTask : BaseNode
    {
        private enum TimerState
        {
            NotStarted,
            Running,
            Finished
        }

        private TimerState _timerState = TimerState.NotStarted;
        private readonly float _idleTime;
        private Coroutine _idleCoroutine;
        
        
        public IdleTask(float idleTime)
        {
            _idleTime = idleTime;
        }
        public override NodeState Run()
        {
            switch (_timerState)
            {
                case TimerState.NotStarted:
                    owningTree.StartCoroutine(IdleTimer());
                    return NodeState.RUNNING;
                case TimerState.Running:
                    return NodeState.RUNNING;
                case TimerState.Finished:
                    _timerState = TimerState.NotStarted;
                    return NodeState.SUCCESS;
                default:
                    Debug.LogError("Unsupported TimerState hit in " + GetType());
                    break;
            }

            return NodeState.FAILURE;
        }

        public override void Reset()
        {
            if (_idleCoroutine != null)
            {
                owningTree.StopCoroutine(_idleCoroutine);
            }
            _timerState = TimerState.NotStarted;
        }

        IEnumerator IdleTimer()
        {
            _timerState = TimerState.Running;
            yield return new WaitForSeconds(_idleTime);
            _timerState = TimerState.Finished;
        }
    }
}