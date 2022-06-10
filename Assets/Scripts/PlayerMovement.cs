using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 10f;
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
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f,targetAngle,0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _characterController.Move(moveDir.normalized * speed*Time.deltaTime);
        }
    }
}
