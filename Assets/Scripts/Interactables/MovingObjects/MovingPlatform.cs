using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables.MovingObjects
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour
    {
        public List<GameObject> wayPoints;

        public float speed = 10f;

        public float pauseTime = 1f;
        public bool shouldPause = true;
        public bool waypointsFormCircle = false;

        private Rigidbody _rigidbody;

        public bool startAtFirstWaypoint = true;
        private int _currentWaypointIndex = 0;
        private bool _movingForwards = true;
        private bool _paused = false;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            if (startAtFirstWaypoint)
            {
                _rigidbody.position = wayPoints[0].transform.position;
                SetNextWaypoint();
            }
        }

        private void SetNextWaypoint()
        {
            if (_movingForwards)
            {
                _currentWaypointIndex++;
            
                if (_currentWaypointIndex < wayPoints.Count) return;
            
                if (waypointsFormCircle)
                {
                    _currentWaypointIndex = 0;
                }
                else
                {
                    _currentWaypointIndex -= 2;
                }
            }
            else
            {
                _currentWaypointIndex--;
            
                if (_currentWaypointIndex >= 0) return;
            
                if (waypointsFormCircle)
                {
                    _currentWaypointIndex = wayPoints.Count - 1;
                }
                else
                {
                    _currentWaypointIndex += 2;
                }
            }
        }

        private void Update()
        {
            if (_paused) return;
            MovePlatform();
        }

        private void MovePlatform()
        {
            Vector3 direction = wayPoints[_currentWaypointIndex].transform.position - transform.position;
            if (direction.magnitude <= speed * Time.deltaTime)
            {
                _rigidbody.position = wayPoints[_currentWaypointIndex].transform.position;
                if (shouldPause)
                {
                    StartCoroutine(WaitAndSetNextWaypoint());
                }
                else
                {
                    SetNextWaypoint();
                }

                return;
            }
            _rigidbody.position += direction.normalized * (speed * Time.deltaTime);
        }

        IEnumerator WaitAndSetNextWaypoint()
        {
            _paused = true;
            yield return new WaitForSeconds(pauseTime);
            SetNextWaypoint();
            _paused = false;
        }
    }
}
