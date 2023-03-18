using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SlimeRPG.UI
{
    [RequireComponent(typeof(UIWindow))/*, RequireComponent(typeof(UIAlwaysOnTop))*/]
    public class UIModalBox : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private Text text1;
        [SerializeField] private Text text2;
        
        [Header("Buttons")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private Text confirmButtonText;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Text cancelButtonText;
        
        [Header("Inputs")]
        [SerializeField] private string confirmInput = "Submit";
        [SerializeField] private string cancelInput = "Cancel";

        private UIWindow window;

        [Header("Events")]
        public UnityEvent onConfirm = new UnityEvent();
        public UnityEvent onCancel = new UnityEvent();

        /// <summary>
        /// Gets a value indicating whether this modal box is active.
        /// </summary>
        public bool isActive { get; private set; }

        protected void Awake()
        {
            // Make sure we have the window component
            if (window == null)
            {
                window = GetComponent<UIWindow>();
            }

            // Prepare some window parameters
            window.ID = UIWindowID.ModalBox;
            window.escapeAction = UIWindow.EscapeKeyAction.None;

            // Hook an event to the window
            window.onTransitionComplete.AddListener(OnWindowTransitionEnd);

            // Prepare the always on top component
            UIAlwaysOnTop aot = GetComponent<UIAlwaysOnTop>();
            aot.order = UIAlwaysOnTop.ModalBoxOrder;

            // Hook the button click event
            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(Confirm);
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(Close);
            }
        }

        protected void Update()
        {
            // 이거 없애야할듯.
            if (!string.IsNullOrEmpty(cancelInput) && Input.GetButtonDown(cancelInput))
                Close();

            if (!string.IsNullOrEmpty(confirmInput) && Input.GetButtonDown(confirmInput))
                Confirm();
        }

        public void SetText1(string text)
        {
            if (text1 != null)
            {
                text1.text = text;
                text1.gameObject.SetActive(!string.IsNullOrEmpty(text));
            }
        }

        public void SetText2(string text)
        {
            if (text2 != null)
            {
                text2.text = text;
                text2.gameObject.SetActive(!string.IsNullOrEmpty(text));
            }
        }

        public void SetConfirmButtonText(string text)
        {
            if (confirmButtonText != null)
            {
                confirmButtonText.text = text;
            }
        }

        public void SetCancelButtonText(string text)
        {
            if (cancelButtonText != null)
            {
                cancelButtonText.text = text;
            }
        }

        public void Show()
        {
            isActive = true;

            if (UIModalBoxManager.Instance != null)
                UIModalBoxManager.Instance.RegisterActiveBox(this);

            // Show the modal
            if (window != null)
            {
                window.Show();
            }
        }

        public void Close()
        {
            Hide();

            // Invoke the cancel event
            if (onCancel != null)
            {
                onCancel.Invoke();
            }
        }

        public void Confirm()
        {
            Hide();

            // Invoke the confirm event
            if (onConfirm != null)
            {
                onConfirm.Invoke();
            }
        }

        private void Hide()
        {
            isActive = false;

            if (UIModalBoxManager.Instance != null)
                UIModalBoxManager.Instance.UnregisterActiveBox(this);

            // Hide the modal
            if (window != null)
            {
                window.Hide();
            }
        }

        public void OnWindowTransitionEnd(UIWindow window, UIWindow.VisualState state)
        {
            // Destroy the modal box when hidden
            if (state == UIWindow.VisualState.Hidden)
            {
                Destroy(gameObject);
            }
        }
    }
}
