using System;
using Systems.AudioManager;
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
        
        [Header("Sound Properties")]
        [SerializeField] private AudioClip jumpSFX;

        private float _lastStandingHeight;

        private float maxFallDamageHeight = 40;

        private float minFallDamageHeight = 20;

        private float maxFallDamage = .5f;

        private float minFallDamage = .2f;
        
        
        private Health _health;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _health = GetComponent<Health>();
            _lastStandingHeight = transform.position.y;
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
            // detecting ground from .1 higher so that raycast isn't below the ground
            Ray ray = new Ray(transform.position + Vector3.up*.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, groundDistance))
            {
                
                float newGroundHeight = transform.position.y;
                if (!_isGrounded)//if just became grounded
                {
                    float fallDistance = _lastStandingHeight - newGroundHeight;
                    Debug.Log("Fall distance was " + fallDistance);
                    if (minFallDamageHeight < fallDistance && fallDistance < maxFallDamageHeight) {
                        float damageScale = (fallDistance - minFallDamageHeight) / (maxFallDamageHeight - minFallDamageHeight);
                        float damageAmtPct = minFallDamage + damageScale * (maxFallDamage - minFallDamage);
                        //Debug.Log("Damage scale " + damageScale);
                        Debug.Log("Damage Amount% " + damageAmtPct + " Damage scale " + damageScale + " Fall dist " + fallDistance);
                        _health.TakeDamage((int)(damageAmtPct * _health.GetMaxHealth()));
                    }  else if (fallDistance > maxFallDamageHeight) {
                        // fatal height
                        _health.TakeDamage((int)(_health.GetMaxHealth()));
                    }  
                    
                    _isGrounded = true;
                    animator.SetBool("HasJumped", false);
                    animator.SetBool("IsGrounded", true);
                }
                _lastStandingHeight = transform.position.y;
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
        public bool Jump()
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
                
                ServiceLocator.Instance.Get<AudioManager>().PlaySFX(jumpSFX);
                return true;
            }
            else
            {
                Debug.Log("Jump Failed");
                return false;
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
