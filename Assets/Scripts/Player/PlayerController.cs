using System;
using Player;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject targetingCamera;
    public Transform targetTransform;
    public Transform cameraTransform;
    
    private PlayerControls _playerControls;
    private PlayerMovement _playerMovement;
    private bool _targetingMovementMode = false;

    private void Awake()
    {
        //create controls
        _playerControls = new PlayerControls();
        
        //start with main camera
        targetingCamera.SetActive(false);
        mainCamera.SetActive(true);
        
        //get player movement component
        _playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        if (_playerControls.Player.Jump.triggered)
        {
            _playerMovement.Jump();
        }
        //change camera if input pressed
        if (_playerControls.Player.LockOnTarget.triggered)
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
        //get inputDirection as Vector3
        Vector2 input = _playerControls.Player.Move.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(input.x, 0, input.y);
            
        if (_targetingMovementMode)
        {
            //rotate the character towards the target transform
            _playerMovement.SetTargetDirection(targetTransform.position-transform.position);
            //rotate the input by the characters current rotation and move in that direction
            inputDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * inputDirection;
            _playerMovement.Move(inputDirection,input.magnitude);
            _playerMovement.Rotate();
        }
        else
        {
            //rotate the input direction input by the camera yaw and rotate character towards the input direction
            inputDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * inputDirection;
            _playerMovement.SetTargetDirection(inputDirection);
            _playerMovement.Rotate();
            //move in the direction character is facing
            _playerMovement.Move(transform.forward, input.magnitude);
        }
    }
}
