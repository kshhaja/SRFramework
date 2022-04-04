using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace SlimeRPG.UI
{
    [DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(CanvasGroup))]
    public class UIScene : MonoBehaviour
    {
        public enum SceneType
        {
            Preloaded,
            Prefab,
            Resource
        }

        public enum Transition
        {
            None,
            Animation,
            CrossFade,
            SlideFromRight,
            SlideFromLeft,
            SlideFromTop,
            SlideFromBottom
        }

        [Serializable] public class OnActivateEvent : UnityEvent<UIScene> { }
        [Serializable] public class OnDeactivateEvent : UnityEvent<UIScene> { }

        private UISceneRegistry sceneManager;
        private bool animationState = false;

        [SerializeField] private int id = 0;
        [SerializeField] private bool isActivated = true;
        [SerializeField] private SceneType sceneType = SceneType.Preloaded;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject prefab;
        [SerializeField] /*[ResourcePath]*/ private string resource;
        [SerializeField] private Transition transition = Transition.None;
        [SerializeField] private float transitionDuration = 0.2f;
        // [SerializeField] private TweenEasing m_TransitionEasing = TweenEasing.InOutQuint;
        
        [SerializeField] private string animateInTrigger = "AnimateIn";
        [SerializeField] private string animateOutTrigger = "AnimateOut";
        
        [SerializeField] private GameObject firstSelected;

        /// <summary>
        /// Gets the scene id.
        /// </summary>
        public int ID
        {
            get { return id; }
        }

        public bool IsActivated
        {
            get { return isActivated; }
            set { if (value) { Activate(); } else { Deactivate(); } }
        }

        public SceneType Type => sceneType;
        
        public Transform Content
        {
            get { return content; }
            set { content = value; }
        }

        public Transition transitionType
        {
            get { return transition; }
            set { transition = value; }
        }

        public float transitionDurationValue
        {
            get { return transitionDuration; }
            set { transitionDuration = value; }
        }

        //public TweenEasing transitionEasing
        //{
        //    get { return m_TransitionEasing; }
        //    set { m_TransitionEasing = value; }
        //}

        public string animateInTriggerValue
        {
            get { return animateInTrigger; }
            set { animateInTrigger = value; }
        }

        public string animateOutTriggerValue
        {
            get { return animateOutTrigger; }
            set { animateOutTrigger = value; }
        }

		public OnActivateEvent onActivate = new OnActivateEvent();
        public OnDeactivateEvent onDeactivate = new OnDeactivateEvent();

        public RectTransform rectTransform
        {
            get { return (transform as RectTransform); }
        }

		public Animator animator
        {
            get { return GetComponent<Animator>(); }
        }

        private CanvasGroup canvasGroup;

        // Tween controls
        //[NonSerialized]
        //private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

        protected virtual void Awake()
        {
            //if (m_FloatTweenRunner == null)
            //    m_FloatTweenRunner = new TweenRunner<FloatTween>();

            //m_FloatTweenRunner.Init(this);

            // Get the scene mangaer
            sceneManager = UISceneRegistry.Instance;

            if (sceneManager == null)
            {
                Debug.LogWarning("Scene registry does not exist.");
                enabled = false;
                return;
            }

            // Set the initial animation state
            animationState = isActivated;

            // Get the canvas group
            canvasGroup = gameObject.GetComponent<CanvasGroup>();

            // Set the first selected game object for the navigation
            if (Application.isPlaying && isActivated && isActiveAndEnabled && firstSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
        }

        protected virtual void OnEnable()
        {
            // Register the scene
            if (sceneManager != null)
            {
                // Register only if in the scene
                if (gameObject.activeInHierarchy)
                {
                    sceneManager.RegisterScene(this);
                }
            }

            // Trigger the on activate event
            if (isActivated && onActivate != null)
                onActivate.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            // Unregister the scene
            if (sceneManager != null)
            {
                sceneManager.UnregisterScene(this);
            }
        }

        protected void Update()
        {
            if (animator != null && !string.IsNullOrEmpty(animateInTrigger) && !string.IsNullOrEmpty(animateOutTrigger))
            {
                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

                // Check which is the current state
                if (state.IsName(animateInTrigger) && !animationState)
                {
                    if (state.normalizedTime >= state.length)
                    {
                        // Flag as opened
                        animationState = true;

                        // On animation finished
                        OnTransitionIn();
                    }
                }
                else if (state.IsName(animateOutTrigger) && animationState)
                {
                    if (state.normalizedTime >= state.length)
                    {
                        // Flag as closed
                        animationState = false;

                        // On animation finished
                        OnTransitionOut();
                    }
                }
            }
        }

        public void Activate()
        {
            // Make sure the scene is active and enabled
            if (!isActiveAndEnabled || !gameObject.activeInHierarchy)
                return;

            // If it's prefab
            if (sceneType == SceneType.Prefab || sceneType == SceneType.Resource)
            {
                GameObject p = null;

                if (sceneType == SceneType.Prefab)
                {
                    // Check the prefab
                    if (p == null)
                    {
                        Debug.LogWarning("You are activating a prefab scene and no prefab is specified.");
                        return;
                    }

                    p = prefab;
                }

                if (sceneType == SceneType.Resource)
                {
                    // Try loading the resource
                    if (string.IsNullOrEmpty(resource))
                    {
                        Debug.LogWarning("You are activating a resource scene and no resource path is specified.");
                        return;
                    }

                    p = Resources.Load<GameObject>(resource);
                }

                // Instantiate the prefab
                if (p != null)
                {
                    // Instantiate the prefab
                    GameObject obj = Instantiate(p);

                    // Set the content variable
                    content = obj.transform;

                    // Set parent
                    content.SetParent(transform);

                    // Check if it's a rect transform
                    if (content is RectTransform)
                    {
                        // Get the rect transform
                        RectTransform rectTransform = content as RectTransform;

                        // Prepare the rect
                        rectTransform.localScale = Vector3.one;
                        rectTransform.localPosition = Vector3.zero;

                        // Set anchor and pivot
                        rectTransform.anchorMin = new Vector2(0f, 0f);
                        rectTransform.anchorMax = new Vector2(1f, 1f);
                        rectTransform.pivot = new Vector2(0.5f, 0.5f);

                        // Get the canvas size
                        Canvas canvas = UIUtility.FindInParents<Canvas>(gameObject);

                        if (canvas == null)
                        {
                            canvas = GetComponentInChildren<Canvas>();
                        }

                        if (canvas != null)
                        {
                            RectTransform crt = canvas.transform as RectTransform;

                            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, crt.sizeDelta.x);
                            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, crt.sizeDelta.y);
                        }

                        // Set position
                        rectTransform.anchoredPosition3D = Vector3.zero;
                    }
                }
            }

            // Enable the game object
            if (content != null)
            {
                content.gameObject.SetActive(true);
            }

            // Set the first selected for the navigation
            if (isActiveAndEnabled && firstSelected != null)
            {
                // 이거 잘 생각해야함... player Input이랑 겹치려나..?
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }

            // Set the active variable
            isActivated = true;

            // Invoke the event
            if (onActivate != null)
            {
                onActivate.Invoke(this);
            }
        }

        public void Deactivate()
        {
            // Disable the game object
            if (content != null)
            {
                content.gameObject.SetActive(false);
            }

            // If prefab destroy the object
            if (sceneType == SceneType.Prefab || sceneType == SceneType.Resource)
            {
                Destroy(content.gameObject);
            }

            // Unload unused assets
            Resources.UnloadUnusedAssets();

            // Set the active variable
            isActivated = false;

            // Invoke the event
            if (onDeactivate != null)
            {
                onDeactivate.Invoke(this);
            }
        }

        public void TransitionTo()
        {
            // Make sure the scene is active and enabled
            if (!isActiveAndEnabled || !gameObject.activeInHierarchy)
                return;

            if (sceneManager != null)
            {
                sceneManager.TransitionToScene(this);
            }
        }

        public void TransitionIn()
        {
            // TransitionIn(transition, transitionDuration, m_TransitionEasing);
        }

        public void TransitionIn(Transition transition, float duration/*, TweenEasing easing*/)
        {
            // Make sure the scene is active and enabled
            if (!isActiveAndEnabled || !gameObject.activeInHierarchy)
                return;

            if (canvasGroup == null)
                return;

            // If no transition is used
            if (transition == Transition.None)
            {
                Activate();
                return;
            }

            // If the transition is animation
            if (transition == Transition.Animation)
            {
                Activate();
                TriggerAnimation(animateInTrigger);
                return;
            }

            // Prepare some variable
            Vector2 rectSize = rectTransform.rect.size;

            // Prepare the rect transform
            if (transition == Transition.SlideFromLeft || transition == Transition.SlideFromRight || transition == Transition.SlideFromTop || transition == Transition.SlideFromBottom)
            {
                // Anchor and pivot top left
                rectTransform.pivot = new Vector2(0f, 1f);
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.sizeDelta = rectSize;
            }

            // Prepare the tween
            
            /*FloatTween floatTween = new FloatTween();
            floatTween.duration = duration;

            switch (transition)
            {
                case Transition.CrossFade:
                    canvasGroup.alpha = 0f;
                    floatTween.startFloat = 0f;
                    floatTween.targetFloat = 1f;
                    floatTween.AddOnChangedCallback(SetCanvasAlpha);
                    break;
                case Transition.SlideFromRight:
                    rectTransform.anchoredPosition = new Vector2(rectSize.x, 0f);
                    floatTween.startFloat = rectSize.x;
                    floatTween.targetFloat = 0f;
                    floatTween.AddOnChangedCallback(SetPositionX);
                    break;
                case Transition.SlideFromLeft:
                    rectTransform.anchoredPosition = new Vector2((rectSize.x * -1f), 0f);
                    floatTween.startFloat = (rectSize.x * -1f);
                    floatTween.targetFloat = 0f;
                    floatTween.AddOnChangedCallback(SetPositionX);
                    break;
                case Transition.SlideFromBottom:
                    rectTransform.anchoredPosition = new Vector2(0f, (rectSize.y * -1f));
                    floatTween.startFloat = (rectSize.y * -1f);
                    floatTween.targetFloat = 0f;
                    floatTween.AddOnChangedCallback(SetPositionY);
                    break;
                case Transition.SlideFromTop:
                    rectTransform.anchoredPosition = new Vector2(0f, rectSize.y);
                    floatTween.startFloat = rectSize.y;
                    floatTween.targetFloat = 0f;
                    floatTween.AddOnChangedCallback(SetPositionY);
                    break;
            }*/

            // Activate the scene
            Activate();

            // Start the transition

            //floatTween.AddOnFinishCallback(OnTransitionIn);
            //floatTween.ignoreTimeScale = true;
            //floatTween.easing = easing;
            //floatTweenRunner.StartTween(floatTween);
        }

        public void TransitionOut()
        {
            // TransitionOut(transition, transitionDuration, m_TransitionEasing);
        }

        public void TransitionOut(Transition transition, float duration/*, TweenEasing easing*/)
        {
            // Make sure the scene is active and enabled
            if (!isActiveAndEnabled || !gameObject.activeInHierarchy)
                return;

            if (canvasGroup == null)
                return;

            // If no transition is used
            if (transition == Transition.None)
            {
                Deactivate();
                return;
            }

            // If the transition is animation
            if (transition == Transition.Animation)
            {
                TriggerAnimation(animateOutTrigger);
                return;
            }

            // Prepare some variable
            Vector2 rectSize = rectTransform.rect.size;

            // Prepare the rect transform
            if (transition == Transition.SlideFromLeft || transition == Transition.SlideFromRight || transition == Transition.SlideFromTop || transition == Transition.SlideFromBottom)
            {
                // Anchor and pivot top left
                rectTransform.pivot = new Vector2(0f, 1f);
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.sizeDelta = rectSize;
                rectTransform.anchoredPosition = new Vector2(0f, 0f);
            }

            // Prepare the tween
            /*
            FloatTween floatTween = new FloatTween();
            floatTween.duration = duration;

            switch (transition)
            {
                case Transition.CrossFade:
                    canvasGroup.alpha = 1f;
                    // Start the tween
                    floatTween.startFloat = canvasGroup.alpha;
                    floatTween.targetFloat = 0f;
                    floatTween.AddOnChangedCallback(SetCanvasAlpha);
                    break;
                case Transition.SlideFromRight:
                    // Start the tween
                    floatTween.startFloat = 0f;
                    floatTween.targetFloat = (rectSize.x * -1f);
                    floatTween.AddOnChangedCallback(SetPositionX);
                    break;
                case Transition.SlideFromLeft:
                    // Start the tween
                    floatTween.startFloat = 0f;
                    floatTween.targetFloat = rectSize.x;
                    floatTween.AddOnChangedCallback(SetPositionX);
                    break;
                case Transition.SlideFromBottom:
                    // Start the tween
                    floatTween.startFloat = 0f;
                    floatTween.targetFloat = rectSize.y;
                    floatTween.AddOnChangedCallback(SetPositionY);
                    break;
                case Transition.SlideFromTop:
                    // Start the tween
                    floatTween.startFloat = 0f;
                    floatTween.targetFloat = (rectSize.y * -1f);
                    floatTween.AddOnChangedCallback(SetPositionY);
                    break;
            }

            // Start the transition
            floatTween.AddOnFinishCallback(OnTransitionOut);
            floatTween.ignoreTimeScale = true;
            floatTween.easing = easing;
            floatTweenRunner.StartTween(floatTween);
            */
        }

        public void StartAlphaTween(float targetAlpha, float duration/*, TweenEasing easing*/, bool ignoreTimeScale, UnityAction callback)
        {
            if (canvasGroup == null)
                return;

            // Start the tween
            //var floatTween = new FloatTween { duration = duration, startFloat = canvasGroup.alpha, targetFloat = targetAlpha };
            //floatTween.AddOnChangedCallback(SetCanvasAlpha);
            //floatTween.AddOnFinishCallback(callback);
            //floatTween.ignoreTimeScale = ignoreTimeScale;
            //floatTween.easing = easing;
            //floatTweenRunner.StartTween(floatTween);
        }

		public void SetCanvasAlpha(float alpha)
        {
            if (canvasGroup == null)
                return;

            // Set the alpha
            canvasGroup.alpha = alpha;
        }

        public void SetPositionX(float x)
        {
            rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
        }

        public void SetPositionY(float y)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
        }

        private void TriggerAnimation(string triggername)
        {
            // Get the animator on the target game object
            Animator animator = gameObject.GetComponent<Animator>();

            if (animator == null || !animator.enabled || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
                return;

            animator.ResetTrigger(animateInTrigger);
            animator.ResetTrigger(animateOutTrigger);
            animator.SetTrigger(triggername);
        }

		protected virtual void OnTransitionIn()
        {
            // Re-enable the canvas group interaction
            if (canvasGroup != null)
            {
                //m_CanvasGroup.interactable = true;
                //m_CanvasGroup.blocksRaycasts = true;
            }
        }

		protected virtual void OnTransitionOut()
        {
            // Deactivate the scene
            Deactivate();

            // Re-enable the canvas group interaction
            if (canvasGroup != null)
            {
                //m_CanvasGroup.interactable = true;
                //m_CanvasGroup.blocksRaycasts = true;
            }

            // Reset the alpha
            SetCanvasAlpha(1f);

            // Reset the position of the transform
            SetPositionX(0f);
        }
    }
}
