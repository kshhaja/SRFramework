using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

namespace SlimeRPG.UI
{
	[DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(CanvasGroup))]
	public class UIWindow : MonoBehaviour, IEventSystemHandler, ISelectHandler, IPointerDownHandler
	{

		public enum Transition
		{
			Instant,
			Fade
		}

		public enum VisualState
		{
			Shown,
			Hidden
		}

		public enum EscapeKeyAction
		{
			None,
			Hide,
			HideIfFocused,
			Toggle
		}

		[Serializable] public class TransitionBeginEvent : UnityEvent<UIWindow, VisualState, bool> { }
		[Serializable] public class TransitionCompleteEvent : UnityEvent<UIWindow, VisualState> { }

		protected static UIWindow focusedWindow;
		public static UIWindow FocusedWindow { get { return focusedWindow; } private set { focusedWindow = value; } }

		[SerializeField] private UIWindowID windowId = UIWindowID.None;
		[SerializeField] private int customWindowId = 0;
		[SerializeField] private VisualState startingState = VisualState.Hidden;
		[SerializeField] private EscapeKeyAction escapeKeyAction = EscapeKeyAction.Hide;
		[SerializeField] private bool useBlackOverlay = false;

		[SerializeField] private bool focusAllowReparent = true;

		[SerializeField] private Transition transition = Transition.Instant;
		// [SerializeField] private TweenEasing m_TransitionEasing = TweenEasing.InOutQuint;
		[SerializeField] private float transitionDuration = 0.1f;

		[SerializeField] private KeyCode controlKeyCode;

		protected bool isFocused = false;
		private VisualState currentVisualState = VisualState.Hidden;
		private CanvasGroup canvasGroup;

		public UIWindowID ID
		{
			get { return windowId; }
			set { windowId = value; }
		}

		public int CustomID
		{
			get { return customWindowId; }
			set { customWindowId = value; }
		}

		/// <summary>
		/// Gets or sets the escape key action.
		/// </summary>
		public EscapeKeyAction escapeAction
		{
			get { return escapeKeyAction; }
			set { escapeKeyAction = value; }
		}

		public bool blackOverlay
		{
			get { return useBlackOverlay; }
			set
			{
				useBlackOverlay = value;

				if (Application.isPlaying && useBlackOverlay && this.isActiveAndEnabled)
				{
					// UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(this.gameObject);

					//if (overlay != null)
					//{
					//	if (value) this.onTransitionBegin.AddListener(overlay.OnTransitionBegin);
					//	else this.onTransitionBegin.RemoveListener(overlay.OnTransitionBegin);
					//}
				}
			}
		}

		public TransitionBeginEvent onTransitionBegin = new TransitionBeginEvent();
		public TransitionCompleteEvent onTransitionComplete = new TransitionCompleteEvent();

		public bool IsVisible => (canvasGroup != null && canvasGroup.alpha > 0f) ? true : false;

		public bool IsOpen => currentVisualState == VisualState.Shown;

		public bool IsFocused => isFocused;

		protected virtual bool IsActive => enabled && gameObject.activeInHierarchy;

		// Tween controls
		// [NonSerialized] private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		protected virtual void Awake()
        {
			//if (this.m_FloatTweenRunner == null)
			//	this.m_FloatTweenRunner = new TweenRunner<FloatTween>();

			//this.m_FloatTweenRunner.Init(this);

			// Get the canvas group
			canvasGroup = GetComponent<CanvasGroup>();

			// Transition to the starting state
			if (Application.isPlaying)
				ApplyVisualState(startingState);
		}

		protected virtual void Start()
		{
			// Assign new custom ID
			if (CustomID == 0)
				CustomID = NextUnusedCustomID;

			// Make sure we have a window manager in the scene if required
			if (escapeKeyAction != EscapeKeyAction.None)
			{
				UIWindowManager manager = Component.FindObjectOfType<UIWindowManager>();

				// Add a manager if not present
				if (manager == null)
				{
					GameObject newObj = new GameObject("Window Manager");
					newObj.AddComponent<UIWindowManager>();
					newObj.transform.SetAsFirstSibling();
				}
			}
		}

		protected virtual void Update()
		{
			// 이거 playerinput에 넣어야함.
			// PlayerInput을 매니저쪽으로 빼야되는구나...
			
		}

		protected virtual void OnEnable()
		{
			if (Application.isPlaying && useBlackOverlay)
			{
				// UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(this.gameObject);

				//if (overlay != null)
				//	onTransitionBegin.AddListener(overlay.OnTransitionBegin);
			}
		}

		protected virtual void OnDisable()
		{
			if (Application.isPlaying && useBlackOverlay)
			{
				//UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(this.gameObject);

				//if (overlay != null)
				//	this.onTransitionBegin.RemoveListener(overlay.OnTransitionBegin);
			}
		}

		public virtual void OnSelect(BaseEventData eventData)
		{
			// Focus the window
			Focus();
		}

		public virtual void OnPointerDown(PointerEventData eventData)
		{
			// Focus the window
			Focus();
		}

		public virtual void Focus()
		{
			if (isFocused)
				return;

			isFocused = true;

			// Call the static on focused window
			OnBeforeFocusWindow(this);

			// Bring the window forward
			BringToFront();
		}

		public void BringToFront()
		{
			UIUtility.BringToFront(gameObject, focusAllowReparent);
		}

		public virtual void Toggle()
		{
			if (currentVisualState == VisualState.Shown)
				Hide();
			else
				Show();
		}

		public virtual void Show()
		{
			Show(false);
		}

		public virtual void Show(bool instant)
		{
			if (!IsActive)
				return;

			// Focus the window
			Focus();

			// Check if the window is already shown
			if (currentVisualState == VisualState.Shown)
				return;

			// Transition
			EvaluateAndTransitionToVisualState(VisualState.Shown, instant);
		}

		public virtual void Hide()
		{
			Hide(false);
		}

		public virtual void Hide(bool instant)
		{
			if (!IsActive)
				return;

			// Check if the window is already hidden
			if (currentVisualState == VisualState.Hidden)
				return;

			// Transition
			EvaluateAndTransitionToVisualState(VisualState.Hidden, instant);
		}

		protected virtual void EvaluateAndTransitionToVisualState(VisualState state, bool instant)
		{
			float targetAlpha = (state == VisualState.Shown) ? 1f : 0f;

			// Call the transition begin event
			if (onTransitionBegin != null)
				onTransitionBegin.Invoke(this, state, (instant || transition == Transition.Instant));

			// Do the transition
			if (transition == Transition.Fade)
			{
				float duration = (instant) ? 0f : transitionDuration;

				// Tween the alpha
				StartAlphaTween(targetAlpha, duration, true);
			}
			else
			{
				// Set the alpha directly
				SetCanvasAlpha(targetAlpha);

				// Call the transition complete event, since it's instant
				if (onTransitionComplete != null)
					onTransitionComplete.Invoke(this, state);
			}

			// Save the state
			currentVisualState = state;

			// If we are transitioning to show, enable the canvas group raycast blocking
			if (state == VisualState.Shown)
			{
				canvasGroup.blocksRaycasts = true;
				//this.m_CanvasGroup.interactable = true;
			}
		}

		public virtual void ApplyVisualState(VisualState state)
		{
			float targetAlpha = (state == VisualState.Shown) ? 1f : 0f;

			// Set the alpha directly
			SetCanvasAlpha(targetAlpha);

			// Save the state
			currentVisualState = state;

			// If we are transitioning to show, enable the canvas group raycast blocking
			if (state == VisualState.Shown)
			{
				canvasGroup.blocksRaycasts = true;
				//this.m_CanvasGroup.interactable = true;
			}
			else
			{
				canvasGroup.blocksRaycasts = false;
				//this.m_CanvasGroup.interactable = false;
			}
		}

		public void StartAlphaTween(float targetAlpha, float duration, bool ignoreTimeScale)
		{
			if (canvasGroup == null)
				return;

			//var floatTween = new FloatTween { duration = duration, startFloat = this.m_CanvasGroup.alpha, targetFloat = targetAlpha };
			//floatTween.AddOnChangedCallback(SetCanvasAlpha);
			//floatTween.AddOnFinishCallback(OnTweenFinished);
			//floatTween.ignoreTimeScale = ignoreTimeScale;
			//floatTween.easing = this.m_TransitionEasing;
			//this.m_FloatTweenRunner.StartTween(floatTween);
		}

		public void SetCanvasAlpha(float alpha)
		{
			if (canvasGroup == null)
				return;

			// Set the alpha
			canvasGroup.alpha = alpha;

			// If the alpha is zero, disable block raycasts
			// Enabling them back on is done in the transition method
			if (alpha == 0f)
			{
				canvasGroup.blocksRaycasts = false;
				//this.m_CanvasGroup.interactable = false;
			}
		}

		protected virtual void OnTweenFinished()
		{
			// Call the transition complete event
			if (this.onTransitionComplete != null)
				this.onTransitionComplete.Invoke(this, currentVisualState);
		}

		#region Static Methods
		public static List<UIWindow> GetWindows()
		{
			List<UIWindow> windows = new List<UIWindow>();

			UIWindow[] ws = Resources.FindObjectsOfTypeAll<UIWindow>();

			foreach (UIWindow w in ws)
			{
				// Check if the window is active in the hierarchy
				if (w.gameObject.activeInHierarchy)
					windows.Add(w);
			}

			return windows;
		}

		public static int SortByCustomWindowID(UIWindow w1, UIWindow w2)
		{
			return w1.CustomID.CompareTo(w2.CustomID);
		}

		public static int NextUnusedCustomID
		{
			get
			{
				// Get the windows
				List<UIWindow> windows = UIWindow.GetWindows();

				if (GetWindows().Count > 0)
				{
					// Sort the windows by id
					windows.Sort(UIWindow.SortByCustomWindowID);

					// Return the last window id plus one
					return windows[windows.Count - 1].CustomID + 1;
				}

				// No windows, return 0
				return 0;
			}
		}

		public static UIWindow GetWindow(UIWindowID id)
		{
			// Get the windows and try finding the window with the given id
			foreach (UIWindow window in UIWindow.GetWindows())
				if (window.ID == id)
					return window;

			return null;
		}

		public static UIWindow GetWindowByCustomID(int customId)
		{
			// Get the windows and try finding the window with the given id
			foreach (UIWindow window in UIWindow.GetWindows())
				if (window.CustomID == customId)
					return window;

			return null;
		}

		public static void FocusWindow(UIWindowID id)
		{
			// Focus the window
			if (GetWindow(id) != null)
				GetWindow(id).Focus();
		}

		protected static void OnBeforeFocusWindow(UIWindow window)
		{
			if (focusedWindow != null)
				focusedWindow.isFocused = false;

			focusedWindow = window;
		}
		#endregion

#if UNITY_EDITOR
		private void OnValidate()
        {
			// 이 함수는 MonoBehaviour 라이프사이클과 상관없이 호출된다.
			// 플레이 중지를 하게되면 인스펙터 내용을 채우면서 호출하기 때문에 Awake가 아직 호출되지 않은 상황 발생.
			if (Application.isPlaying == false && canvasGroup)
				ApplyVisualState(startingState);
        }
#endif
	}
}
