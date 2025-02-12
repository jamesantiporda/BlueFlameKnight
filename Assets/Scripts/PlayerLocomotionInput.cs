using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace itsSALT.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        #region Class Variables
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool SprintInput { get; private set; }

        public bool RollInput { get; private set; }

        public bool LockToggledOn { get; private set; }

        private float sprintButtonTimer = 0.0f;
        private bool sprintButtonHeld = false;
        private float sprintDelay = 0.5f;
        #endregion

        #region Startup
        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }
        #endregion

        #region Update/Late Update Logic
        private void Update()
        {
            if (sprintButtonHeld)
            {
                sprintButtonTimer += Time.deltaTime;

                if (sprintButtonTimer > sprintDelay)
                {
                    SprintInput = true;
                }
            }
            else
            {
                SprintInput = false;
            }
        }

        private void LateUpdate()
        {
            RollInput = false;
        }
        #endregion

        #region Input Callbacks
        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnStickLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>() * 2f;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            //SprintInput = context.started || context.performed;

            if(context.started)
            {
                sprintButtonHeld = true;
            }
            else if(context.canceled)
            {
                sprintButtonHeld = false;

                if(sprintButtonTimer < sprintDelay)
                {
                    //Debug.Log("Roll!");
                    RollInput = true;
                }

                sprintButtonTimer = 0.0f;
            }
        }

        public void OnToggleLock(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            LockToggledOn = !LockToggledOn;
        }
        #endregion

        public void ForceDisableLock()
        {
            LockToggledOn = false;
        }
    }
}
