using System;
using UnityEngine;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        public enum PlayerWeapon
        {
            None,
            Sword,
            SlingShot,
        };

        [SerializeField]private Animator animator;
        [SerializeField] private PlayerAnimationEventManager _animationEventManager;
        
        [SerializeField] private Vector3 swordAttackBoxSize = Vector3.one;
        [SerializeField] private Vector3 swordAttackOffset = Vector3.forward;
        [SerializeField] private int swordAttackDamage = 1;
        [SerializeField] private LayerMask attackLayerMask = -1;


        private bool _isAttacking = false;
        private Coroutine _attackCoroutine = null;

        private PlayerWeapon _currentPlayerWeapon = PlayerWeapon.Sword;
        
        //Component to move the player
        private PlayerMovement _movementComponent;

        //Component to push Rigidbodies
        private RigidbodyPush _rigidbodyPush;

        //Component that finds targets
        private PlayerTargetingComponent _targetingComponent;

        private PlayerCameraComponent _cameraComponent;

        private void OnEnable()
        {
            _animationEventManager.swordAttackEndedAnimationEvent += SwordAttackEnded;
            _animationEventManager.swordAttackDealDamageAnimationEvent += SwordAttackDealDamage;
        }

        private void OnDisable()
        {
            _animationEventManager.swordAttackEndedAnimationEvent -= SwordAttackEnded;
            _animationEventManager.swordAttackDealDamageAnimationEvent -= SwordAttackDealDamage;
        }

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
                    _movementComponent.SetTargetDirection(_cameraComponent.GetMainCamera().transform.forward);
                    _movementComponent.Rotate();
                    //Move Character Relative To Camera
                    if (!_isAttacking)
                    {
                        _movementComponent.Move(transform.forward, direction.magnitude);
                    }

                    break;
                
                case PlayerCameraMode.Orbit:
                    //Rotate Character To Input Direction
                    _movementComponent.SetTargetDirection(direction);
                    _movementComponent.Rotate();
                    //Move Character Relative To Camera
                    if (!_isAttacking)
                    {
                        _movementComponent.Move(transform.forward, direction.magnitude);
                    }

                    break;
                
                case PlayerCameraMode.TargetLocked:
                    //Rotate Character Towards Locked Target
                    _movementComponent.SetTargetDirection(_targetingComponent.GetCurrentTarget().TargetTransform.position - transform.position);
                    _movementComponent.Rotate();
                    //Move Character Relative To Target
                    if (!_isAttacking)
                    {
                        _movementComponent.Move(direction, direction.magnitude);
                    }

                    break;
                
                default:
                    Debug.LogError("Unsupported CameraMode case hit during Move().");
                    break;
            }
        }
        
        public void Jump()
        {
            //if not attacking, try to jump
            if (!_isAttacking)
            {
                _movementComponent.Jump();
            }
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
            if (_currentPlayerWeapon != PlayerWeapon.SlingShot)
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

        public void EquipWeapon(PlayerWeapon playerWeaponToEquip)
        {
            _currentPlayerWeapon = playerWeaponToEquip;
            //TODO: swap weapon model/or animate switch weapons
        }
        public void Attack()
        {
            switch (_currentPlayerWeapon)
            {
                case PlayerWeapon.None:
                    //if no weapon is equipped, Equip the Sword, then attack
                    break;
                case PlayerWeapon.Sword:
                    //Attack with the Sword
                    StartSwordAttack();
                    break;
                case PlayerWeapon.SlingShot:
                    //Attack with the SlingShot
                    SlingShotAttack();
                    break;
                default:
                    Debug.LogError("Unsupported Weapon case hit during Attack().");
                    break;
            }
        }

        private void StartSwordAttack()
        {
            if (_isAttacking) { return; }

            _isAttacking = true;
            animator.SetBool("IsAttacking",true);
        }

        public void SwordAttackDealDamage()
        {
            Vector3 attackBoxPosition = transform.position + transform.rotation * swordAttackOffset;
            Collider[] colliders = Physics.OverlapBox(attackBoxPosition, swordAttackBoxSize / 2, transform.rotation,attackLayerMask);
            
            foreach (var col in colliders)
            {
                Debug.Log(col.gameObject.name + " hit by attack");
                IDamageable damageableComponent = col.gameObject.GetComponent<IDamageable>();
                if (damageableComponent != null)
                {
                    damageableComponent.TakeDamage(swordAttackDamage);
                }
            }
        }
        public void SwordAttackEnded()
        {
            _isAttacking = false;
            animator.SetBool("IsAttacking", false);
        }

        private void SlingShotAttack()
        {
            
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(swordAttackOffset, swordAttackBoxSize);
        }
    }
}
