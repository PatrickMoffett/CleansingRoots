using System;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        enum Weapon
        {
            None,
            Sword,
            SlingShot,
        };

        public Animator animator;

        private Weapon _currentWeapon = Weapon.Sword;
        
        //Component to move the player
        private PlayerMovement _movementComponent;

        //Component to push Rigidbodies
        private RigidbodyPush _rigidbodyPush;

        //Component that finds targets
        private PlayerTargetingComponent _targetingComponent;

        private PlayerCameraComponent _cameraComponent;

        
        private void Start()
        {
            _movementComponent = GetComponent<PlayerMovement>();
            _targetingComponent = GetComponent<PlayerTargetingComponent>();
            _cameraComponent = GetComponent<PlayerCameraComponent>();
            _rigidbodyPush = GetComponent<RigidbodyPush>();
        }

        public void Move(Vector3 direction)
        {
            //rotate the input direction input by the camera yaw
            direction = Quaternion.Euler(0, _cameraComponent.GetMainCamera().transform.eulerAngles.y, 0) * direction;
            
            switch (_cameraComponent.GetCurrentCameraMode())
            {
                case PlayerCameraMode.Aiming:
                    //Rotate Character To Face Direction of Camera
                    
                    //Move Character Relative To Camera
                    
                    break;
                case PlayerCameraMode.Orbit:
                    //Rotate Character To Input Direction
                    _movementComponent.SetTargetDirection(direction);
                    _movementComponent.Rotate();
                    //Move Character Relative To Camera
                    _movementComponent.Move(transform.forward, direction.magnitude);
                    break;
                
                case PlayerCameraMode.TargetLocked:
                    //Rotate Character Towards Locked Target
                    _movementComponent.SetTargetDirection(_targetingComponent.GetCurrentTarget().TargetTransform.position - transform.position);
                    _movementComponent.Rotate();
                    //Move Character Relative To Target
                    _movementComponent.Move(direction,direction.magnitude);
                    break;
                
                default:
                    Debug.LogError("Unsupported CameraMode case hit during Move().");
                    break;
            }
        }
        
        public void Jump()
        {
            //if on ground, jump
        }
        
        public void ToggleLockOnTarget()
        {
            if (_cameraComponent.GetCurrentCameraMode() != PlayerCameraMode.TargetLocked)
            {
                //Try to lock onto a target
                _targetingComponent.StartTargeting();
            }
            else
            {
                //Unlock from target
                _targetingComponent.StopTargeting();
            }
        }
        
        public void ChangeLockOnTarget(Vector2 changeTargetDirection)
        {
            if (_cameraComponent.GetCurrentCameraMode() != PlayerCameraMode.TargetLocked) { return; }
            
            //Try to find a new target in the direction hit
            _targetingComponent.ChangeLockOnTarget(changeTargetDirection);
            
        }
        
        public void AimPressed()
        {
            //TODO: Also don't aim if in air
            //Don't aim if SlingShot isn't equipped
            if (_currentWeapon != Weapon.SlingShot)
            {
                return;
            }
            
            //TODO: Set Animation to Aiming
            _cameraComponent.SetCurrentCameraMode(PlayerCameraMode.Aiming);
        }

        public void AimReleased()
        {
            if (_cameraComponent.GetCurrentCameraMode() != PlayerCameraMode.Aiming)
            {
                return;
            }
            
            //TODO: turn off aiming Animation
            _cameraComponent.SetCurrentCameraMode(PlayerCameraMode.Orbit);

        }

        public void Attack()
        {
            switch (_currentWeapon)
            {
                case Weapon.None:
                    //if no weapon is equipped, Equip the Sword, then attack
                    break;
                case Weapon.Sword:
                    //Attack with the Sword
                    SwordAttack();
                    break;
                case Weapon.SlingShot:
                    //Attack with the SlingShot
                    SlingShotAttack();
                    break;
                default:
                    Debug.LogError("Unsupported Weapon case hit during Attack().");
                    break;
            }
        }

        private void SwordAttack()
        {
            
            if (animator.GetBool("IsAttacking"))
            {
                animator.SetBool("IsAttacking",false);
            }
            else
            {
                animator.SetBool("IsAttacking",true);
            }
        }

        private void SlingShotAttack()
        {
            
        }
    }
}
