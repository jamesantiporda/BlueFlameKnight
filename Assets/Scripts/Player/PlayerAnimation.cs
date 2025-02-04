using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace itsSALT.FinalCharacterController
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float locomotionBlendSpeed = 0.02f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerCombatInput _playerCombatInput;
        private PlayerState _playerState;

        private static int inputXHash = Animator.StringToHash("inputX");
        private static int inputYHash = Animator.StringToHash("inputY");
        private static int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
        private static int isRollingHash = Animator.StringToHash("isRolling");

        private Vector3 _currentBlendInput = Vector3.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerCombatInput = GetComponent<PlayerCombatInput>();
            _playerState = GetComponent<PlayerState>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            bool isRunning = _playerState.CurrentPlayerMovementState == PlayerMovementState.Running;
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isRolling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Rolling;

            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * 1.5f :
                                  isRunning ? _playerLocomotionInput.MovementInput * 1f : _playerLocomotionInput.MovementInput * 0.5f;
            inputTarget = new Vector2(inputTarget.x/10, inputTarget.y/10);

            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);

            //Debug.Log("input: " + inputTarget);

            if(_playerCombatInput.LightAttackInput)
            {
                _animator.SetTrigger("LightAttack");
            }

            _animator.SetBool(isRollingHash, isRolling);
            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
            _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
        }
    }
}
