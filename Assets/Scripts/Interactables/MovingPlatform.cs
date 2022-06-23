using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    public List<GameObject> wayPoints;

    public float speed = 10f;
    
    public bool waypointsFormCircle = false;
    public Collider stickCollider;

    private Rigidbody _rigidbody;

    public bool startAtFirstWaypoint = true;
    private int _currentWaypointIndex = 0;
    private bool _movingForwards = true;

    public GameObject connectedObject;
    
    // Start is called before the first frame update
    void Start()
    {
        stickCollider.isTrigger = true;
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

    private void FixedUpdate()
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
        if (connectedObject != null)
        {
            connectedObject.transform.position += direction.normalized * (speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        
    }

    private void OnCollisionExit(Collision col)
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        connectedObject = col.gameObject;

    }

    private void OnTriggerExit(Collider col)
    {
        connectedObject = null;
    }
}
