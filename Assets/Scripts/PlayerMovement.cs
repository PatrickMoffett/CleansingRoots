using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 20f;
    public float airControlSpeed = 5f;
    public float rotationRate = 360f;
    public float gravityScale = 1f;
    public float slopeForce = 1f;
    private bool _onWalkableSlope = false;
    private readonly float _gravity = -9.8f;
    private Vector3 _velocity = Vector3.zero;
    private PlayerControls _playerControls;
    private CharacterController _characterController;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _characterController = GetComponent<CharacterController>();
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
        Vector2 moveInput = _playerControls.Player.Move.ReadValue<Vector2>();
        Move(moveInput);

    }
    void Move(Vector2 moveInput)
    {
        //turn the character and set the forward velocity
        if (moveInput.magnitude > 0f)
        {
            //get target forwards direction
            Vector3 targetForwardDirection = new Vector3(moveInput.x, 0, moveInput.y);
            //rotate the target forwards direction input by the camera yaw
            targetForwardDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * targetForwardDirection;
            //rotate the current forwards direction towards the target clamped by rotationRate
            transform.forward = Vector3.RotateTowards(transform.forward, targetForwardDirection,
                rotationRate * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            //set velocity
            _velocity = transform.forward * (speed) + new Vector3(0, _velocity.y, 0);
        }
        else
        {
            //set velocity 0
            _velocity = new Vector3(0, _velocity.y, 0);
        }
        
        //increment vertical velocity with gravity
        _velocity.y += _gravity*gravityScale*Time.deltaTime;
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
}
