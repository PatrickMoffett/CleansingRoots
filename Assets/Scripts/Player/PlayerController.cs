using System.Collections.Generic;
using Cinemachine;
using Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {

        //Player Variables
        private PlayerControls _playerControls;
        private PlayerCharacter _playerCharacter;

        private int health = 100; //FM: Feel free to move final player attributes


        private void Awake()
        {
            //create controls
            _playerControls = new PlayerControls();
            
            //get PlayerCharacter
            _playerCharacter = GetComponent<PlayerCharacter>();
        
            //bind controls
            _playerControls.Player.Attack.performed += AttackPressed;
            _playerControls.Player.Jump.performed += JumpPressed;
            _playerControls.Player.LockOnTarget.performed += ToggleLockOnPressed;
            _playerControls.Player.ChangeLockOnTarget.performed += ChangeLockOnTarget;
            _playerControls.Player.PauseMenu.performed += PausePressed;
            _playerControls.Player.SwapWeapon.performed += SwapWeapon;
            
            

#if UNITY_EDITOR
            //Bind Debug Controls
            _playerControls.Player.DebugTeleport.performed += DebugTeleportPressed;
#endif
            
        }

        private void SwapWeapon(InputAction.CallbackContext obj)
        {
            _playerCharacter.SwapWeapon();
        }

        private void PausePressed(InputAction.CallbackContext obj)
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(PauseMenuState));
        }

        private void ToggleLockOnPressed(InputAction.CallbackContext obj)
        {
            _playerCharacter.ToggleLockOnTarget();
        }
        private void ChangeLockOnTarget(InputAction.CallbackContext obj)
        {
            _playerCharacter.ChangeLockOnTarget(new Vector2(obj.ReadValue<float>(),0));
        }
        private void JumpPressed(InputAction.CallbackContext obj)
        {
            _playerCharacter.Jump();
        }

        private void AttackPressed(InputAction.CallbackContext ctx)
        {
            _playerCharacter.Attack();
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
            
            _playerCharacter.Move(inputDirection);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "pickup")
            {
                HandlePickup(other.gameObject);
            }
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
#if UNITY_EDITOR
        ///////////////////////////////////
        //Editor & Debug Variables
        //////////////////////////////////
        public Transform debugTeleportTransform;
        
        ///////////////////////////////////
        //Editor & Debug Methods
        //////////////////////////////////
        private void DebugTeleportPressed(InputAction.CallbackContext obj)
        {
            gameObject.transform.position = debugTeleportTransform.position;
        }
#endif
        
    }
}
