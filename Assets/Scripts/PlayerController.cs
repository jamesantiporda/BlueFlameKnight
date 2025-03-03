using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

namespace itsSALT.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {

        #region Class Variables
        [Header("Components")]
        [SerializeField]
        private CharacterController _characterController;
        [SerializeField]
        private Camera _playerCamera;
        [SerializeField]
        private GameObject _targetEnemy;
        [SerializeField]
        private Transform groundCheck;
        [SerializeField]
        private LayerMask groundMask;
        private Health _health;
        private Stamina _stamina;
        [SerializeField]
        private Animator _animator;

        public float RotationMismatch { get; private set; } = 0f;
        public bool IsRotatingToTarget { get; private set; } = false;

        public bool IsAttacking { get; private set; } = false;

        public bool IsMovingWhileAttacking { get; private set; } = false;

        public bool IsMovingRolling { get; private set; } = false;

        public bool IsLockedInAnimation { get; private set; } = false;

        public bool IsMoveable { get; private set; } = false;

        public bool IsKnockedBack { get; private set; } = false;

        public bool IsDead { get; private set; } = false;

        [Header("Base Movement")]
        public float runAcceleration = 0.25f;
        public float runSpeed = 2f;
        public float drag = 0.1f;
        public float sprintAcceleratiion = 0.5f;
        public float sprintSpeed = 3f;
        public float movingThreshold = 0.01f;
        public float rollSpeed = 5f;
        public float rollCooldown = 0.4f;
        public float attackMoveSpeed = 1f;
        public float knockbackSpeed = 1f;
        public float gravity = 9.8f;
        public float groundDistance = 0.01f;

        [Header("Animation")]
        public float playerModelRotationSpeed = 10f;
        public float rotateToTargetTime = 0.25f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 70f;
        public float lockSpeed = 5f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerCombatInput _playerCombatInput;
        private PlayerState _playerState;

        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;
        private bool sprinting = false;
        private Vector3 rollDirection = Vector3.zero;
        private Vector2 _playerTargetDirection = Vector2.zero;
        public Vector3 knockbackDirection = Vector3.zero;

        private float _rotatingToTargetTimer = 0f;

        public bool IsDrinking { get; private set; } = false;
        #endregion

