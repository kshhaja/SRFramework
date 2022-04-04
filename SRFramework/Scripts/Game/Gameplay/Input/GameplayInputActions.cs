// GENERATED AUTOMATICALLY FROM 'Assets/SRFramework/SRFramework/Scripts/Game/Gameplay/Input/GameplayInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SlimeRPG.Gameplay.Input
{
    public class @GameplayInputActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameplayInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameplayInputActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""cb4067f3-a685-4c86-b9ea-46a6eabfada7"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""548e32fd-77d1-40e5-8197-32ca56b41bc0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryAbility"",
                    ""type"": ""Button"",
                    ""id"": ""10fecf58-9483-40e0-ae20-15c8d01a4288"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SecondaryAbility"",
                    ""type"": ""Button"",
                    ""id"": ""b5837bf4-e4ce-4e79-81bb-781cbe500c17"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""TertiaryAbility"",
                    ""type"": ""Button"",
                    ""id"": ""7ee895b4-443a-44aa-b503-e286a2fb8b65"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""8ebbde1f-3044-41bc-bdac-430e0eae1a68"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TogglePause"",
                    ""type"": ""Button"",
                    ""id"": ""a70a208e-b491-4921-b460-a0144030ef2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""99258992-afbc-4513-a4ee-24146566e341"",
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
                    ""id"": ""d57e0987-ea9f-4b18-9042-239931d4c060"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""54b99838-0c45-421e-af38-b1f25b3f6927"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""75168890-922f-4122-9968-1ecac0f33c28"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""77680fb0-0b9d-4a74-97de-9e3149ad6526"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""df04a4e1-fc36-4ebd-b050-536736220da7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b12426a0-38ab-4d90-95e0-6840fcd30db6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""PrimaryAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e6d6425-a990-4434-b58d-57464db363d4"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PrimaryAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43d1488b-4ff2-4456-b1bf-a368a70fb680"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d23f9eb5-e32b-417b-8edf-d10cd6bdc1d3"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e30f0b75-70a4-401a-b038-58b2714d90a5"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""SecondaryAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""927ca6ef-8a4f-4c56-8325-c1d9f0c638ff"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SecondaryAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42716e15-291c-4c71-8ded-0d03279959df"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bc14fbd-1107-4761-aa6f-b3367d7705e6"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1e6a5bc-04fd-4a78-a408-f6b97fde0d1e"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""TertiaryAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc9fc72a-531d-458c-a15c-4950e770ed70"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TertiaryAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu Controls"",
            ""id"": ""0914fb5b-51f6-4b26-9ed7-a3e72d065118"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""538ffe95-ba92-4acb-84f7-314f6ac8e0a5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""96c8be88-a7bb-4861-b5e9-956b4208d043"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d54e5ff5-4f35-4d2f-a745-95d14aef8c43"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5f571f6a-e9e7-4120-ae3c-79f846bdd202"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""146e681c-77dd-4ff0-9ad5-f4351fea14cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TogglePause"",
                    ""type"": ""Button"",
                    ""id"": ""e773b1f9-ce5b-4fa2-9c1f-d194202c43b7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Right Click"",
                    ""type"": ""Button"",
                    ""id"": ""36a79137-f75c-469a-8d92-7855f0ad3e7c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ToggleInventory"",
                    ""type"": ""Button"",
                    ""id"": ""63a04d90-5d0b-4d33-8d75-a98711bf57e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad Right Stick"",
                    ""id"": ""c1491510-9d0f-47b0-868e-99575e46d097"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""57fb7217-68c1-483e-a15b-0fd1e5ab3fc3"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0c09243f-be8c-44a1-87c4-a0d3ca3a27a5"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a16b4641-1591-4d94-9fd4-e1eafd539931"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""341f36e1-889b-4d62-834f-622378da658d"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Left Stick"",
                    ""id"": ""c2c92ef2-a9d0-4393-86c7-4180acc16b6d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bed70561-f1cc-4c56-9715-66475aa6437f"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0005d032-151a-4ee0-8127-110d55e5ed9d"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""84fcadd5-5853-4142-b3f9-58a5ab2ad788"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""233f731d-8d73-4761-8879-66c0e0da124d"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d687f18d-7559-488c-8542-e3da3a3dd1f7"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""c2a77ff0-1ce1-4c49-a4dd-94601087a2a2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a67b96dc-9151-496b-9be2-b4d65a82f52a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4f8ac3a8-5653-4cf1-9687-259b7e6bfca4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a3da140b-c504-4aea-9824-ffd10d44e52a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d2cdc452-d127-4c2c-b57c-1f78e29cb425"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""c50ac654-ca86-486c-b427-057a0aacbb3b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""80a43030-09a8-4324-b825-39a685b9a975"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0284caff-9cfb-477f-901c-c6be4082785f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""359d5348-82b3-4e60-9536-8c817495d31a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b77717c3-b7e9-450f-8bc2-3aa284fac5cd"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b5076a57-fe62-4632-8d6c-da0844960a14"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7658a78-a141-4f0c-beb5-0a6a3e393c7b"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touchscreen"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfac3b8e-d348-4a7a-b60b-14745c641340"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f509a7b5-e79a-485e-ba2f-da5431d6fe4c"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fee8919-99e7-4770-abd4-da1b7d4e4cc4"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14559c94-e8a7-426b-8687-fa5f1420a0c1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b35f103a-716c-4078-ad8d-66c5fb7fbb45"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touchscreen"",
                    ""action"": ""Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40b14102-9017-4522-862f-97d92a0da5f6"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c88534db-dffe-4245-85c8-f41ba53024a1"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""602b1c98-04d1-4905-a008-fbb3d6c91434"",
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
                    ""id"": ""654b0fad-6698-4f15-90f8-b7cbe0a06932"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard And Mouse"",
            ""bindingGroup"": ""Keyboard And Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touchscreen"",
            ""bindingGroup"": ""Touchscreen"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Gameplay
            m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
            m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
            m_Gameplay_PrimaryAbility = m_Gameplay.FindAction("PrimaryAbility", throwIfNotFound: true);
            m_Gameplay_SecondaryAbility = m_Gameplay.FindAction("SecondaryAbility", throwIfNotFound: true);
            m_Gameplay_TertiaryAbility = m_Gameplay.FindAction("TertiaryAbility", throwIfNotFound: true);
            m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
            m_Gameplay_TogglePause = m_Gameplay.FindAction("TogglePause", throwIfNotFound: true);
            // Menu Controls
            m_MenuControls = asset.FindActionMap("Menu Controls", throwIfNotFound: true);
            m_MenuControls_Navigate = m_MenuControls.FindAction("Navigate", throwIfNotFound: true);
            m_MenuControls_LeftClick = m_MenuControls.FindAction("Left Click", throwIfNotFound: true);
            m_MenuControls_Point = m_MenuControls.FindAction("Point", throwIfNotFound: true);
            m_MenuControls_Submit = m_MenuControls.FindAction("Submit", throwIfNotFound: true);
            m_MenuControls_Cancel = m_MenuControls.FindAction("Cancel", throwIfNotFound: true);
            m_MenuControls_TogglePause = m_MenuControls.FindAction("TogglePause", throwIfNotFound: true);
            m_MenuControls_RightClick = m_MenuControls.FindAction("Right Click", throwIfNotFound: true);
            m_MenuControls_ToggleInventory = m_MenuControls.FindAction("ToggleInventory", throwIfNotFound: true);
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

        // Gameplay
        private readonly InputActionMap m_Gameplay;
        private IGameplayActions m_GameplayActionsCallbackInterface;
        private readonly InputAction m_Gameplay_Movement;
        private readonly InputAction m_Gameplay_PrimaryAbility;
        private readonly InputAction m_Gameplay_SecondaryAbility;
        private readonly InputAction m_Gameplay_TertiaryAbility;
        private readonly InputAction m_Gameplay_Look;
        private readonly InputAction m_Gameplay_TogglePause;
        public struct GameplayActions
        {
            private @GameplayInputActions m_Wrapper;
            public GameplayActions(@GameplayInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
            public InputAction @PrimaryAbility => m_Wrapper.m_Gameplay_PrimaryAbility;
            public InputAction @SecondaryAbility => m_Wrapper.m_Gameplay_SecondaryAbility;
            public InputAction @TertiaryAbility => m_Wrapper.m_Gameplay_TertiaryAbility;
            public InputAction @Look => m_Wrapper.m_Gameplay_Look;
            public InputAction @TogglePause => m_Wrapper.m_Gameplay_TogglePause;
            public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
            public void SetCallbacks(IGameplayActions instance)
            {
                if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                    @PrimaryAbility.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryAbility;
                    @PrimaryAbility.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryAbility;
                    @PrimaryAbility.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryAbility;
                    @SecondaryAbility.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryAbility;
                    @SecondaryAbility.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryAbility;
                    @SecondaryAbility.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryAbility;
                    @TertiaryAbility.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTertiaryAbility;
                    @TertiaryAbility.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTertiaryAbility;
                    @TertiaryAbility.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTertiaryAbility;
                    @Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @TogglePause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTogglePause;
                    @TogglePause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTogglePause;
                    @TogglePause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTogglePause;
                }
                m_Wrapper.m_GameplayActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @PrimaryAbility.started += instance.OnPrimaryAbility;
                    @PrimaryAbility.performed += instance.OnPrimaryAbility;
                    @PrimaryAbility.canceled += instance.OnPrimaryAbility;
                    @SecondaryAbility.started += instance.OnSecondaryAbility;
                    @SecondaryAbility.performed += instance.OnSecondaryAbility;
                    @SecondaryAbility.canceled += instance.OnSecondaryAbility;
                    @TertiaryAbility.started += instance.OnTertiaryAbility;
                    @TertiaryAbility.performed += instance.OnTertiaryAbility;
                    @TertiaryAbility.canceled += instance.OnTertiaryAbility;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @TogglePause.started += instance.OnTogglePause;
                    @TogglePause.performed += instance.OnTogglePause;
                    @TogglePause.canceled += instance.OnTogglePause;
                }
            }
        }
        public GameplayActions @Gameplay => new GameplayActions(this);

        // Menu Controls
        private readonly InputActionMap m_MenuControls;
        private IMenuControlsActions m_MenuControlsActionsCallbackInterface;
        private readonly InputAction m_MenuControls_Navigate;
        private readonly InputAction m_MenuControls_LeftClick;
        private readonly InputAction m_MenuControls_Point;
        private readonly InputAction m_MenuControls_Submit;
        private readonly InputAction m_MenuControls_Cancel;
        private readonly InputAction m_MenuControls_TogglePause;
        private readonly InputAction m_MenuControls_RightClick;
        private readonly InputAction m_MenuControls_ToggleInventory;
        public struct MenuControlsActions
        {
            private @GameplayInputActions m_Wrapper;
            public MenuControlsActions(@GameplayInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Navigate => m_Wrapper.m_MenuControls_Navigate;
            public InputAction @LeftClick => m_Wrapper.m_MenuControls_LeftClick;
            public InputAction @Point => m_Wrapper.m_MenuControls_Point;
            public InputAction @Submit => m_Wrapper.m_MenuControls_Submit;
            public InputAction @Cancel => m_Wrapper.m_MenuControls_Cancel;
            public InputAction @TogglePause => m_Wrapper.m_MenuControls_TogglePause;
            public InputAction @RightClick => m_Wrapper.m_MenuControls_RightClick;
            public InputAction @ToggleInventory => m_Wrapper.m_MenuControls_ToggleInventory;
            public InputActionMap Get() { return m_Wrapper.m_MenuControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuControlsActions set) { return set.Get(); }
            public void SetCallbacks(IMenuControlsActions instance)
            {
                if (m_Wrapper.m_MenuControlsActionsCallbackInterface != null)
                {
                    @Navigate.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNavigate;
                    @Navigate.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNavigate;
                    @Navigate.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNavigate;
                    @LeftClick.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnLeftClick;
                    @LeftClick.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnLeftClick;
                    @LeftClick.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnLeftClick;
                    @Point.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnPoint;
                    @Point.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnPoint;
                    @Point.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnPoint;
                    @Submit.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSubmit;
                    @Submit.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSubmit;
                    @Submit.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSubmit;
                    @Cancel.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnCancel;
                    @TogglePause.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnTogglePause;
                    @TogglePause.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnTogglePause;
                    @TogglePause.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnTogglePause;
                    @RightClick.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnRightClick;
                    @RightClick.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnRightClick;
                    @RightClick.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnRightClick;
                    @ToggleInventory.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnToggleInventory;
                    @ToggleInventory.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnToggleInventory;
                    @ToggleInventory.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnToggleInventory;
                }
                m_Wrapper.m_MenuControlsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Navigate.started += instance.OnNavigate;
                    @Navigate.performed += instance.OnNavigate;
                    @Navigate.canceled += instance.OnNavigate;
                    @LeftClick.started += instance.OnLeftClick;
                    @LeftClick.performed += instance.OnLeftClick;
                    @LeftClick.canceled += instance.OnLeftClick;
                    @Point.started += instance.OnPoint;
                    @Point.performed += instance.OnPoint;
                    @Point.canceled += instance.OnPoint;
                    @Submit.started += instance.OnSubmit;
                    @Submit.performed += instance.OnSubmit;
                    @Submit.canceled += instance.OnSubmit;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                    @TogglePause.started += instance.OnTogglePause;
                    @TogglePause.performed += instance.OnTogglePause;
                    @TogglePause.canceled += instance.OnTogglePause;
                    @RightClick.started += instance.OnRightClick;
                    @RightClick.performed += instance.OnRightClick;
                    @RightClick.canceled += instance.OnRightClick;
                    @ToggleInventory.started += instance.OnToggleInventory;
                    @ToggleInventory.performed += instance.OnToggleInventory;
                    @ToggleInventory.canceled += instance.OnToggleInventory;
                }
            }
        }
        public MenuControlsActions @MenuControls => new MenuControlsActions(this);
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        private int m_KeyboardAndMouseSchemeIndex = -1;
        public InputControlScheme KeyboardAndMouseScheme
        {
            get
            {
                if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard And Mouse");
                return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
            }
        }
        private int m_TouchscreenSchemeIndex = -1;
        public InputControlScheme TouchscreenScheme
        {
            get
            {
                if (m_TouchscreenSchemeIndex == -1) m_TouchscreenSchemeIndex = asset.FindControlSchemeIndex("Touchscreen");
                return asset.controlSchemes[m_TouchscreenSchemeIndex];
            }
        }
        public interface IGameplayActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnPrimaryAbility(InputAction.CallbackContext context);
            void OnSecondaryAbility(InputAction.CallbackContext context);
            void OnTertiaryAbility(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnTogglePause(InputAction.CallbackContext context);
        }
        public interface IMenuControlsActions
        {
            void OnNavigate(InputAction.CallbackContext context);
            void OnLeftClick(InputAction.CallbackContext context);
            void OnPoint(InputAction.CallbackContext context);
            void OnSubmit(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnTogglePause(InputAction.CallbackContext context);
            void OnRightClick(InputAction.CallbackContext context);
            void OnToggleInventory(InputAction.CallbackContext context);
        }
    }
}
