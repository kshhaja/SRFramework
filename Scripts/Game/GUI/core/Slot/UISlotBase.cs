using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SlimeRPG.UI
{
	[ExecuteInEditMode, DisallowMultipleComponent]
	public class UISlotBase : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
	{

		public enum Transition
		{
			None,
			ColorTint,
			SpriteSwap,
			Animation
		}

		public enum DragKeyModifier
		{
			None,
			Control,
			Alt,
			Shift
		}

		protected GameObject currentDraggedObject;
		protected RectTransform currentDraggingPlane;

		public Graphic iconGraphic;

		[SerializeField, Tooltip("The game object that should be cloned on drag.")]
		private GameObject cloneTarget;

		[SerializeField, Tooltip("Should the drag and drop functionallty be enabled.")]
		private bool dragAndDropEnabled = true;

		[SerializeField, Tooltip("If set to static the slot won't be unassigned when drag and drop is preformed.")]
		private bool isStatic = false;

		[SerializeField, Tooltip("Should the icon assigned to the slot be throwable.")]
		private bool allowThrowAway = true;

		[SerializeField, Tooltip("Should the tooltip functionallty be enabled.")]
		private bool tooltipEnabled = true;

		[SerializeField, Tooltip("How long of a delay to expect before showing the tooltip.")]
		private float tooltipDelay = 1f;

		public Transition hoverTransition = Transition.None;
		public Graphic hoverTargetGraphic;
		public Color hoverNormalColor = Color.white;
		public Color hoverHighlightColor = Color.white;
		public float hoverTransitionDuration = 0.15f;
		public Sprite hoverOverrideSprite;
		public string hoverNormalTrigger = "Normal";
		public string hoverHighlightTrigger = "Highlighted";

		public Transition pressTransition = Transition.None;
		public Graphic pressTargetGraphic;
		public Color pressNormalColor = Color.white;
		public Color pressPressColor = new Color(0.6117f, 0.6117f, 0.6117f, 1f);
		public float pressTransitionDuration = 0.15f;
		public Sprite pressOverrideSprite;
		public string pressNormalTrigger = "Normal";
		public string pressPressTrigger = "Pressed";

		[SerializeField, Tooltip("Should the pressed state transition to normal state instantly.")]
		private bool pressTransitionInstaOut = true;

		[SerializeField, Tooltip("Should the pressed state force normal state transition on the hover target.")]
		private bool pressForceHoverNormal = true;

		private bool isPointerDown = false;
		private bool isPointerInside = false;
		private bool dragHasBegan = false;
		private bool dropPreformed = false;
		private bool isTooltipShown = false;

		
		
		protected override void Start()
		{
			// Check if the slot is not assigned but the icon graphic is active
			if (!IsAssigned() && iconGraphic != null && iconGraphic.gameObject.activeSelf)
			{
				// Disable the icon graphic object
				iconGraphic.gameObject.SetActive(false);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			// Instant transition
			EvaluateAndTransitionHoveredState(true);
			EvaluateAndTransitionPressedState(true);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			isPointerInside = false;
			isPointerDown = false;

			// Instant transition
			EvaluateAndTransitionHoveredState(true);
			EvaluateAndTransitionPressedState(true);
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			isPointerInside = true;
			EvaluateAndTransitionHoveredState(false);

			// Check if tooltip is enabled
			if (enabled && IsActive() && tooltipEnabled)
			{
				// Start the tooltip delayed show coroutine
				// If delay is set at all
				if (tooltipDelay > 0f)
				{
					StartCoroutine("TooltipDelayedShow");
				}
				else
				{
					InternalShowTooltip();
				}
			}
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			isPointerInside = false;
			EvaluateAndTransitionHoveredState(false);
			InternalHideTooltip();
		}

		public virtual void OnTooltip(bool show)
		{
		}

		public virtual void OnPointerDown(PointerEventData eventData)
		{
			isPointerDown = true;
			EvaluateAndTransitionPressedState(false);

			// Hide the tooltip
			InternalHideTooltip();
		}

		public virtual void OnPointerUp(PointerEventData eventData)
		{
			isPointerDown = false;
			EvaluateAndTransitionPressedState(this.pressTransitionInstaOut);
		}

		public virtual void OnPointerClick(PointerEventData eventData) 
		{ 
		}

		protected bool IsHighlighted(BaseEventData eventData)
		{
			if (!IsActive())
				return false;

			if (eventData is PointerEventData)
			{
				PointerEventData pointerEventData = eventData as PointerEventData;
				return ((isPointerDown && !isPointerInside && pointerEventData.pointerPress == base.gameObject) || (!this.isPointerDown && this.isPointerInside && pointerEventData.pointerPress == base.gameObject) || (!this.isPointerDown && this.isPointerInside && pointerEventData.pointerPress == null));
			}

			return false;
		}

		protected bool IsPressed(BaseEventData eventData)
		{
			return IsActive() && isPointerInside && isPointerDown;
		}

		protected virtual void EvaluateAndTransitionHoveredState(bool instant)
		{
			if (!IsActive() || hoverTargetGraphic == null || !hoverTargetGraphic.gameObject.activeInHierarchy)
				return;

			// Determine what should the state of the hover target be
			bool highlighted = (pressForceHoverNormal ? (isPointerInside && !isPointerDown) : isPointerInside);

			// Do the transition
			switch (hoverTransition)
			{
				case Transition.ColorTint:
					{
						StartColorTween(hoverTargetGraphic, (highlighted ? hoverHighlightColor : hoverNormalColor), (instant ? 0f : hoverTransitionDuration));
						break;
					}
				case Transition.SpriteSwap:
					{
						DoSpriteSwap(hoverTargetGraphic, (highlighted ? hoverOverrideSprite : null));
						break;
					}
				case Transition.Animation:
					{
						TriggerHoverStateAnimation(highlighted ? hoverHighlightTrigger : hoverNormalTrigger);
						break;
					}
			}
		}

		protected virtual void EvaluateAndTransitionPressedState(bool instant)
		{
			if (!IsActive() || pressTargetGraphic == null || !pressTargetGraphic.gameObject.activeInHierarchy)
				return;

			// Do the transition
			switch (pressTransition)
			{
				case Transition.ColorTint:
					{
						StartColorTween(pressTargetGraphic, (isPointerDown ? pressPressColor : pressNormalColor), (instant ? 0f : pressTransitionDuration));
						break;
					}
				case Transition.SpriteSwap:
					{
						DoSpriteSwap(pressTargetGraphic, (isPointerDown ? pressOverrideSprite : null));
						break;
					}
				case Transition.Animation:
					{
						TriggerPressStateAnimation(isPointerDown ? pressPressTrigger : pressNormalTrigger);
						break;
					}
			}

			// If we should force normal state transition on the hover target
			if (pressForceHoverNormal)
				EvaluateAndTransitionHoveredState(false);
		}

		protected virtual void StartColorTween(Graphic target, Color targetColor, float duration)
		{
			if (target == null)
				return;

			target.CrossFadeColor(targetColor, duration, true, true);
		}

		protected virtual void DoSpriteSwap(Graphic target, Sprite newSprite)
		{
			if (target == null)
				return;

			Image image = target as Image;

			if (image == null)
				return;

			image.overrideSprite = newSprite;
		}

		private void TriggerHoverStateAnimation(string triggername)
		{
			if (hoverTargetGraphic == null)
				return;

			// Get the animator on the target game object
			Animator animator = hoverTargetGraphic.gameObject.GetComponent<Animator>();

			if (animator == null || !animator.enabled || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
				return;

			animator.ResetTrigger(hoverNormalTrigger);
			animator.ResetTrigger(hoverHighlightTrigger);
			animator.SetTrigger(triggername);
		}

		private void TriggerPressStateAnimation(string triggername)
		{
			if (pressTargetGraphic == null)
				return;

			// Get the animator on the target game object
			Animator animator = pressTargetGraphic.gameObject.GetComponent<Animator>();

			if (animator == null || !animator.enabled || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
				return;

			animator.ResetTrigger(pressNormalTrigger);
			animator.ResetTrigger(pressPressTrigger);
			animator.SetTrigger(triggername);
		}

		public virtual bool IsAssigned()
		{
			return (GetIconSprite() != null || GetIconTexture() != null);
		}

		public bool Assign(Sprite icon)
		{
			// Set the icon
			SetIcon(icon);

			if (icon == null)
			{
				return false;
			}

			return true;
		}

		public bool Assign(Texture icon)
		{
			if (icon == null)
				return false;

			// Set the icon
			SetIcon(icon);

			return true;
		}

		public virtual bool Assign(object source)
		{
			if (source is UISlotBase)
			{
				UISlotBase sourceSlot = source as UISlotBase;

				if (sourceSlot != null)
				{
					// Assign by sprite or texture
					if (sourceSlot.GetIconSprite() != null)
					{
						return Assign(sourceSlot.GetIconSprite());
					}
					else if (sourceSlot.GetIconTexture() != null)
					{
						return Assign(sourceSlot.GetIconTexture());
					}
				}
			}

			return false;
		}

		public virtual void Unassign()
		{
			// Remove the icon
			ClearIcon();
		}

		public Sprite GetIconSprite()
		{
			// Check if the icon graphic valid image
			if (iconGraphic == null || !(iconGraphic is Image))
				return null;

			return (iconGraphic as Image).sprite;
		}

		public Texture GetIconTexture()
		{
			// Check if the icon graphic valid image
			if (iconGraphic == null || !(iconGraphic is RawImage))
				return null;

			return (iconGraphic as RawImage).texture;
		}

		public Object GetIconAsObject()
		{
			if (iconGraphic == null)
				return null;

			if (iconGraphic is Image)
			{
				return GetIconSprite();
			}
			else if (iconGraphic is RawImage)
			{
				return GetIconTexture();
			}

			// Default
			return null;
		}

		public void SetIcon(Sprite iconSprite)
		{
			if (iconSprite == null)
			{
				iconGraphic.gameObject.SetActive(true);
				(iconGraphic as Image).color = Color.red;
				Debug.LogWarning("Icon Sprite is null.");
				return;
			}

			// Check if the icon graphic valid image
			if (iconGraphic == null || !(iconGraphic is Image))
				return;

			// Set the sprite
			(iconGraphic as Image).sprite = iconSprite;

			// Enable or disabled the icon graphic game object
			if (iconSprite != null && !iconGraphic.gameObject.activeSelf) this.iconGraphic.gameObject.SetActive(true);
			if (iconSprite == null && iconGraphic.gameObject.activeSelf) this.iconGraphic.gameObject.SetActive(false);
		}

		public void SetIcon(Texture iconTex)
		{
			// Check if the icon graphic valid raw image
			if (iconGraphic == null || !(iconGraphic is RawImage))
				return;

			// Set the sprite
			(iconGraphic as RawImage).texture = iconTex;

			// Enable or disabled the icon graphic game object
			if (iconTex != null && !iconGraphic.gameObject.activeSelf) iconGraphic.gameObject.SetActive(true);
			if (iconTex == null && iconGraphic.gameObject.activeSelf) iconGraphic.gameObject.SetActive(false);
		}

		public void ClearIcon()
		{
			// Check if the icon graphic valid
			if (iconGraphic == null)
				return;

			// In case of image
			if (iconGraphic is Image)
				(iconGraphic as Image).sprite = null;

			// In case of raw image
			if (iconGraphic is RawImage)
				(iconGraphic as RawImage).texture = null;

			// Disable the game object
			iconGraphic.gameObject.SetActive(false);
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (!enabled || !IsAssigned() || !dragAndDropEnabled)
			{
				eventData.Reset();
				return;
			}

			// Start the drag
			dragHasBegan = true;

			// Create the temporary icon for dragging
			CreateTemporaryIcon(eventData);

			// Prevent event propagation
			eventData.Use();
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			// Check if the dragging has been started
			if (dragHasBegan)
			{
				// Update the dragged object's position
				if (currentDraggedObject != null)
					UpdateDraggedPosition(eventData);
			}
		}

		public virtual void OnDrop(PointerEventData eventData)
		{
			// Get the source slot
			UISlotBase source = (eventData.pointerPress != null) ? eventData.pointerPress.GetComponent<UISlotBase>() : null;

			// Make sure we have the source slot
			if (source == null || !source.IsAssigned() || !source.dragAndDropEnabled)
				return;

			// Notify the source that a drop was performed so it does not unassign
			source.dropPreformed = true;

			// Check if this slot is enabled and it's drag and drop feature is enabled
			if (!enabled || !dragAndDropEnabled)
				return;

			// Prepare a variable indicating whether the assign process was successful
			bool assignSuccess = false;

			// Normal empty slot assignment
			if (!IsAssigned())
			{
				// Assign the target slot with the info from the source
				assignSuccess = Assign(source);

				// Unassign the source on successful assignment and the source is not static
				if (assignSuccess && !source.isStatic)
					source.Unassign();
			}
			// The target slot is assigned
			else
			{
				// If the target slot is not static
				// and we have a source slot that is not static
				if (!isStatic && !source.isStatic)
				{
					// Check if we can swap
					if (CanSwapWith(source) && source.CanSwapWith(this))
					{
						// Swap the slots
						assignSuccess = source.PerformSlotSwap(this);
					}
				}
				// If the target slot is not static
				// and the source slot is a static one
				else if (!isStatic && source.isStatic)
				{
					assignSuccess = Assign(source);
				}
			}

			// If this slot failed to be assigned
			if (!assignSuccess)
			{
				OnAssignBySlotFailed(source);
			}
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			// Check if a drag was initialized at all
			if (!dragHasBegan)
				return;

			// Reset the drag begin bool
			dragHasBegan = false;

			// Destroy the dragged icon object
			if (currentDraggedObject != null)
				Destroy(currentDraggedObject);

			// Reset the variables
			currentDraggedObject = null;
			currentDraggingPlane = null;

			// Check if we are returning the icon to the same slot
			// By checking if the slot is highlighted
			if (IsHighlighted(eventData))
				return;

			// Check if no drop was preformed
			if (!dropPreformed)
				// Try to throw away the assigned content
				OnThrowAway();
			else
				// Reset the drop preformed variable
				dropPreformed = false;
		}

		public virtual bool CanSwapWith(Object target)
		{
			return (target is UISlotBase);
		}

		public virtual bool PerformSlotSwap(Object targetObject)
		{
			// Get the source slot
			UISlotBase targetSlot = (targetObject as UISlotBase);

			// Get the target slot icon
			Object targetIcon = targetSlot.GetIconAsObject();

			// Assign the target slot with this one
			bool assign1 = targetSlot.Assign(this);

			// Assign this slot by the target slot icon
			bool assign2 = Assign(targetIcon);

			// Return the status
			return (assign1 && assign2);
		}

		protected virtual void OnAssignBySlotFailed(Object source)
		{
			Debug.Log("UISlotBase (" + gameObject.name + ") failed to get assigned by (" + (source as UISlotBase).gameObject.name + ").");
		}

		protected virtual void OnThrowAway()
		{
			// Check if throwing away is allowed
			if (allowThrowAway)
				// Throw away successful, unassign the slot
				Unassign();
			else
				// Throw away was denied
				OnThrowAwayDenied();
		}

		protected virtual void OnThrowAwayDenied() 
		{ 
		}

		protected virtual void CreateTemporaryIcon(PointerEventData eventData)
		{
			Canvas canvas = UIUtility.FindInParents<Canvas>(gameObject);

			if (canvas == null || iconGraphic == null)
				return;

			// Create temporary panel
			GameObject iconObj = Instantiate((cloneTarget == null) ? iconGraphic.gameObject : cloneTarget);

			iconObj.transform.localScale = new Vector3(1f, 1f, 1f);
			iconObj.transform.SetParent(canvas.transform, false);
			iconObj.transform.SetAsLastSibling();
			(iconObj.transform as RectTransform).pivot = new Vector2(0.5f, 0.5f);

			// The icon will be under the cursor.
			// We want it to be ignored by the event system.
			iconObj.AddComponent<UIIgnoreRaycast>();

			// Save the dragging plane
			currentDraggingPlane = canvas.transform as RectTransform;

			// Set as the current dragging object
			currentDraggedObject = iconObj;

			// Update the icon position
			UpdateDraggedPosition(eventData);
		}

		private void UpdateDraggedPosition(PointerEventData data)
		{
			var rt = currentDraggedObject.GetComponent<RectTransform>();
			Vector3 globalMousePos;

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(currentDraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
			{
				rt.position = globalMousePos;
				rt.rotation = currentDraggingPlane.rotation;
			}
		}

		protected void InternalShowTooltip()
		{
			// Call the on tooltip only if it's currently not shown
			if (!isTooltipShown)
			{
				isTooltipShown = true;
				OnTooltip(true);
			}
		}

		protected void InternalHideTooltip()
		{
			// Cancel the delayed show coroutine
			StopCoroutine("TooltipDelayedShow");

			// Call the on tooltip only if it's currently shown
			if (isTooltipShown)
			{
				isTooltipShown = false;
				OnTooltip(false);
			}
		}

		protected IEnumerator TooltipDelayedShow()
		{
			yield return new WaitForSeconds(tooltipDelay);
			InternalShowTooltip();
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			hoverTransitionDuration = Mathf.Max(hoverTransitionDuration, 0f);
			pressTransitionDuration = Mathf.Max(pressTransitionDuration, 0f);

			if (isActiveAndEnabled)
			{
				DoSpriteSwap(hoverTargetGraphic, null);
				DoSpriteSwap(pressTargetGraphic, null);

				if (!EditorApplication.isPlayingOrWillChangePlaymode)
				{
					// Instant transition
					EvaluateAndTransitionHoveredState(true);
					EvaluateAndTransitionPressedState(true);
				}
				else
				{
					// Regular transition
					EvaluateAndTransitionHoveredState(false);
					EvaluateAndTransitionPressedState(false);
				}
			}
		}
#endif

	}
}
