//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace itsSALT.FinalCharacterController
{
    public partial class @PlayerControls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerLocomotionMap"",
            ""id"": ""1ecc4ce2-44dc-47e8-a54a-fbcf1ee7f88d"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""2775f2fb-32fc-4406-aed6-3236c5ae2a1f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""ScaleVector2(x=10,y=10)"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""34218da4-fac5-4e09-b8b1-81368b74049e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""StickLook"",
                    ""type"": ""Value"",
                    ""id"": ""cec950d4-dc48-4863-a18b-caf78cc5a347"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""83f58121-e2a2-42a7-a608-c3b702dc59e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleLock"",
                    ""type"": ""Button"",
                    ""id"": ""9ea524c7-ea2d-4294-a620-47dfde7d2546"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Settings"",
                    ""type"": ""Button"",
                    ""id"": ""e5636195-41ee-448c-83d1-fe77ad5c9340"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchTarget"",
                    ""type"": ""Value"",
                    ""id"": ""30b89724-02a5-4991-99cb-3654fa6e4f81"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""f77ee432-df61-499d-ae2c-f02299da3bb7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""79166dcc-2b49-4cf5-818f-44a749665723"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6926ec35-e4a9-4bb1-a1c7-cd08990625c4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a2de393a-a16a-4562-9e3c-d17a1bca916f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f57e0f17-f309-4f0c-a96a-35af16ef8a14"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""2e597132-2a3a-435b-acae-e4f24434b268"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""75e70fc9-2746-4be9-84ce-a04f5136b01e"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6703138a-1628-4ac9-9dae-53caa4a0bb78"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""10ce8d3a-de45-4e97-a334-21c18622ebaf"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""75995777-e7e9-4339-823e-486c5bbf642b"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c5662334-dffb-4e1a-943b-1b80b8d2a5c2"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6dca0b72-2611-49eb-9531-afd1cb5be809"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=30,y=30)"",
                    ""groups"": """",
                    ""action"": ""StickLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d0dc8f8-b563-4ce3-bc96-e43adb7ce961"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3c1f1c3-3a3d-4de7-9347-f63e58499d54"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f214655-bc8a-454f-94d0-869707e8ef00"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""379b1c00-b695-4ec5-b48a-ac5232459146"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89c45890-2052-4265-8f12-778d422e17a2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Settings"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4db40cec-0db6-4faa-a311-48879fde96e7"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Settings"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""b8e9ca08-aeea-4431-8600-fb6c325b12ab"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchTarget"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ee854edf-4f06-4e17-8b30-46be216028fa"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9c38ca4e-7315-4774-9854-1fbf985c4284"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a89e3371-2554-4810-8572-9947e4086ad6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5927680b-d5a5-4d27-886f-91d673cc425b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""PlayerCombatMap"",
            ""id"": ""f5f7a4c2-df40-4be9-acb4-71df2f518871"",
            ""actions"": [
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""dbf695aa-02e5-4ff6-9c12-f65e1c1ffdc1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Flask"",
                    ""type"": ""Button"",
                    ""id"": ""d69aa30c-b7fb-4897-b624-9e650aa123a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4023a04d-ca27-4cd8-bd61-a632d0f077b4"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b949b0b-6840-4f64-b0b4-6ac1bcf0adfd"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""288e3f2e-7aae-44b3-affe-895dbeed5bd8"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flask"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af16ff97-49f6-413a-9f87-8df1d38c0b4a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flask"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PlayerLocomotionMap
            m_PlayerLocomotionMap = asset.FindActionMap("PlayerLocomotionMap", throwIfNotFound: true);
            m_PlayerLocomotionMap_Movement = m_PlayerLocomotionMap.FindAction("Movement", throwIfNotFound: true);
            m_PlayerLocomotionMap_Look = m_PlayerLocomotionMap.FindAction("Look", throwIfNotFound: true);
            m_PlayerLocomotionMap_StickLook = m_PlayerLocomotionMap.FindAction("StickLook", throwIfNotFound: true);
            m_PlayerLocomotionMap_Sprint = m_PlayerLocomotionMap.FindAction("Sprint", throwIfNotFound: true);
            m_PlayerLocomotionMap_ToggleLock = m_PlayerLocomotionMap.FindAction("ToggleLock", throwIfNotFound: true);
            m_PlayerLocomotionMap_Settings = m_PlayerLocomotionMap.FindAction("Settings", throwIfNotFound: true);
            m_PlayerLocomotionMap_SwitchTarget = m_PlayerLocomotionMap.FindAction("SwitchTarget", throwIfNotFound: true);
            // PlayerCombatMap
            m_PlayerCombatMap = asset.FindActionMap("PlayerCombatMap", throwIfNotFound: true);
            m_PlayerCombatMap_LightAttack = m_PlayerCombatMap.FindAction("LightAttack", throwIfNotFound: true);
            m_PlayerCombatMap_Flask = m_PlayerCombatMap.FindAction("Flask", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // PlayerLocomotionMap
        private readonly InputActionMap m_PlayerLocomotionMap;
        private List<IPlayerLocomotionMapActions> m_PlayerLocomotionMapActionsCallbackInterfaces = new List<IPlayerLocomotionMapActions>();
        private readonly InputAction m_PlayerLocomotionMap_Movement;
        private readonly InputAction m_PlayerLocomotionMap_Look;
        private readonly InputAction m_PlayerLocomotionMap_StickLook;
        private readonly InputAction m_PlayerLocomotionMap_Sprint;
        private readonly InputAction m_PlayerLocomotionMap_ToggleLock;
        private readonly InputAction m_PlayerLocomotionMap_Settings;
        private readonly InputAction m_PlayerLocomotionMap_SwitchTarget;
        public struct PlayerLocomotionMapActions
        {
            private @PlayerControls m_Wrapper;
            public PlayerLocomotionMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_PlayerLocomotionMap_Movement;
            public InputAction @Look => m_Wrapper.m_PlayerLocomotionMap_Look;
            public InputAction @StickLook => m_Wrapper.m_PlayerLocomotionMap_StickLook;
            public InputAction @Sprint => m_Wrapper.m_PlayerLocomotionMap_Sprint;
            public InputAction @ToggleLock => m_Wrapper.m_PlayerLocomotionMap_ToggleLock;
            public InputAction @Settings => m_Wrapper.m_PlayerLocomotionMap_Settings;
            public InputAction @SwitchTarget => m_Wrapper.m_PlayerLocomotionMap_SwitchTarget;
            public InputActionMap Get() { return m_Wrapper.m_PlayerLocomotionMap; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerLocomotionMapActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerLocomotionMapActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterfaces.Add(instance);
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @StickLook.started += instance.OnStickLook;
                @StickLook.performed += instance.OnStickLook;
                @StickLook.canceled += instance.OnStickLook;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @ToggleLock.started += instance.OnToggleLock;
                @ToggleLock.performed += instance.OnToggleLock;
                @ToggleLock.canceled += instance.OnToggleLock;
                @Settings.started += instance.OnSettings;
                @Settings.performed += instance.OnSettings;
                @Settings.canceled += instance.OnSettings;
                @SwitchTarget.started += instance.OnSwitchTarget;
                @SwitchTarget.performed += instance.OnSwitchTarget;
                @SwitchTarget.canceled += instance.OnSwitchTarget;
            }

            private void UnregisterCallbacks(IPlayerLocomotionMapActions instance)
            {
                @Movement.started -= instance.OnMovement;
                @Movement.performed -= instance.OnMovement;
                @Movement.canceled -= instance.OnMovement;
                @Look.started -= instance.OnLook;
                @Look.performed -= instance.OnLook;
                @Look.canceled -= instance.OnLook;
                @StickLook.started -= instance.OnStickLook;
                @StickLook.performed -= instance.OnStickLook;
                @StickLook.canceled -= instance.OnStickLook;
                @Sprint.started -= instance.OnSprint;
                @Sprint.performed -= instance.OnSprint;
                @Sprint.canceled -= instance.OnSprint;
                @ToggleLock.started -= instance.OnToggleLock;
                @ToggleLock.performed -= instance.OnToggleLock;
                @ToggleLock.canceled -= instance.OnToggleLock;
                @Settings.started -= instance.OnSettings;
                @Settings.performed -= instance.OnSettings;
                @Settings.canceled -= instance.OnSettings;
                @SwitchTarget.started -= instance.OnSwitchTarget;
                @SwitchTarget.performed -= instance.OnSwitchTarget;
                @SwitchTarget.canceled -= instance.OnSwitchTarget;
            }

            public void RemoveCallbacks(IPlayerLocomotionMapActions instance)
            {
                if (m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerLocomotionMapActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerLocomotionMapActions @PlayerLocomotionMap => new PlayerLocomotionMapActions(this);

        // PlayerCombatMap
        private readonly InputActionMap m_PlayerCombatMap;
        private List<IPlayerCombatMapActions> m_PlayerCombatMapActionsCallbackInterfaces = new List<IPlayerCombatMapActions>();
        private readonly InputAction m_PlayerCombatMap_LightAttack;
        private readonly InputAction m_PlayerCombatMap_Flask;
        public struct PlayerCombatMapActions
        {
            private @PlayerControls m_Wrapper;
            public PlayerCombatMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @LightAttack => m_Wrapper.m_PlayerCombatMap_LightAttack;
            public InputAction @Flask => m_Wrapper.m_PlayerCombatMap_Flask;
            public InputActionMap Get() { return m_Wrapper.m_PlayerCombatMap; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerCombatMapActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerCombatMapActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerCombatMapActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerCombatMapActionsCallbackInterfaces.Add(instance);
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
                @Flask.started += instance.OnFlask;
                @Flask.performed += instance.OnFlask;
                @Flask.canceled += instance.OnFlask;
            }

            private void UnregisterCallbacks(IPlayerCombatMapActions instance)
            {
                @LightAttack.started -= instance.OnLightAttack;
                @LightAttack.performed -= instance.OnLightAttack;
                @LightAttack.canceled -= instance.OnLightAttack;
                @Flask.started -= instance.OnFlask;
                @Flask.performed -= instance.OnFlask;
                @Flask.canceled -= instance.OnFlask;
            }

            public void RemoveCallbacks(IPlayerCombatMapActions instance)
            {
                if (m_Wrapper.m_PlayerCombatMapActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerCombatMapActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerCombatMapActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerCombatMapActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerCombatMapActions @PlayerCombatMap => new PlayerCombatMapActions(this);
        public interface IPlayerLocomotionMapActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnStickLook(InputAction.CallbackContext context);
            void OnSprint(InputAction.CallbackContext context);
            void OnToggleLock(InputAction.CallbackContext context);
            void OnSettings(InputAction.CallbackContext context);
            void OnSwitchTarget(InputAction.CallbackContext context);
        }
        public interface IPlayerCombatMapActions
        {
            void OnLightAttack(InputAction.CallbackContext context);
            void OnFlask(InputAction.CallbackContext context);
        }
    }
}
