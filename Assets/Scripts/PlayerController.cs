using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public float moveSpeed = 10f;

    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        //MoveAction.
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
    }
    
}
