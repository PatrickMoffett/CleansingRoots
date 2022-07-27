using System.Collections.Generic;
using UnityEngine;

namespace Interactables.MovingObjects
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour
    {
        public List<GameObject> wayPoints;

        public float speed = 10f;
    
        public bool waypointsFormCircle = false;

        private Rigidbody _rigidbody;

        public bool startAtFirstWaypoint = true;
        private int _currentWaypointIndex = 0;
        private bool _movingForwards = true;

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
            MovePlatform();
        }

        private void MovePlatform()
        {
            Vector3 direction = wayPoints[_currentWaypointIndex].transform.position - transform.position;
            if (direction.magnitude <= speed * Time.deltaTime)
            {
                _rigidbody.position = wayPoints[_currentWaypointIndex].transform.position;
                SetNextWaypoint();
                return;
            }
            _rigidbody.position += direction.normalized * (speed * Time.deltaTime);
        }
    }
}
