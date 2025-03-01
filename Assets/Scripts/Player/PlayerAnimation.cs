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
        private PlayerController _playerController;
        private Health _health;
        private bool isDead = false;
        
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
            _playerController = GetComponent<PlayerController>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            if(_playerState.CurrentPlayerMovementState == PlayerMovementState.Dead && !isDead)
            {
                _animator.SetTrigger("Die");
                return;
            }

            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            bool isRunning = _playerState.CurrentPlayerMovementState == PlayerMovementState.Running;
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isRolling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Rolling;

            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * 1.5f :
                                  isRunning ? _playerLocomotionInput.MovementInput * 1f :
                                  _playerState.CurrentPlayerMovementState == PlayerMovementState.Drinking ? _playerLocomotionInput.MovementInput * 0.35f :
                                  _playerLocomotionInput.MovementInput * 0.5f;
            inputTarget = new Vector2(inputTarget.x/10, inputTarget.y/10);

            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);

            //Debug.Log("input: " + inputTarget);

            if(_playerCombatInput.LightAttackInput && !_playerController.IsLockedInAnimation && _playerController.ReturnStamina() > 0 && _playerState.CurrentPlayerMovementState != PlayerMovementState.Drinking)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Attacking);
                //_playerController.DecreaseStamina(250);
                _animator.SetTrigger("LightAttack");
            }

            if(_playerCombatInput.FlaskInput && !_playerController.IsLockedInAnimation && _playerState.CurrentPlayerMovementState != PlayerMovementState.Attacking)
            {
                _playerLocomotionInput.ForceDisableSprint();
                _playerState.SetPlayerMovementState(PlayerMovementState.Drinking);
                _animator.SetTrigger("Drink");
            }

            _animator.SetBool(isRollingHash, isRolling);
            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
            _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
        }

        public void Die()
        {
            _animator.SetTrigger("Die");
            isDead = true;
        }
    }
}
