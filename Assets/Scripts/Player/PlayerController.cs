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
            _playerControls.Player.AimDownSight.performed += Aim;
            _playerControls.Player.AimDownSight.canceled += StopAiming;
            _playerControls.Player.AimAxis.performed += AimAxis;
            
#if UNITY_EDITOR
            //Bind Debug Controls
            _playerControls.Player.DebugTeleport.performed += DebugTeleportPressed;
            _playerControls.Player.DebugUnlimitedAmmo.performed += DebugUnlimitedAmmo;
#endif

        }

        private void AimAxis(InputAction.CallbackContext obj)
        {
           _playerCharacter.Aim(obj.ReadValue<Vector2>());
        }

        private void StopAiming(InputAction.CallbackContext obj)
        {
            _playerCharacter.StopAiming();
        }

        private void Aim(InputAction.CallbackContext obj)
        {
            _playerCharacter.StartAiming();
        }

        private void SwapWeapon(InputAction.CallbackContext obj)
        {
            _playerCharacter.SwapWeapon();
        }

        private void PausePressed(InputAction.CallbackContext obj)
        {
            ApplicationStateManager asm = ServiceLocator.Instance.Get<ApplicationStateManager>();
            if (asm.GetCurrentState() == typeof(GameState))
            {
                ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(PauseMenuState));
            }
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
            GetComponent<CharacterController>().enabled = false;
            gameObject.transform.position = debugTeleportTransform.position;
            GetComponent<CharacterController>().enabled = true;
        }

        private void DebugUnlimitedAmmo(InputAction.CallbackContext obj)
        {
            _playerCharacter.unlimitedAmmoDebug = !_playerCharacter.unlimitedAmmoDebug;
        }
#endif
        
    }
}
