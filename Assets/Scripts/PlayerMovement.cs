using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 10f;
    public float rotationRate = 360f;
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
        if (moveInput.magnitude > 0f)
        {
            //get target forwards direction
            Vector3 targetForwardDirection = new Vector3(moveInput.x, 0, moveInput.y);
            //rotate the target forwards direction input by the camera yaw
            targetForwardDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0)*targetForwardDirection;
            //rotate the current forwards direction towards the target clamped by rotationRate
            transform.forward = Vector3.RotateTowards(transform.forward, targetForwardDirection, rotationRate*Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            //move the character in the forward direction
            _characterController.Move(transform.forward * (speed * Time.deltaTime));
        }
    }
}
