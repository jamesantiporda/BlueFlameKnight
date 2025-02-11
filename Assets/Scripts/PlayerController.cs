using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

        public float RotationMismatch { get; private set; } = 0f;
        public bool IsRotatingToTarget { get; private set; } = false;

        public bool IsAttacking { get; private set; } = false;

        public bool IsMovingWhileAttacking { get; private set; } = false;

        public bool IsMovingRolling { get; private set; } = false;

        public bool IsLockedInAnimation { get; private set; } = false;

        public bool IsMoveable { get; private set; } = false;

        public bool IsKnockedBack { get; private set; } = false;

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

        [Header("Animation")]
        public float playerModelRotationSpeed = 10f;
        public float rotateToTargetTime = 0.25f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 70f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerCombatInput _playerCombatInput;
        private PlayerState _playerState;

        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;
        private bool sprinting = false;
        private Vector2 rollDirection = Vector2.zero;
        private Vector2 _playerTargetDirection = Vector2.zero;
        public Vector3 knockbackDirection = Vector3.zero;

        private float _rotatingToTargetTimer = 0f;
        #endregion

        #region Startup
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerCombatInput = GetComponent<PlayerCombatInput>();
            _playerState = GetComponent<PlayerState>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion

        #region Update Logic
        private void Update()
        {
            UpdateMovementState();
            HandleLateralMovement();

            if(IsMovingWhileAttacking)
            {
                Vector3 attackDir = new Vector3(transform.forward.x, 0.0f, transform.forward.z);

                MovePlayer(attackDir, attackMoveSpeed);
            }

            if(IsKnockedBack)
            {
                Vector3 knockbackDir = knockbackDirection;

                MovePlayer(knockbackDir, knockbackSpeed);
            }
        }

        public void MovePlayer(Vector3 direction, float speed)
        {
            _characterController.Move(speed * direction * Time.deltaTime);
        }

        private void UpdateMovementState()
        {
            if(_playerState.CurrentPlayerMovementState != PlayerMovementState.Rolling && !IsAttacking && !IsLockedInAnimation)
            {
                bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
                bool isMovingLaterally = IsMovingLaterally();
                bool isSprinting = _playerLocomotionInput.SprintInput && isMovingLaterally;
                bool isGrounded = IsGrounded();
                bool isRolling =  _playerLocomotionInput.RollInput && isMovementInput;
                bool isStrafing = _playerLocomotionInput.LockToggledOn && isMovingLaterally;

                if(isRolling)
                {
                    StartCoroutine(Roll());
                }

                PlayerMovementState lateralState = isRolling ? PlayerMovementState.Rolling :
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

            if(newVelocity != Vector3.zero)
            {
                _playerTargetDirection = new Vector2(newVelocity.x, newVelocity.z);
            }

            //Debug.Log("Target Dir: " + _playerTargetDirection);

            if(!IsAttacking && !IsLockedInAnimation)
            {
                if (_playerState.CurrentPlayerMovementState == PlayerMovementState.Rolling)
                {
                    Vector3 rollDirection3D = new Vector3(rollDirection.x, 0.0f, rollDirection.y);
                    rollDirection3D.Normalize();

                    if(IsMovingRolling)
                    {
                        _characterController.Move(rollSpeed * rollDirection3D * Time.deltaTime);
                    }
                }
                else
                {
                    // Move character (unity says only call this once per frame)
                    if(IsMoveable)
                    {
                        _characterController.Move(newVelocity * _playerLocomotionInput.MovementInput.magnitude / 10 * Time.deltaTime);
                    }
                }
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

            if(IsMovingLaterally() || IsAttacking || _rotatingToTargetTimer > 0)
            {
                Vector3 lockDirectionProj = _targetEnemy.transform.position - transform.position;
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

            if(_playerLocomotionInput.LockToggledOn)
            {
                Vector3 lockDirection = _targetEnemy.transform.position - _playerCamera.transform.position;

                lockDirection.Normalize();

                Quaternion cameraFinalRotation = Quaternion.LookRotation(lockDirection);

                _playerCamera.transform.rotation = cameraFinalRotation; //Quaternion.Lerp(_playerCamera.transform.rotation, cameraFinalRotation, playerModelRotationSpeed * Time.deltaTime);
                
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
            return _characterController.isGrounded;
        }

        public void StartAttacking()
        {
            IsAttacking = true;
        }

        public void StopAttacking()
        {
            IsAttacking = false;
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

            _playerState.SetPlayerMovementState(PlayerMovementState.Idling);
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
            _playerState.SetPlayerMovementState(PlayerMovementState.Damaged);
        }
        #endregion

        #region Coroutines

        private IEnumerator Roll()
        {
            Debug.Log("ROLLING");

            rollDirection = new Vector2(_characterController.velocity.x, _characterController.velocity.z);
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

        #endregion
    }
}