        #region Startup
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerCombatInput = GetComponent<PlayerCombatInput>();
            _playerState = GetComponent<PlayerState>();
            _health = GetComponent<Health>();
            _stamina = GetComponent<Stamina>();
        }
        #endregion

        #region Update Logic
        private void Update()
        {
            UpdateMovementState();
            HandleLateralMovement();
            HandleVerticalMovement();

            if (_health.currentHealth <= 0 && _playerState.CurrentPlayerMovementState != PlayerMovementState.Dead)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Dead);
                Die();
                return;
            }

            if (IsMovingWhileAttacking)
            {
                Vector3 attackDir = new Vector3(transform.forward.x, 0.0f, transform.forward.z);

                MovePlayer(attackDir, attackMoveSpeed);
            }

            if(IsKnockedBack)
            {
                Vector3 knockbackDir = knockbackDirection;

                MovePlayer(knockbackDir, knockbackSpeed);
            }

            //Debug.Log("attacking: " + IsAttacking);
            //Debug.Log("moveable: " + IsMoveable);
        }

        public void MovePlayer(Vector3 direction, float speed)
        {
            _characterController.Move(speed * direction * Time.deltaTime);
        }

        private void UpdateMovementState()
        {
            if(_playerState.CurrentPlayerMovementState != PlayerMovementState.Rolling && !IsLockedInAnimation && _playerState.CurrentPlayerMovementState != PlayerMovementState.Dead)
            {
                bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
                bool isMovingLaterally = IsMovingLaterally() && !IsAttacking;
                bool isSprinting = _playerLocomotionInput.SprintInput && isMovingLaterally && _stamina.currentStamina > 0 && _playerState.CurrentPlayerMovementState != PlayerMovementState.Drinking && !IsAttacking;
                bool isGrounded = IsGrounded();
                bool isRolling =  _playerLocomotionInput.RollInput && isMovementInput && _stamina.currentStamina > 0 && !IsDrinking;
                bool isStrafing = _playerLocomotionInput.LockToggledOn && isMovingLaterally && !IsAttacking;
                bool isDrinking = _playerState.CurrentPlayerMovementState == PlayerMovementState.Drinking && !IsAttacking;

                if(isRolling)
                {
                    StartCoroutine(Roll());
                }

                PlayerMovementState lateralState = isDrinking ? PlayerMovementState.Drinking :
                                                    isRolling ? PlayerMovementState.Rolling :
                                                    isSprinting ? PlayerMovementState.Sprinting :
                                                    isStrafing ? PlayerMovementState.Strafing :
                                                    isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;
                _playerState.SetPlayerMovementState(lateralState);
            }
        }

        private void HandleLateralMovement()
        {
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isStrafing = _playerState.CurrentPlayerMovementState == PlayerMovementState.Strafing;

            if(IsDead)
            {
                _characterController.Move(Vector3.zero);
            }
            else
            {
                // Sprint Stamina calculation
                if (isSprinting)
                {
                    _stamina.DecreaseStamina(1);
                }


                // State Dependent
                float lateralAcceleration = isSprinting ? sprintAcceleratiion : runAcceleration;
                float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;

                Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
                Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
                Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

                Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
                Vector3 newVelocity = _characterController.velocity + movementDelta;
                newVelocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);

                //Add Drag
                Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
                newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
                newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);

                // Zero Y component of new velocity
                newVelocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);

                if (newVelocity != Vector3.zero)
                {
                    _playerTargetDirection = new Vector2(newVelocity.x, newVelocity.z);
                }

                // Slow down if drinking flask
                if (IsDrinking)
                {
                    newVelocity = newVelocity * 0.5f;
                }

                //Debug.Log("Target Dir: " + _playerTargetDirection);

                if (!IsLockedInAnimation)
                {
                    if (_playerState.CurrentPlayerMovementState == PlayerMovementState.Rolling)
                    {
                        Vector3 rollDirection3D = new Vector3(rollDirection.x, 0.0f, rollDirection.z);
                        rollDirection3D.Normalize();

                        if (IsMovingRolling)
                        {
                            _characterController.Move(rollSpeed * rollDirection3D * Time.deltaTime);
                        }
                    }
                    else
                    {
                        // Move character (unity says only call this once per frame)
                        if (IsMoveable && !IsAttacking)
                        {
                            _characterController.Move(newVelocity * _playerLocomotionInput.MovementInput.magnitude / 10 * Time.deltaTime);
                        }
                    }
                }
            }
        }

        private void HandleVerticalMovement()
        {
            if(!IsGrounded())
            {
                transform.position -= Vector3.up * gravity * Time.deltaTime;
                //Debug.Log("NOT GROUNDED");
            }
        }
        #endregion

        #region Late Update
        private void LateUpdate()
        {
            UpdateCameraRotation();
        }

        private void UpdateCameraRotation()
        {
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;

            float rotationTolerance = 90f;

            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            IsRotatingToTarget = _rotatingToTargetTimer > 0;

            Vector3 targetDirection = new Vector3(_playerTargetDirection.x, 0.0f, _playerTargetDirection.y);
            targetDirection.Normalize();

            if(IsMovingLaterally() || _rotatingToTargetTimer > 0)
            {
                Vector3 lockDirectionProj = Vector3.zero;

                if(_targetEnemy != null)
                {
                    lockDirectionProj = _targetEnemy.transform.position - transform.position;
                }

                lockDirectionProj = new Vector3(lockDirectionProj.x, 0.0f, lockDirectionProj.z);
                lockDirectionProj.Normalize();

                Quaternion targetRotationX = Quaternion.LookRotation(lockDirectionProj);

                Quaternion finalRotation = Quaternion.LookRotation(targetDirection);

                if(_playerState.CurrentPlayerMovementState == PlayerMovementState.Strafing || (_playerState.CurrentPlayerMovementState == PlayerMovementState.Idling && _playerLocomotionInput.LockToggledOn))
                {
                    finalRotation = targetRotationX;
                }

                if (!IsLockedInAnimation)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, playerModelRotationSpeed * Time.deltaTime);
                }

                if(Mathf.Abs(RotationMismatch) > rotationTolerance)
                {
                    _rotatingToTargetTimer = rotateToTargetTime;
                }
                _rotatingToTargetTimer -= Time.deltaTime;

            }

            if(_playerLocomotionInput.LockToggledOn && !IsDead)
            {
                Vector3 lockDirection = _targetEnemy.transform.position - _playerCamera.transform.position;

                lockDirection.Normalize();

                Quaternion cameraFinalRotation = Quaternion.LookRotation(lockDirection);

                _playerCamera.transform.rotation = cameraFinalRotation; //Quaternion.Lerp(_playerCamera.transform.rotation, cameraFinalRotation, playerModelRotationSpeed * Time.deltaTime);

                Vector3 euler = _playerCamera.transform.rotation.eulerAngles;

                _cameraRotation = new Vector2(euler.y, euler.x);

                //_playerCamera.transform.rotation = Quaternion.Slerp(_playerCamera.transform.rotation, cameraFinalRotation, Time.deltaTime * lockSpeed);
            }
            else
            {
                _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
            }

            Vector3 camForwardProjectedXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 crossProduct = Vector3.Cross(transform.forward, camForwardProjectedXZ);
            float sign = Mathf.Sign(Vector3.Dot(crossProduct, transform.up));

            RotationMismatch = sign * Vector3.Angle(transform.forward, camForwardProjectedXZ);
        }
        #endregion

        #region State Checks
        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.y);

            return lateralVelocity.magnitude > movingThreshold;
        }

        private bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        public void StartAttacking()
        {
            _playerLocomotionInput.ForceDisableSprint();
            IsAttacking = true;
            _playerState.SetPlayerMovementState(PlayerMovementState.Attacking);
        }

        public void StopAttacking()
        {
            Debug.Log("CALLED");

            IsAttacking = false;

            if (_playerState.CurrentPlayerMovementState != PlayerMovementState.Rolling && _playerState.CurrentPlayerMovementState != PlayerMovementState.Damaged)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Idling);
            }
        }

        public void StartDrinking()
        {
            //IsLockedInAnimation = true;
            IsDrinking = true;
            _playerState.SetPlayerMovementState(PlayerMovementState.Drinking);
        }

        public void StopDrinking()
        {
            IsDrinking = false;
            if (_playerState.CurrentPlayerMovementState != PlayerMovementState.Rolling && _playerState.CurrentPlayerMovementState != PlayerMovementState.Damaged)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Idling);
            }
        }

        public void StartMovingWhileAttacking()
        {
            IsMovingWhileAttacking = true;
        }

        public void StopMovingWhileAttacking()
        {
            IsMovingWhileAttacking = false;
        }

        public void StartKnockback()
        {
            IsKnockedBack = true;
        }

        public void StopKnockback()
        {
            IsKnockedBack = false;
        }

        public void StartMovingWhileRolling()
        {
            IsMovingRolling = true;
        }

        public void StopMovingWhileRolling()
        {
            IsMovingRolling = false;
        }

        public void LockInAnimation()
        {
            IsLockedInAnimation = true;
        }

        public void UnlockOutOfAnimation()
        {
            IsLockedInAnimation = false;

            if (_playerState.CurrentPlayerMovementState != PlayerMovementState.Rolling && _playerState.CurrentPlayerMovementState != PlayerMovementState.Damaged)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Idling);
            }
        }
        
        public void MakeMoveable()
        {
            IsMoveable = true;
        }

        public void MakeUnmoveable()
        {
            IsMoveable = false;
        }

        public void Damage()
        {
            IsLockedInAnimation = true;
            IsMovingRolling = false;
            IsKnockedBack = false;
            IsMovingWhileAttacking = false;
            IsAttacking = false;

            _playerState.SetPlayerMovementState(PlayerMovementState.Damaged);
        }

        public void Die()
        {
            IsDead = true;
            IsMoveable = false;
            IsLockedInAnimation = true;
            IsMovingRolling = false;
            IsKnockedBack = false;
            IsMovingWhileAttacking = false;
            IsAttacking = false;
            _playerLocomotionInput.ForceDisableLock();
            MovePlayer(Vector3.zero, 0);
            _animator.SetBool("isDead", true);
            _playerState.SetPlayerMovementState(PlayerMovementState.Dead);
        }
        #endregion

        #region Coroutines

        private IEnumerator Roll()
        {
            Debug.Log("ROLLING");

            _stamina.DecreaseStamina(200);

            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            rollDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;
            rollDirection = new Vector3(rollDirection.x, 0.0f, rollDirection.z);
            rollDirection = rollDirection.normalized;

            yield return new WaitForSeconds(rollCooldown);

            Debug.Log("STOP ROLLING");

            _playerState.SetPlayerMovementState(PlayerMovementState.Idling);
        }
        #endregion

        #region Helpers

        public void WarpToPosition(Vector3 pos)
        {
            _characterController.enabled = false;
            transform.position = pos;
            _characterController.enabled = true;
        }

        public void SnapTowardsTarget(Vector3 pos)
        {
            Vector3 lockDirectionProj = pos - transform.position;
            lockDirectionProj = new Vector3(lockDirectionProj.x, 0.0f, lockDirectionProj.z);
            lockDirectionProj.Normalize();

            transform.rotation = Quaternion.LookRotation(lockDirectionProj, Vector3.up);
        }

        public void ForceLockOnDisable()
        {
            _playerLocomotionInput.ForceDisableLock();
        }

        public void TakeDamage(int damage)
        {
            IsDrinking = false;
            _health.TakeDamage(damage);
        }

        public void DecreaseStamina(int val)
        {
            _stamina.DecreaseStamina(val);
        }

        public int ReturnStamina()
        {
            return _stamina.currentStamina;
        }

        public bool ReturnIsDrinking()
        {
            bool isDrinking = _playerState.CurrentPlayerMovementState == PlayerMovementState.Drinking;

            return isDrinking;
        }

        public bool IsMoving()
        {
            return _characterController.velocity != Vector3.zero;
        }
        #endregion
    }
}
