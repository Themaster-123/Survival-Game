// GENERATED AUTOMATICALLY FROM 'Assets/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""ea39f397-1621-4452-9fe9-8edb83e4d5af"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""2ae92ce4-4106-488d-a7ce-1b6c1c468571"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMovement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b962a800-b1b2-4814-885a-c853d0a8d5ce"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0962e210-932c-4c6d-ab78-199649cf3645"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""PassThrough"",
                    ""id"": ""62242559-c20e-41c6-bc6d-42676f909a7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""StopInteract"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9d2b6a42-7c45-4712-b517-f34a23d7d235"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""ToggleInventory"",
                    ""type"": ""Button"",
                    ""id"": ""bc7f2ed5-eb43-47d5-a22f-f9946eb71797"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""UIMousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""64e527f7-6765-46e3-8a4b-d329f4cb9c36"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryInteract "",
                    ""type"": ""Button"",
                    ""id"": ""a11ceefe-4dbc-47c1-93ee-05bd64fb12d1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""ec6dc395-6b34-4134-8215-e0b7ce2d6e40"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SlotChange"",
                    ""type"": ""PassThrough"",
                    ""id"": ""550fd130-fc88-4029-a754-020d1461ba34"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InstantSlotChange"",
                    ""type"": ""PassThrough"",
                    ""id"": ""03a9c8ad-5083-4ad7-85b1-9592000bfe78"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RemoveBuilding"",
                    ""type"": ""PassThrough"",
                    ""id"": ""89203782-9ab0-40cc-802a-92309f7c5853"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b3eb7a7e-5b09-49f4-814a-a1886a377380"",
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
                    ""id"": ""e542145d-e858-4fca-aa0a-5ee1e812289c"",
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
                    ""id"": ""c559b4e6-b795-4bc4-bcc9-bb01c1485985"",
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
                    ""id"": ""3abf52b9-cdfa-43b8-b320-ff2411fa59cb"",
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
                    ""id"": ""a2dac824-05c9-422d-b76f-16239b2352df"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3b63bf0c-7a24-4ff8-bc85-f32004f83170"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68474c9c-da52-42b2-8ec3-9c3b8efa5825"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f4d6d79-870b-46f0-b3e3-afd708bb63e1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd36abf2-9152-42c8-b9c9-c814953c66df"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopInteract"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bbca22a7-9444-4aed-a85b-8a1cd861360d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a2bfd61-dd72-45f0-9b68-bc0fc30b8c30"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UIMousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65892521-5895-4a28-90f5-9287077b7e75"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryInteract "",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f27d7604-c5c1-4ab7-84f4-a0499f88e113"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95069187-3f0b-477a-8a1b-9c33071705d2"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-1,max=1),Normalize(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""SlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51cace0f-5bef-4c37-92cb-5a664d66ad42"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": ""Scale"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e17b8776-bdc7-4d07-ae32-a2410857f254"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=2)"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36c7ae39-a216-4b71-9f12-330bd9ce8a63"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=3)"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8958b547-3fa8-4a13-ac08-5675206ecf0e"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=4)"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ef1e833-776e-4457-8935-12d4eb7c3873"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=5)"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65c43792-a115-4a2d-b0e8-1e290b2b2b10"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=6)"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b9cc670-95c5-48df-b77e-9be8db6a4c46"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=7)"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""050751af-1182-4b96-9401-093dc84776d2"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=8)"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""060e70a3-43ea-41b3-abff-3037aed50f80"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=9)"",
                    ""groups"": """",
                    ""action"": ""InstantSlotChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5225a59-514e-4204-837f-ef9944b660d3"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RemoveBuilding"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Testing"",
            ""id"": ""5fd5db50-da18-480b-aa0c-097e392c4f37"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e4e7a5aa-6e82-45a9-a985-9967506d8fe6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e8bac482-8ebc-4e00-bbc3-38d50d6c5d98"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Right Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""525fb795-7eba-4007-a3d8-f9e5db36aa39"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""G"",
                    ""type"": ""Button"",
                    ""id"": ""b042ec12-994c-4e66-991a-39fe7aaca04e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""215b7424-f894-4ca7-9058-9582d8200786"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8204cd7-cba6-469a-91cb-5fafd70b643d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""505cb326-1978-476b-9e5c-2eac4285f986"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51698996-b8e2-486f-aa30-580954e37031"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""G"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_MouseMovement = m_Player.FindAction("MouseMovement", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_StopInteract = m_Player.FindAction("StopInteract", throwIfNotFound: true);
        m_Player_ToggleInventory = m_Player.FindAction("ToggleInventory", throwIfNotFound: true);
        m_Player_UIMousePosition = m_Player.FindAction("UIMousePosition", throwIfNotFound: true);
        m_Player_SecondaryInteract = m_Player.FindAction("SecondaryInteract ", throwIfNotFound: true);
        m_Player_Drop = m_Player.FindAction("Drop", throwIfNotFound: true);
        m_Player_SlotChange = m_Player.FindAction("SlotChange", throwIfNotFound: true);
        m_Player_InstantSlotChange = m_Player.FindAction("InstantSlotChange", throwIfNotFound: true);
        m_Player_RemoveBuilding = m_Player.FindAction("RemoveBuilding", throwIfNotFound: true);
        // Testing
        m_Testing = asset.FindActionMap("Testing", throwIfNotFound: true);
        m_Testing_MousePosition = m_Testing.FindAction("MousePosition", throwIfNotFound: true);
        m_Testing_LeftClick = m_Testing.FindAction("Left Click", throwIfNotFound: true);
        m_Testing_RightClick = m_Testing.FindAction("Right Click", throwIfNotFound: true);
        m_Testing_G = m_Testing.FindAction("G", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_MouseMovement;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_StopInteract;
    private readonly InputAction m_Player_ToggleInventory;
    private readonly InputAction m_Player_UIMousePosition;
    private readonly InputAction m_Player_SecondaryInteract;
    private readonly InputAction m_Player_Drop;
    private readonly InputAction m_Player_SlotChange;
    private readonly InputAction m_Player_InstantSlotChange;
    private readonly InputAction m_Player_RemoveBuilding;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @MouseMovement => m_Wrapper.m_Player_MouseMovement;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @StopInteract => m_Wrapper.m_Player_StopInteract;
        public InputAction @ToggleInventory => m_Wrapper.m_Player_ToggleInventory;
        public InputAction @UIMousePosition => m_Wrapper.m_Player_UIMousePosition;
        public InputAction @SecondaryInteract => m_Wrapper.m_Player_SecondaryInteract;
        public InputAction @Drop => m_Wrapper.m_Player_Drop;
        public InputAction @SlotChange => m_Wrapper.m_Player_SlotChange;
        public InputAction @InstantSlotChange => m_Wrapper.m_Player_InstantSlotChange;
        public InputAction @RemoveBuilding => m_Wrapper.m_Player_RemoveBuilding;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @MouseMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseMovement;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @StopInteract.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStopInteract;
                @StopInteract.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStopInteract;
                @StopInteract.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStopInteract;
                @ToggleInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleInventory;
                @ToggleInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleInventory;
                @ToggleInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleInventory;
                @UIMousePosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUIMousePosition;
                @UIMousePosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUIMousePosition;
                @UIMousePosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUIMousePosition;
                @SecondaryInteract.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryInteract;
                @SecondaryInteract.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryInteract;
                @SecondaryInteract.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryInteract;
                @Drop.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrop;
                @SlotChange.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlotChange;
                @SlotChange.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlotChange;
                @SlotChange.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlotChange;
                @InstantSlotChange.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInstantSlotChange;
                @InstantSlotChange.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInstantSlotChange;
                @InstantSlotChange.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInstantSlotChange;
                @RemoveBuilding.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRemoveBuilding;
                @RemoveBuilding.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRemoveBuilding;
                @RemoveBuilding.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRemoveBuilding;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @MouseMovement.started += instance.OnMouseMovement;
                @MouseMovement.performed += instance.OnMouseMovement;
                @MouseMovement.canceled += instance.OnMouseMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @StopInteract.started += instance.OnStopInteract;
                @StopInteract.performed += instance.OnStopInteract;
                @StopInteract.canceled += instance.OnStopInteract;
                @ToggleInventory.started += instance.OnToggleInventory;
                @ToggleInventory.performed += instance.OnToggleInventory;
                @ToggleInventory.canceled += instance.OnToggleInventory;
                @UIMousePosition.started += instance.OnUIMousePosition;
                @UIMousePosition.performed += instance.OnUIMousePosition;
                @UIMousePosition.canceled += instance.OnUIMousePosition;
                @SecondaryInteract.started += instance.OnSecondaryInteract;
                @SecondaryInteract.performed += instance.OnSecondaryInteract;
                @SecondaryInteract.canceled += instance.OnSecondaryInteract;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @SlotChange.started += instance.OnSlotChange;
                @SlotChange.performed += instance.OnSlotChange;
                @SlotChange.canceled += instance.OnSlotChange;
                @InstantSlotChange.started += instance.OnInstantSlotChange;
                @InstantSlotChange.performed += instance.OnInstantSlotChange;
                @InstantSlotChange.canceled += instance.OnInstantSlotChange;
                @RemoveBuilding.started += instance.OnRemoveBuilding;
                @RemoveBuilding.performed += instance.OnRemoveBuilding;
                @RemoveBuilding.canceled += instance.OnRemoveBuilding;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Testing
    private readonly InputActionMap m_Testing;
    private ITestingActions m_TestingActionsCallbackInterface;
    private readonly InputAction m_Testing_MousePosition;
    private readonly InputAction m_Testing_LeftClick;
    private readonly InputAction m_Testing_RightClick;
    private readonly InputAction m_Testing_G;
    public struct TestingActions
    {
        private @InputMaster m_Wrapper;
        public TestingActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_Testing_MousePosition;
        public InputAction @LeftClick => m_Wrapper.m_Testing_LeftClick;
        public InputAction @RightClick => m_Wrapper.m_Testing_RightClick;
        public InputAction @G => m_Wrapper.m_Testing_G;
        public InputActionMap Get() { return m_Wrapper.m_Testing; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestingActions set) { return set.Get(); }
        public void SetCallbacks(ITestingActions instance)
        {
            if (m_Wrapper.m_TestingActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_TestingActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_TestingActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_TestingActionsCallbackInterface.OnMousePosition;
                @LeftClick.started -= m_Wrapper.m_TestingActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_TestingActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_TestingActionsCallbackInterface.OnLeftClick;
                @RightClick.started -= m_Wrapper.m_TestingActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_TestingActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_TestingActionsCallbackInterface.OnRightClick;
                @G.started -= m_Wrapper.m_TestingActionsCallbackInterface.OnG;
                @G.performed -= m_Wrapper.m_TestingActionsCallbackInterface.OnG;
                @G.canceled -= m_Wrapper.m_TestingActionsCallbackInterface.OnG;
            }
            m_Wrapper.m_TestingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @G.started += instance.OnG;
                @G.performed += instance.OnG;
                @G.canceled += instance.OnG;
            }
        }
    }
    public TestingActions @Testing => new TestingActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnMouseMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnStopInteract(InputAction.CallbackContext context);
        void OnToggleInventory(InputAction.CallbackContext context);
        void OnUIMousePosition(InputAction.CallbackContext context);
        void OnSecondaryInteract(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnSlotChange(InputAction.CallbackContext context);
        void OnInstantSlotChange(InputAction.CallbackContext context);
        void OnRemoveBuilding(InputAction.CallbackContext context);
    }
    public interface ITestingActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnG(InputAction.CallbackContext context);
    }
}
