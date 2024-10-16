using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace itsSALT.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private CharacterController _characterController;
        [SerializeField]
        private Camera _playerCamera;

        [Header("Base Movement")]
        public float runAcceleration = 0.25f;
        public float runSpeed = 2f;
        public float drag = 0.1f;
        public float sprintSpeed = 2f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 70f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;
        private bool sprinting = false;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }

        private void Update()
        {
            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;
            Vector3 newVelocity = _characterController.velocity + movementDelta;
            newVelocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);

            //Add Drag
            Vector3 currentDrag  = newVelocity.normalized * drag * Time.deltaTime;
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;

            // Clamp Velocity
            newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);

            // Zero Y component of new velocity
            newVelocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);

            float multiplier = 1;

            if(_playerLocomotionInput.SprintInput)
            {
                multiplier = sprintSpeed;
            }

            // Move character (unity says only call this once per frame)
            _characterController.Move(newVelocity * multiplier * Time.deltaTime);



            //Debug.Log("CameraRight: " + cameraRightXZ);
            //Debug.Log("CameraForward: " + cameraForwardXZ);
            //Debug.Log("MovementDirection: " + movementDirection);
            //Debug.Log("MovementDelta: " + movementDelta);
            //Debug.Log("New Velocity: " + newVelocity);
            //Debug.Log(_playerLocomotionInput.MovementInput);
        }

        private void LateUpdate()
        {
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }
    }
}
