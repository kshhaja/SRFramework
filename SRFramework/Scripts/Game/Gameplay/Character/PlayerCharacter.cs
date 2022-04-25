using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using SlimeRPG.Gameplay.Input;
using SlimeRPG.Gameplay.Character.Ability;
using SlimeRPG.Gameplay.Character.Controller;


namespace SlimeRPG.Gameplay.Character
{
    public class PlayerCharacter : CharacterBase, GameplayInputActions.IGameplayActions
    {
        public enum ActionHandler
        {
            // based on keyboard, mouse
            primary,
            secondary,
            tertiary,

            custom01,
            custom02,
            custom03,
            custom04,
            custom05,
        }

        // for network. 0 is you
        [Header("Player ID")]
        private int playerID = 0;
        public int PlayerID => playerID;

        private GameplayInputActions userInputActions;
        private Dictionary<ActionHandler, int?> inputActions = new Dictionary<ActionHandler, int?>();


        protected override void Awake()
        {
            base.Awake();

            // init empty actions
            foreach (ActionHandler handler in System.Enum.GetValues(typeof(ActionHandler)))
                inputActions.Add(handler, null);
        }

        private void OnEnable()
        {
            if (userInputActions == null)
                userInputActions = new GameplayInputActions();

            userInputActions.Gameplay.SetCallbacks(this);
            userInputActions.Gameplay.Enable();
        }

        private void OnDisable()
        {
            userInputActions.Gameplay.Disable();
        }

        public void SetPlayerID(int index)
        {
            playerID = index;
        }

        #region Ability System
        public void GrantAbility(ActionHandler handler, AbilityBase ability)
        {
            inputActions[handler] = GrantAbility(ability);
        }

        public void CastAbility(ActionHandler handler)
        {
            if (inputActions.TryGetValue(handler, out var index))
            {
                if (index != null)
                {
                    CastAbility(index.Value);
                }
            }
        }

        #endregion

        #region Player Input Events
        public void OnMovement(InputAction.CallbackContext context)
        {
            Vector2 inputMovement = context.ReadValue<Vector2>();
            var dir = new Vector3(inputMovement.x, 0, inputMovement.y);
            MoveTo(dir);

            if (movementController is PlayerMovementController)
            {
                // input 값을 조금더 깔끔하게 공유할수있는 방법을 마련해보자.
                var p = (movementController as PlayerMovementController);
                p.input.x = inputMovement.x;
                p.input.z = inputMovement.y;
                p.ControlKeepDirection();
            }
        }

        public void OnPrimaryAbility(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                CastAbility(ActionHandler.primary);
                // animationController.PlayAttackAnimation();
            }
        }

        public void OnSecondaryAbility(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                CastAbility(ActionHandler.secondary);
            }
        }

        public void OnTertiaryAbility(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                CastAbility(ActionHandler.tertiary);
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            /// 여긴 입력장치마다 다르게 처리해야하겠다.
            /// 키보드일 경우엔 Mouse ScreenPosition,
            /// 게임패드일 경우엔 바라보는 방향벡터
            var pos = context.ReadValue<Vector2>();
            SetAimPoint(pos);
        }
        
        public void OnTogglePause(InputAction.CallbackContext context)
        {
        }
        #endregion

        #region input device events
        public void OnDeviceLost()
        {

        }

        public void OnDeviceRegained()
        {
            StartCoroutine(WaitForDeviceToBeRegained());
        }

        public void OnControlsChanged()
        {

            //if (playerInput.currentControlScheme != currentControlScheme)
            //{
            //    currentControlScheme = playerInput.currentControlScheme;

            //    RemoveAllBindingOverrides();
            //}
        }

        IEnumerator WaitForDeviceToBeRegained()
        {
            yield return new WaitForSeconds(0.1f);
        }

        void RemoveAllBindingOverrides()
        {
            // InputActionRebindingExtensions.RemoveAllBindingOverrides(playerInput.currentActionMap);
        }

        #endregion
    }
}
