using System;
using Systems.AudioManager;
using Systems.PlayerManager;
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

        public Action<int> playerAmmoChanged;

        [Header("Animation Properties")]
        [SerializeField]private Animator animator;
        [SerializeField] private PlayerAnimationEventManager _animationEventManager;
        [SerializeField] private GameObject aimRotationBone;
        private Quaternion originalAimBoneRotation;
        private float _currentAimBonePitch;
        
        [Header("Sling Shot Properties")]
        [SerializeField] private GameObject slingShotGameObject;
        [SerializeField] private Transform projectileSpawnTransform;
        [SerializeField] private GameObject slingShotProjectilePrefab;
        [SerializeField] private AudioClip slingShotFiredSFX;
        [SerializeField] private float aimRotationSpeed = .5f;
        [SerializeField] private float projectileSpeed = 60f;
        [SerializeField] private int maxAmmo = 10;
        [SerializeField] private int currentAmmo = 10;
#if UNITY_EDITOR
        public bool unlimitedAmmoDebug;
        public bool drawDebug = false;
#endif

        [Header("Sword Properties")]
        [SerializeField] private GameObject swordGameObject;
        [SerializeField] private AudioClip swordSwingSFX;
        [SerializeField] private Vector3 swordAttackBoxSize = Vector3.one;
        [SerializeField] private Vector3 swordAttackOffset = Vector3.forward;
        [SerializeField] private int swordAttackDamage = 1;
        [SerializeField] private LayerMask attackLayerMask = -1;

        private LayerMask shotLayers;
        private bool _isAttacking = false;

        private PlayerWeapon _currentPlayerWeapon = PlayerWeapon.Sword;
        
        //Component to move the player
        private PlayerMovement _movementComponent;

        //Component to push Rigidbodies
        private RigidbodyPush _rigidbodyPush;

        //Component that finds targets
        private PlayerTargetingComponent _targetingComponent;

        private PlayerCameraComponent _cameraComponent;

        private Health _health;

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
            // register this game object as the player
            ServiceLocator.Instance.Get<PlayerManager>().RegisterPlayer(gameObject);
            
            //get health component
            _health = GetComponent<Health>();
            
            //bind event to health reaching zero
            _health.OnHealthIsZero += PlayerDied;
            
            //initialize layermake
            shotLayers = ~LayerMask.GetMask("Player");
            
            //get other components
            _movementComponent = GetComponent<PlayerMovement>();
            _targetingComponent = GetComponent<PlayerTargetingComponent>();
            _cameraComponent = GetComponent<PlayerCameraComponent>();
            _rigidbodyPush = GetComponent<RigidbodyPush>();
            
            //initialize aim rotation
            originalAimBoneRotation = aimRotationBone.transform.localRotation;
            
            //start with sword equipped
            EquipWeapon(PlayerWeapon.Sword);

        }

        private void PlayerDied()
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(GameOverState));
        }

        public void Move(Vector3 direction)
        {
            //don't move while attacking
            if (_isAttacking) return;
            //rotate the input direction input by the camera yaw
            direction = Quaternion.Euler(0, _cameraComponent.GetMainCamera().transform.eulerAngles.y, 0) * direction;

            switch (_cameraComponent.GetCurrentCameraMode())
            {
                case PlayerCameraMode.Aiming:
                    //Move Character Relative To Camera
                    _movementComponent.Move(direction, direction.magnitude);
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
                    _movementComponent.Move(direction, direction.magnitude);
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
        
        public void StartAiming()
        {
            //Don't aim if SlingShot isn't equipped or if character is not on ground
            if (_currentPlayerWeapon != PlayerWeapon.SlingShot || !_movementComponent.IsGrounded())
            {
                return;
            }
            
            //TODO: Set Animation to Aiming
            if (_cameraComponent.GetCurrentCameraMode() == PlayerCameraMode.TargetLocked)
            {
                _targetingComponent.StopTargeting();
            }
            _cameraComponent.SetCurrentCameraMode(PlayerCameraMode.Aiming);
            _currentAimBonePitch = aimRotationBone.transform.localEulerAngles.x;
            animator.SetBool("IsAiming",true);
        }

        public void StopAiming()
        {
            if (_cameraComponent.GetCurrentCameraMode() != PlayerCameraMode.Aiming)
            {
                return;
            }
            
            //TODO: turn off aiming Animation
            _cameraComponent.SetCurrentCameraMode(PlayerCameraMode.Orbit); 
            aimRotationBone.transform.localRotation = originalAimBoneRotation;
            animator.SetBool("IsAiming",false);
        }

        public void Aim(Vector2 direction)
        {
            if (_cameraComponent.GetCurrentCameraMode() != PlayerCameraMode.Aiming) {return;}

            _currentAimBonePitch -= direction.y * aimRotationSpeed;
            _currentAimBonePitch = Mathf.Clamp(_currentAimBonePitch, 45f, 135f);
            aimRotationBone.transform.localRotation = Quaternion.Euler(_currentAimBonePitch,0,0);
            
            _movementComponent.SetTargetDirection(Quaternion.Euler(0,direction.x*aimRotationSpeed,0)*transform.forward);
            _movementComponent.Rotate();
        }

        public void EquipWeapon(PlayerWeapon playerWeaponToEquip)
        {
            //cant change weapons while attacking
            if (_isAttacking) { return;}

            //TODO:animate switch weapons
            switch (playerWeaponToEquip)
            {
                case PlayerWeapon.Sword:
                    swordGameObject.SetActive(true);
                    slingShotGameObject.SetActive(false);
                    break;
                case PlayerWeapon.SlingShot:
                    swordGameObject.SetActive(false);
                    slingShotGameObject.SetActive(true);
                    break;
                default:
                    Debug.LogError("Unsupported Weapon Equipped");
                    break;
            }
            _currentPlayerWeapon = playerWeaponToEquip;
            
            
        }

        public void SwapWeapon()
        {
            if (_currentPlayerWeapon == PlayerWeapon.Sword)
            {
                EquipWeapon(PlayerWeapon.SlingShot);
            }
            else
            {
                EquipWeapon(PlayerWeapon.Sword);
            }
        }
        public void Attack()
        {
            switch (_currentPlayerWeapon)
            {
                case PlayerWeapon.None:
                    //TODO: if no weapon is equipped, Equip the Sword, then attack
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
            if (_isAttacking || !_movementComponent.IsGrounded()) { return; }

            _isAttacking = true;
            animator.SetBool("IsAttacking",true);
            ServiceLocator.Instance.Get<AudioManager>().PlaySFX(swordSwingSFX);
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
#if UNITY_EDITOR
            if (unlimitedAmmoDebug)
            {
                currentAmmo = maxAmmo;
                playerAmmoChanged?.Invoke(currentAmmo);
            }            
#endif
            //don't attack without ammo or not aiming
            if (currentAmmo <= 0 || (_cameraComponent.GetCurrentCameraMode() != PlayerCameraMode.Aiming && _cameraComponent.GetCurrentCameraMode() != PlayerCameraMode.TargetLocked)) { return; }

            // origin direction hitinfo distance
            if (Physics.Raycast(projectileSpawnTransform.position, projectileSpawnTransform.rotation.eulerAngles, out var rhInfo, 5000.0f, shotLayers)) {
                var targetedObject = rhInfo.collider.gameObject;
                var targetedPoint = rhInfo.point;
                Debug.Log(targetedObject.name + " " + targetedPoint);
            }
            //Spawn projectile
            GameObject projectile =  Instantiate(slingShotProjectilePrefab, projectileSpawnTransform.position,projectileSpawnTransform.rotation);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;
            
            //reduce ammo
            currentAmmo--;
            playerAmmoChanged?.Invoke(currentAmmo);
            
            //Play SFX
            ServiceLocator.Instance.Get<AudioManager>().PlaySFX(slingShotFiredSFX);
            Debug.Log("Current Ammo: " + currentAmmo);
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
            ServiceLocator.Instance.Get<AudioManager>().PlaySFX(castPickup.clipToPlayOnPickup);
            
            //handle pickup type
            if (castPickup != null)
            {
                switch (castPickup.category)
                {
                    case Pickup.Category.Health:
                        _health.AddHealth(castPickup.modifier);
                        Debug.Log("Added health");
                        //TODO: Add Health
                        break;
                    case Pickup.Category.Ammo:
                        currentAmmo += castPickup.modifier;
                        currentAmmo = currentAmmo > maxAmmo ? maxAmmo : currentAmmo;
                        playerAmmoChanged?.Invoke(currentAmmo);
                        Debug.Log("Current Ammo: " + currentAmmo);
                        break;
                    default:
                        Debug.LogError("Unsupported Pickup Type");
                        break;
                }
            }
            Destroy(pickup);
        }
        private void OnDrawGizmos()
        {
            if (drawDebug)
            {
                Gizmos.color = Color.red;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(swordAttackOffset, swordAttackBoxSize);
            }
        }

        public int GetMaxAmmo() 
        {
            return maxAmmo;
        }

        public int GetCurrentAmmo() 
        {
            return currentAmmo;
        }
    }
}
