using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using SlimeRPG.Gameplay.Input;


namespace SlimeRPG.UI
{
    public enum UIWindowID
    {
        None,
        Settings = 2,
        GameMenu = 3,
        ModalBox = 4,
        Character = 5,
        Inventory = 6,
        SpellBook = 7,
        Dialog = 8,
        Detail = 9,
        SkillInfo = 10,
    }

    public class UIWindowManager : MonoBehaviour, GameplayInputActions.IMenuControlsActions
    {
        private static UIWindowManager instance;

        public static UIWindowManager Instance
        {
            get { return instance; }
        }

        private GameplayInputActions userInputActions;

        //private bool escapeUsed = false;

        // public bool EscapeUsed
        //{
        //    get { return escapeUsed; }
        //}

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance.Equals(this))
                instance = null;
        }

        void OnEnable()
        {
            if (userInputActions == null)
                userInputActions = new GameplayInputActions();

            userInputActions.MenuControls.SetCallbacks(this);
            userInputActions.MenuControls.Enable();
        }

        void OnDisable()
        {
            userInputActions.MenuControls.Disable();
        }

        public UIWindow GetWindow(UIWindowID windowID)
        {
            foreach (var window in UIWindow.GetWindows())
            {
                if (window.ID == windowID)
                    return window;
            }

            return null;
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // Check for currently opened modal and exit this method if one is found
                UIModalBox[] modalBoxes = FindObjectsOfType<UIModalBox>();

                if (modalBoxes.Length > 0)
                {
                    foreach (UIModalBox box in modalBoxes)
                    {
                        // If the box is active
                        if (box.isActive && box.isActiveAndEnabled && box.gameObject.activeInHierarchy)
                            return;
                    }
                }

                // Get the windows list
                List<UIWindow> windows = UIWindow.GetWindows();

                // Loop through the windows and hide if required
                foreach (UIWindow window in windows)
                {
                    // Check if the window has escape key action
                    if (window.escapeAction != UIWindow.EscapeKeyAction.None)
                    {
                        // Check if the window should be hidden on escape
                        if (window.IsOpen && (window.escapeAction == UIWindow.EscapeKeyAction.Hide || window.escapeAction == UIWindow.EscapeKeyAction.Toggle || (window.escapeAction == UIWindow.EscapeKeyAction.HideIfFocused && window.IsFocused)))
                        {
                            // Hide the window
                            window.Hide();

                            // Mark the escape input as used
                            // this.m_EscapeUsed = true;
                        }
                    }
                }

                // Exit the method if the escape was used for hiding windows
                //if (this.m_EscapeUsed)
                //    return;

                // Loop through the windows again and show any if required
                foreach (UIWindow window in windows)
                {
                    // Check if the window has escape key action toggle and is not shown
                    if (!window.IsOpen && window.escapeAction == UIWindow.EscapeKeyAction.Toggle)
                    {
                        // Show the window
                        window.Show();
                    }
                }
            }
        }

        public void OnTogglePause(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnToggleInventory(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var inventory = GetWindow(UIWindowID.Inventory);
                inventory.Toggle();
            }
        }
    }
}
