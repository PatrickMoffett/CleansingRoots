using System;
using System.Collections.Generic;
using Cinemachine;
using Combat;
using UnityEngine;

namespace Player
{
    public class PlayerTargetingComponent : MonoBehaviour
    {
        //Targeting Variables
        public LayerMask targetingLayers = -1;
        public float targetingDistance = 15f;
        private bool _isTargeting = false;
        private ITargetable _currentTarget;
        private PlayerCameraComponent _playerCameraComponent;

        private void Start()
        {
            _playerCameraComponent = GetComponent<PlayerCameraComponent>();
        }

        public ITargetable GetCurrentTarget()
        {
            return _currentTarget;
        }

        private void OnDisable()
        {
            _currentTarget.TargetDestroyed -= CurrentTargetDestroyed;
        }

        /// <summary>
        /// Attempts to find an initial target and stores the result as the CurrentTarget and switches player camera to Targeting mode
        /// </summary>
        /// <returns>returns true if a target was found</returns>
        public void StartTargeting()
        {
            if (_isTargeting) { return; }
            
            //find all possible targets
            List<ITargetable> targetables = new List<ITargetable>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, targetingDistance, targetingLayers);
            foreach (var collider in colliders)
            {
                ITargetable targetable = collider.GetComponent<ITargetable>();
                if(targetable == null) continue;
                if (targetable.Targetable == false) continue;
                if (!IsOnScreen(targetable)) continue;
                targetables.Add(targetable);
            }
        
            //chose target based on distance from center of screen
            float closestDistance = Mathf.Infinity;
            float distance;
            ITargetable closestTargetable = null;
            foreach (var targetable in targetables)
            {
                distance = CalulateDistanceFromCenterOfScreen(targetable.TargetTransform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTargetable = targetable;
                }
            }
        
            //set target if not null and return
            if (closestTargetable != null)
            {
                _currentTarget = closestTargetable;
                _currentTarget.TargetDestroyed += CurrentTargetDestroyed;
                _isTargeting = true;
                _playerCameraComponent.SetTargetCameraLookAt(_currentTarget.TargetTransform);
                _playerCameraComponent.SetCurrentCameraMode(PlayerCameraMode.TargetLocked);
            }
        }

        private void CurrentTargetDestroyed()
        {
            if (!ChangeLockOnTarget(Vector2.left) && !ChangeLockOnTarget(Vector2.right))
            {
                StopTargeting();
            }
        }

        public void StopTargeting()
        {
            _currentTarget.TargetDestroyed -= CurrentTargetDestroyed;
            _isTargeting = false;
            _playerCameraComponent.SetCurrentCameraMode(PlayerCameraMode.Orbit);
        }
        
        /// <summary>
        /// Attempts to find a new target in the direction of changeTargetDirection
        /// </summary>
        /// <param name="changeTargetDirection">direction to look for the new target relative to the currentTarget</param>
        public bool ChangeLockOnTarget(Vector2 changeTargetDirection)
        {
            //if not currently targeting or current target is null, we can't find a new target
            if (!_isTargeting || _currentTarget == null) return false;
        
            //find all possible targets
            List<ITargetable> targetables = new List<ITargetable>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, targetingDistance, targetingLayers);
            foreach (var collider in colliders)
            {
                ITargetable targetable = collider.GetComponent<ITargetable>();
                if(targetable == null) continue;
                if (targetable == _currentTarget) continue;
                if (targetable.Targetable == false) continue;
                if (!IsOnScreen(targetable)) continue;
                targetables.Add(targetable);
            }
            
            
            //Using transform.right instead, it should be the same
            /*
            Vector3 currentTargetDirection = _currentTarget.TargetTransform.position - transform.position;
            currentTargetDirection = Vector3.Cross(Vector3.up,currentTargetDirection);
            currentTargetDirection.y = 0;
            currentTargetDirection.Normalize();
            */
            
            ITargetable nextTarget = null;
            float minDotProduct;
            
            //TODO: Maybe make this support looking up or down for new targets
            if (changeTargetDirection.x > 0) //if true find next target clockwise of current target
            {
                //find the smallest positive dot product
                minDotProduct = Mathf.Infinity;
                foreach (var targetable in targetables)
                {
                    Vector3 direction = targetable.TargetTransform.position - transform.position;
                    direction.y = 0;
                    direction.Normalize();
                    float dot = Vector3.Dot(transform.right, direction);
                    if (dot > 0 && dot <= minDotProduct)
                    {
                        minDotProduct = dot;
                        nextTarget = targetable;
                    }
                }
            }
            else //find target counterclockwise of the current target
            {
                //find the smallest negative dot product
                minDotProduct = Mathf.NegativeInfinity;
                foreach (var targetable in targetables)
                {
                    Vector3 direction = targetable.TargetTransform.position - transform.position;
                    direction.y = 0;
                    direction.Normalize();
                    float dot = Vector3.Dot(transform.right, direction);
                    if (dot < 0 && dot >= minDotProduct)
                    {
                        minDotProduct = dot;
                        nextTarget = targetable;
                    }
                }
            }
            //if a target was found set the new target and update the camera's target
            if (nextTarget != null)
            {
                _currentTarget.TargetDestroyed -= CurrentTargetDestroyed;
                _currentTarget = nextTarget;
                _currentTarget.TargetDestroyed += CurrentTargetDestroyed;
                _playerCameraComponent.SetTargetCameraLookAt(_currentTarget.TargetTransform);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if ITargetable object is visible to the main camera
        /// </summary>
        /// <param name="targetable">ITargetable to check</param>
        /// <returns>returns true if ITargetable is on screen</returns>
        private bool IsOnScreen(ITargetable targetable)
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(targetable.TargetTransform.position);
            if (viewportPosition.x < 0 || viewportPosition.x > 1) { return false; }
            if (viewportPosition.y < 0 || viewportPosition.y > 1) { return false; }
            if (viewportPosition.z < 0) { return false; }

            return true;
        }
        
        /// <summary>
        /// Projects the provided Vector3 from world space onto the camera plane and calculates the distance from the center
        /// </summary>
        /// <param name="position">The world space position to project</param>
        /// <returns>The distance from the center of the screen</returns>
        private float CalulateDistanceFromCenterOfScreen(Vector3 position)
        {
            float screenCenterX = Camera.main.pixelWidth/2;
            float screenCenterY = Camera.main.pixelHeight / 2;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            float xDelta = screenCenterX - screenPosition.x;
            float yDelta = screenCenterY - screenPosition.y;
            float hypotenuseLength = Mathf.Sqrt(Mathf.Pow(xDelta, 2) + Mathf.Pow(yDelta, 2));
            return hypotenuseLength;
        }
    }
}