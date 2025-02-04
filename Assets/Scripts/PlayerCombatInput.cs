using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace itsSALT.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerCombatInput : MonoBehaviour, PlayerControls.IPlayerCombatMapActions
    {
        public PlayerControls PlayerControls { get; private set; }

        public bool LightAttackInput { get; private set; }

        #region Startup
        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerCombatMap.Enable();
            PlayerControls.PlayerCombatMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerControls.PlayerCombatMap.Disable();
            PlayerControls.PlayerCombatMap.RemoveCallbacks(this);
        }
        #endregion

        private void LateUpdate()
        {
            LightAttackInput = false;
        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                LightAttackInput = true;
            }
        }
    }
}
