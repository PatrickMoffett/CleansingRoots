using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 20f;
        public float rotationRate = 360f;
        public float jumpHeight = 2f;
        public float gravityScale = 1f;
        public float slopeForce = 1f;
        public float groundDistance = 1f;
        public Animator animator;


        private bool _isGrounded = false;
        private bool _onWalkableSlope = false;
        private bool _jumpStarted = false;
        private readonly float _gravity = -9.8f;
        private Vector3 _targetDirection = Vector3.forward;
        private Vector3 _velocity = Vector3.zero;
        private CharacterController _characterController;

        // idea is to use this as an accumulator while we're falling 
        // and take damage if it's over a certain value
        private float _terminalVelocity = -30.0f;


        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            UpdateGroundParameters();
            if (_jumpStarted && _velocity.y <= 0)
            {
                _jumpStarted = false;
            }            
        }

        public bool IsGrounded()
        {
            return _isGrounded;
        }

        public void UpdateGroundParameters()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, groundDistance))
            {
                if (!_isGrounded)//if just became grounded
                {
                    // check our landing velocity
                    if (_velocity.y < _terminalVelocity) {
                        // we should maybe set a flag or
                        // reduce health here?
                        Debug.Log("Ouch!");
                    }
                    _isGrounded = true;
                    animator.SetBool("HasJumped", false);
                    animator.SetBool("IsGrounded", true);
                }
                //update _onWalkable Slope
                if (Mathf.Acos(Vector3.Dot(hit.normal, Vector3.up)) * Mathf.Rad2Deg <= _characterController.slopeLimit && hit.normal != Vector3.up)
                {
                    _onWalkableSlope = true;
                }
                else
                {
                    _onWalkableSlope = false;
                }
            }
            else
            {
                if (_isGrounded)//if just left the ground
                {
                    _isGrounded = false;
                    _onWalkableSlope = false;
                    animator.SetBool("HasJumped", true);
                    animator.SetBool("IsGrounded", false);
                }
            }
        }
        
        /// <summary>
        /// Rotates toward the current TargetDirection
        /// </summary>
        /// <param name="useRotationRate">use RotationRate to clamp max rotation this frame</param>
        public void Rotate(bool useRotationRate = true)
        {
            if (useRotationRate)
            {
                //rotate the current forwards direction towards the target clamped by rotationRate
                transform.forward = Vector3.RotateTowards(transform.forward, _targetDirection,
                    rotationRate * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            }
            else
            {
                //set forward to the targetForwardDirection
                transform.forward = _targetDirection;
            }
        }
        public void Jump()
        {
            if (_isGrounded)
            {
                _jumpStarted = true;
                _velocity.y = Mathf.Sqrt(-2 * _gravity * gravityScale * jumpHeight);
                if (_onWalkableSlope)
                {
                    _velocity.y += slopeForce;
                }
                _characterController.Move(_velocity * Time.deltaTime);
            }
            else
            {
                Debug.Log("Jump Failed");
            }
        }

        /// <summary>
        /// Move the character
        /// </summary>
        /// <param name="moveDirection">direction to move the character</param>
        /// <param name="percentSpeed"> percent of moveSpeed to use this movement</param>
        public void Move(Vector3 moveDirection,float percentSpeed)
        {
            animator.SetFloat("Speed",percentSpeed);
            //set velocity
            _velocity = moveDirection * (moveSpeed * percentSpeed) + new Vector3(0, _velocity.y, 0);
        
            //increment vertical velocity with gravity
            _velocity.y += _gravity*gravityScale*Time.deltaTime;

            //move the character in the forward direction
            if (!_jumpStarted && _onWalkableSlope)
            {
                //apply additional downward force if on slope to prevent jumping down the slope
                _characterController.Move(_velocity * Time.deltaTime+ new Vector3(0,-slopeForce,0));
            }
            else
            {
                _characterController.Move(_velocity * Time.deltaTime);
            }

            //if character is grounded reset vertical velocity
            if (_characterController.isGrounded && _velocity.y < -2f)
            {
               _velocity.y = 0f;
               
            }            
        }
        
        /// <summary>
        /// returns the current TargetDirection
        /// </summary>
        /// <returns></returns>
        public Vector3 GetTargetDirection()
        {
            return _targetDirection;
        }
        
        /// <summary>
        /// Sets the current TargetDirection
        /// </summary>
        /// <param name="newTargetDirection">Direction to set</param>
        public void SetTargetDirection(Vector3 newTargetDirection)
        {
            _targetDirection = newTargetDirection;
            _targetDirection.y = 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down*groundDistance);
        }
    }
}
