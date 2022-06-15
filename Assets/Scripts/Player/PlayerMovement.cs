using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public Transform cameraTransform;
        public float speed = 20f;
        public float rotationRate = 360f;
        public float gravityScale = 1f;
        public float slopeForce = 1f;
        public Transform targetTransform;
        public GameObject mainCamera;
        public GameObject targetingCamera;
    
        private bool _onWalkableSlope = false;
        private readonly float _gravity = -9.8f;
        private Vector3 _targetDirection = Vector3.forward;
        private Vector3 _velocity = Vector3.zero;

        private bool _targetingMovementMode;
        private PlayerControls _playerControls;
        private CharacterController _characterController;


        

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _characterController = GetComponent<CharacterController>();
            targetingCamera.SetActive(false);
        }
        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 input = _playerControls.Player.Move.ReadValue<Vector2>();
            //get target forwards direction
            Vector3 targetForwardDirection = new Vector3(input.x, 0, input.y);
            //rotate the target forwards direction input by the camera yaw
            targetForwardDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * targetForwardDirection;

            if (_playerControls.Player.Fire.triggered)
            {
                _targetingMovementMode = !_targetingMovementMode;
                if (_targetingMovementMode)
                {
                    targetingCamera.SetActive(true);
                    mainCamera.SetActive(false);
                }
                else
                {
                    mainCamera.SetActive(true);
                    targetingCamera.SetActive(false);
                }
            }
            
            
            if (_targetingMovementMode)
            { 
                SetTargetDirection(targetTransform.position-transform.position);
                Rotate();
                Move(targetForwardDirection,input.magnitude*speed);
            }
            else
            {
                SetTargetDirection(targetForwardDirection);
                Rotate();
                Move(transform.forward, input.magnitude * speed);
            }
        }

        /// <summary>
        /// Rotates toward the current TargetDirection
        /// </summary>
        /// <param name="useRotationRate">use RotationRate to clamp max rotation this frame</param>
        void Rotate(bool useRotationRate = true)
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
        /// <summary>
        /// Move the character
        /// </summary>
        /// <param name="moveDirection">direction to move the character</param>
        /// <param name="moveSpeed">speed to move the character (Before Applying deltaTime)</param>
        void Move(Vector3 moveDirection, float moveSpeed)
        {
            //set velocity
            _velocity = moveDirection * (moveSpeed) + new Vector3(0, _velocity.y, 0);
        
            //increment vertical velocity with gravity
            _velocity.y += _gravity*gravityScale*Time.deltaTime;
        
            //determine if character is on a slope
            if (_characterController.isGrounded)
            {
                Ray ray = new Ray(transform.position, Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit hit, _characterController.height*2))
                {
                    if (Mathf.Acos(Vector3.Dot(hit.normal, Vector3.up)) * Mathf.Rad2Deg <= _characterController.slopeLimit)
                    {
                        _onWalkableSlope = true;
                    }
                }
            }

            //move the character in the forward direction
            if (_onWalkableSlope)
            {
                //apply additional downward force if on slope to prevent jumping down the slope
                _characterController.Move(_velocity * Time.deltaTime+ new Vector3(0,-slopeForce,0));
            }
            else
            {
                _characterController.Move(_velocity * Time.deltaTime);
            }

            //if character is grounded reset vertical velocity
            if (_characterController.isGrounded)
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
    }
}
