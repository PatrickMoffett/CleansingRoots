using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Cinemachine;
using Combat;
using Player;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    //Camera Variables
    public GameObject mainCamera;
    public GameObject targetingCamera;
    public Transform cameraTransform;
    
    //Editor Variables
#if UNITY_EDITOR
    public Transform debugTeleportTransform;
#endif
    
    //Player Variables
    private PlayerControls _playerControls;
    private PlayerMovement _playerMovement;
    private int health = 100; //FM: Feel free to move final player attributes
    
    //Targeting Variables
    public LayerMask targetingLayers = -1;
    public float targetingDistance = 15f;
    private bool _isTargeting = false;
    private ITargetable _currentTarget;


    private void Awake()
    {
        //create controls
        _playerControls = new PlayerControls();
        
        //bind controls
        _playerControls.Player.Attack.performed += AttackPressed;
        _playerControls.Player.Jump.performed += JumpPressed;
        _playerControls.Player.LockOnTarget.performed += ToggleLockOnPressed;
        _playerControls.Player.ChangeLockOnTarget.performed += ChangeLockOnTarget;
        _playerControls.Player.PauseMenu.performed += PausePressed;
#if UNITY_EDITOR
        _playerControls.Player.DebugTeleport.performed += DebugTeleportPressed;
#endif  
        
        //start with main camera
        targetingCamera.SetActive(false);
        mainCamera.SetActive(true);
        
        //get player movement component
        _playerMovement = GetComponent<PlayerMovement>();
    }
#if UNITY_EDITOR
    private void DebugTeleportPressed(InputAction.CallbackContext obj)
    {
        gameObject.transform.position = debugTeleportTransform.position;
    }
#endif
    
    private void PausePressed(InputAction.CallbackContext obj)
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(PauseMenuState));
    }

    private void ToggleLockOnPressed(InputAction.CallbackContext obj)
    {
        _isTargeting = !_isTargeting;
        //if we switched to targeting mode and found a target
        if (_isTargeting && FindTarget())
        {
            targetingCamera.GetComponent<CinemachineVirtualCamera>().LookAt = _currentTarget.TargetTransform;
            targetingCamera.SetActive(true);
            mainCamera.SetActive(false);
        }
        else
        {
            _isTargeting = false;
            mainCamera.SetActive(true);
            targetingCamera.SetActive(false);
        }
    }
    private void ChangeLockOnTarget(InputAction.CallbackContext obj)
    {
        if (!_isTargeting || _currentTarget == null) return;
        
        //find all possible targets
        List<ITargetable> targetables = new List<ITargetable>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, targetingDistance, targetingLayers);
        foreach (var collider in colliders)
        {
            ITargetable targetable = collider.GetComponent<ITargetable>();
            if(targetable == null) continue;
            if (targetable == _currentTarget) continue;
            if (targetable.Targetable == false) continue;
            if (!OnScreen(targetable)) continue;
            targetables.Add(targetable);
        }
        
        
        Vector3 currentTargetDirection = _currentTarget.TargetTransform.position - transform.position;
        currentTargetDirection = Vector3.Cross(Vector3.up,currentTargetDirection);
        currentTargetDirection.y = 0;
        currentTargetDirection.Normalize();

        ITargetable nextTarget = null;
        float minDotProduct;
        if (obj.ReadValue<float>() > 0) //if true find target to the right of current target
        {
            //find the smallest positive dot product
            minDotProduct = Mathf.Infinity;
            foreach (var targetable in targetables)
            {
                Vector3 direction = targetable.TargetTransform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                float dot = Vector3.Dot(currentTargetDirection, direction);
                if (dot > 0 && dot <= minDotProduct)
                {
                    minDotProduct = dot;
                    nextTarget = targetable;
                }
            }
        }
        else //find target to the left of the current target
        {
            //find the smallest negative dot product
            minDotProduct = Mathf.NegativeInfinity;
            foreach (var targetable in targetables)
            {
                Vector3 direction = targetable.TargetTransform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                float dot = Vector3.Dot(currentTargetDirection, direction);
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
            _currentTarget = nextTarget;
            targetingCamera.GetComponent<CinemachineVirtualCamera>().LookAt = _currentTarget.TargetTransform;
        }
    }
    private void JumpPressed(InputAction.CallbackContext obj)
    {
        _playerMovement.Jump();
    }

    private void AttackPressed(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
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
        //get inputDirection as Vector3
        Vector2 input = _playerControls.Player.Move.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(input.x, 0, input.y);
            
        if (_isTargeting)
        {
            //rotate the character towards the target transform
            _playerMovement.SetTargetDirection(_currentTarget.TargetTransform.position-transform.position);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "pickup")
        {
            HandlePickup(other.gameObject);
        }
    }

    private bool FindTarget()
    {
        //find all possible targets
        List<ITargetable> targetables = new List<ITargetable>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, targetingDistance, targetingLayers);
        foreach (var collider in colliders)
        {
            ITargetable targetable = collider.GetComponent<ITargetable>();
            if(targetable == null) continue;
            if (targetable.Targetable == false) continue;
            if (!OnScreen(targetable)) continue;
            targetables.Add(targetable);
        }
        
        //chose target based on distance,
        float closestDistance = Mathf.Infinity;
        float distance;
        ITargetable closestTargetable = null;
        foreach (var targetable in targetables)
        {
            distance = CalculateHypotenuse(targetable.TargetTransform.position);
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
            return true;
        }
        else
        {
            return false;
        }
    }

    private float CalculateHypotenuse(Vector3 position)
    {
        float screenCenterX = Camera.main.pixelWidth/2;
        float screenCenterY = Camera.main.pixelHeight / 2;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        float xDelta = screenCenterX - screenPosition.x;
        float yDelta = screenCenterY - screenPosition.y;
        float hypotenuseLength = Mathf.Sqrt(Mathf.Pow(xDelta, 2) + Mathf.Pow(yDelta, 2));
        return hypotenuseLength;
    }

    private bool OnScreen(ITargetable targetable)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(targetable.TargetTransform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1) { return false; }
        if (viewportPosition.y < 0 || viewportPosition.y > 1) { return false; }
        if (viewportPosition.z < 0) { return false; }

        return true;
    }

    private void HandlePickup(GameObject pickup)
    {
        Pickup castPickup = pickup.GetComponent<Pickup>();
        AudioSource pickupAudioSource = pickup.GetComponent<AudioSource>();
        if (castPickup != null)
        {
            switch (castPickup.category)
            {
                case Pickup.Category.Health:
                    health += castPickup.modifier;
                    break;
            }

            if (pickupAudioSource && pickupAudioSource.clip != null)
            {
                pickupAudioSource.Play();
            }

            Destroy(pickup, pickupAudioSource.clip.length);
        }
    }
}
