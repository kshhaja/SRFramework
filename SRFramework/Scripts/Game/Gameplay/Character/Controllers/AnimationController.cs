using SlimeRPG.Event;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace SlimeRPG.Gameplay.Character.Controller
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : AbstractCharacterController
    {
        #region Variables      
        public Animator animator;

        public bool disableAnimations;

        public AnimatorStateInfos animatorStateInfos { get; protected set; }
        private float randomIdleCount;
        private float _direction = 0;
        public const float walkSpeed = 0.5f;
        public const float runningSpeed = 1f;
        public const float sprintSpeed = 1.5f;
        private bool triggerDieBehaviour;

        protected Vector3 lastCharacterAngle;

        protected internal bool lockAnimMovement;           // internaly used with the vAnimatorTag("LockMovement"), use on the animator to lock the movement of a specific animation clip        
        protected internal bool lockAnimRotation;           // internaly used with the vAnimatorTag("LockRotation"), use on the animator to lock a rotation of a specific animation clip

        [Tooltip("While in Free Locomotion the character will lean to left/right when making turns")]
        public bool useLeanMovement = true;


        // w.i.p not ready yet
        [HideInInspector]
        [Tooltip("Check this to use the TurnOnSpot animations, you also need to check the option 'RotateWithCamera' in the strafe speed options")]
        public bool turnOnSpotAnim = false;


        [Tooltip("Put your Random Idle animations at the AnimatorController and select a value to randomize, 0 is disable.")]
        public float randomIdleTime = 0f;


        #endregion

        public enum DeathBy
        {
            Animation,
            AnimationWithRagdoll,
            Ragdoll
        }
        public DeathBy deathBy = DeathBy.Animation;


        #region Animator Layer
        internal AnimatorStateInfo baseLayerInfo, underBodyInfo, rightArmInfo, leftArmInfo, fullBodyInfo, upperBodyInfo;

        public int baseLayer { get { return animator.GetLayerIndex("Base Layer"); } }
        public int underBodyLayer { get { return animator.GetLayerIndex("UnderBody"); } }
        public int rightArmLayer { get { return animator.GetLayerIndex("RightArm"); } }
        public int leftArmLayer { get { return animator.GetLayerIndex("LeftArm"); } }
        public int upperBodyLayer { get { return animator.GetLayerIndex("UpperBody"); } }
        public int fullbodyLayer { get { return animator.GetLayerIndex("FullBody"); } }
        #endregion

        protected void Awake()
        {
            character = GetComponent<CharacterBase>();
            animator = GetComponent<Animator>();
        }

        protected void Start()
        {
            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

            animatorStateInfos = new AnimatorStateInfos(animator);
            animatorStateInfos.RegisterListener();
        }

        public IEnumerator PlayAnimationClip(AnimationClip clip)
        {
            // 목표는 mecanim제거하고 Playable로...
            AnimationPlayableUtilities.PlayClip(animator, clip, out var playableGraph);
            
            while (playableGraph.IsDone() == false)
                yield return null;

            yield break;
        }

        public IEnumerator PlayAttackAnimation()
        {
            animator.SetTrigger("WeakAttack");
            yield return null;
            var state = animator.GetCurrentAnimatorStateInfo(fullbodyLayer);
            Debug.Log(state.IsName("Attacks"));
            //yield return new WaitUntil(() => state.IsName("Attacks"));

            yield return null;
        }

        protected virtual void OnEnable()
        {
            if (animatorStateInfos != null)
                animatorStateInfos.RegisterListener();
        }

        protected virtual void OnDisable()
        {
            if (animatorStateInfos != null)
                animatorStateInfos.RemoveListener();
        }

        protected virtual void FixedUpdate()
        {
            UpdateAnimator();
        }

        public virtual void UpdateAnimator()
        {
            if (animator == null || !animator.enabled) return;

            AnimatorLayerControl();
            // ActionsControl();

            // TriggerRandomIdle();

            UpdateAnimatorParameters();
            // DeadAnimation();
        }

        public virtual void AnimatorLayerControl()
        {
            baseLayerInfo = animator.GetCurrentAnimatorStateInfo(baseLayer);
            underBodyInfo = animator.GetCurrentAnimatorStateInfo(underBodyLayer);
            rightArmInfo = animator.GetCurrentAnimatorStateInfo(rightArmLayer);
            leftArmInfo = animator.GetCurrentAnimatorStateInfo(leftArmLayer);
            upperBodyInfo = animator.GetCurrentAnimatorStateInfo(upperBodyLayer);
            fullBodyInfo = animator.GetCurrentAnimatorStateInfo(fullbodyLayer);
        }

        public virtual void ActionsControl()
        {
            // to have better control of your actions, you can assign bools to know if an animation is playing or not
            // this way you can use this bool to create custom behavior for the controller

            // identify if the rolling animations is playing
            character.isRolling = IsAnimatorTag("IsRolling");
            // identify if a turn on spot animation is playing
            character.isTurningOnSpot = IsAnimatorTag("TurnOnSpot");
            // locks player movement while a animation with tag 'LockMovement' is playing
            lockAnimMovement = IsAnimatorTag("LockMovement");
            // locks player rotation while a animation with tag 'LockRotation' is playing
            lockAnimRotation = IsAnimatorTag("LockRotation");
            
            
            // 이건 뭐지?
            // ! -- you can add the Tag "CustomAction" into a AnimatonState and the character will not perform any Melee action -- !            
            //character.customAction = IsAnimatorTag("CustomAction");
            
            
            // identify if the controller is airborne
            character.isInAirborne = IsAnimatorTag("Airborne");
        }

        public virtual void UpdateAnimatorParameters()
        {
            if (disableAnimations) 
                return;

            // 이런 상태를 원하는 것들은 전부 action쪽과 연동될듯.
            // 혹은 stateMachine을 도입할수도

            var movement = character.movementController as PlayerMovementController;
            animator.SetBool(AnimatorParameters.IsStrafing, character.isStrafing);
            animator.SetBool(AnimatorParameters.IsSprinting, character.isSprinting);
            animator.SetBool(AnimatorParameters.IsSliding, character.isSliding);
            animator.SetBool(AnimatorParameters.IsCrouching, character.isCrouching);
            animator.SetBool(AnimatorParameters.IsGrounded, movement.isGrounded);
            animator.SetBool(AnimatorParameters.IsDead, character.isDead);
            // animator.SetFloat(AnimatorParameters.GroundDistance, groundDistance);
            // animator.SetFloat(AnimatorParameters.GroundAngle, GroundAngleFromDirection());

            if (!movement.isGrounded)
                 animator.SetFloat(AnimatorParameters.VerticalVelocity, movement.verticalVelocity);

            if (!lockAnimMovement)
            {
                if (character.isStrafing)
                {
                    animator.SetFloat(AnimatorParameters.InputHorizontal, !movement.stopMove ? movement.horizontalSpeed : 0f, movement.strafeSpeed.animationSmooth, Time.deltaTime);
                    animator.SetFloat(AnimatorParameters.InputVertical, !movement.stopMove ? movement.verticalSpeed : 0f, movement.strafeSpeed.animationSmooth, Time.deltaTime);
                }
                else
                {

                    animator.SetFloat(AnimatorParameters.InputVertical, movement.stopMove ? 0 : movement.verticalSpeed, movement.freeSpeed.animationSmooth, Time.deltaTime);
                    animator.SetFloat(AnimatorParameters.InputHorizontal, movement.stopMove ? 0 : LeanMovement(), movement.freeSpeed.animationSmooth, Time.deltaTime);
                }

                animator.SetFloat(AnimatorParameters.InputMagnitude, movement.stopMove ? 0f : movement.inputMagnitude, movement.isStrafing ? movement.strafeSpeed.animationSmooth : movement.freeSpeed.animationSmooth, Time.deltaTime);
            }

            if (turnOnSpotAnim)
            {
                // GetTurnOnSpotDirection(transform, rotateTarget, ref _speed, ref _direction, input);
                FreeTurnOnSpot(_direction * 180);
                StrafeTurnOnSpot();
            }
            lastCharacterAngle = transform.eulerAngles;
        }

        protected virtual float LeanMovement()
        {
            if (!useLeanMovement)
            {
                return 0;
            }

            var leanEuler = transform.eulerAngles - lastCharacterAngle;
            var movement = character.movementController as PlayerMovementController;
            float angleY = leanEuler.NormalizeAngle().y / (character.isStrafing ? movement.strafeSpeed.rotationSpeed : movement.freeSpeed.rotationSpeed);

            return angleY;
        }

        public virtual void SetAnimatorMoveSpeed(MovementSpeed speed)
        {
            var movement = character.movementController as PlayerMovementController;
            Vector3 relativeInput = transform.InverseTransformDirection(movement.moveDirection);
            movement.verticalSpeed = relativeInput.z;
            movement.horizontalSpeed = relativeInput.x;

            var newInput = new Vector2(movement.verticalSpeed, movement.horizontalSpeed);

            if (speed.walkByDefault)
                movement.inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, movement.isSprinting ? runningSpeed : walkSpeed);
            else
                movement.inputMagnitude = Mathf.Clamp(movement.isSprinting ? newInput.magnitude + 0.5f : newInput.magnitude, 0, movement.isSprinting ? sprintSpeed : runningSpeed);
        }

        public virtual void ResetInputAnimatorParameters()
        {
            animator.SetFloat("InputHorizontal", 0f, 0f, Time.deltaTime);
            animator.SetFloat("InputVertical", 0f, 0f, Time.deltaTime);
            animator.SetFloat("InputMagnitude", 0f, 0f, Time.deltaTime);
            animator.SetBool(AnimatorParameters.IsSprinting, false);
            animator.SetBool(AnimatorParameters.IsSliding, false);
            animator.SetBool(AnimatorParameters.IsCrouching, false);
            animator.SetBool(AnimatorParameters.IsGrounded, true);
            animator.SetFloat(AnimatorParameters.GroundDistance, 0f);
        }

        protected virtual void TriggerRandomIdle()
        {
            // if (input != Vector3.zero || customAction) 
            //    return;

            if (randomIdleTime > 0)
            {
                if (/*input.sqrMagnitude == 0 && */!character.isCrouching /*&& _capsuleCollider.enabled && isGrounded*/)
                {
                    randomIdleCount += Time.fixedDeltaTime;
                    if (randomIdleCount > 6)
                    {
                        randomIdleCount = 0;
                        animator.SetTrigger(AnimatorParameters.IdleRandomTrigger);
                        animator.SetInteger(AnimatorParameters.IdleRandom, Random.Range(1, 4));
                    }
                }
                else
                {
                    randomIdleCount = 0;
                    animator.SetInteger(AnimatorParameters.IdleRandom, 0);
                }
            }
        }

        public virtual void DeathBehaviour()
        {
            // lock the player input
            lockAnimMovement = true;
            // change the culling mode to render the animation until finish
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            // trigger die animation            
            // if (deathBy == DeathBy.Animation || deathBy == DeathBy.AnimationWithRagdoll)
            {
                // animator.SetBool("isDead", true);
            }
        }

        protected virtual void DeadAnimation()
        {
            // if (!isDead) return;

            if (!triggerDieBehaviour)
            {
                triggerDieBehaviour = true;
                // DeathBehaviour();
            }

            // death by animation
            if (deathBy == DeathBy.Animation)
            {
                int deadLayer = 0;
                if (animatorStateInfos.HasAnimatorLayerUsingTag("Dead", out deadLayer))
                {
                    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(deadLayer);
                    if (!animator.IsInTransition(deadLayer) && info.normalizedTime >= 0.99f /*&& groundDistance <= 0.15f*/)
                    {
                        // RemoveComponents();
                    }

                }
            }
            // death by animation & ragdoll after a time
            else if (deathBy == DeathBy.AnimationWithRagdoll)
            {
                int deadLayer = 0;
                if (animatorStateInfos.HasAnimatorLayerUsingTag("Dead", out deadLayer))
                {
                    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(deadLayer);
                    if (!animator.IsInTransition(deadLayer) && info.normalizedTime >= 0.8f)
                    {
                        //onActiveRagdoll.Invoke(null);
                    }
                }
            }
            // death by ragdoll
            else if (deathBy == DeathBy.Ragdoll)
            {
                // onActiveRagdoll.Invoke(null);
            }
        }

        #region TurnOnSpot

        public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
        {
            return Mathf.Atan2(
                Vector3.Dot(n, Vector3.Cross(v1, v2)),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }

        protected virtual void StrafeTurnOnSpot()
        {
            if (!character.isStrafing /*|| input.sqrMagnitude >= 0.25f*/ || character.isTurningOnSpot /*|| customAction || !strafeSpeed.rotateWithCamera*/ || character.isRolling)
            {
                animator.SetFloat(AnimatorParameters.TurnOnSpotDirection, 0);
                return;
            }

            var localFwd = transform.InverseTransformDirection(character.rotateTarget.forward);
            var angle = System.Math.Round(localFwd.x, 1);

            if (angle >= 0.01f && !character.isTurningOnSpot)
                animator.SetFloat(AnimatorParameters.TurnOnSpotDirection, 10);
            else if (angle <= -0.01f && !character.isTurningOnSpot)
                animator.SetFloat(AnimatorParameters.TurnOnSpotDirection, -10);
            else
                animator.SetFloat(AnimatorParameters.TurnOnSpotDirection, 0);
        }

        protected virtual void FreeTurnOnSpot(float direction)
        {
            if (character.isStrafing /*|| !freeSpeed.rotateWithCamera*/ || character.isRolling) 
                return;

            bool inTransition = animator.IsInTransition(0);
            float directionDampTime = character.isTurningOnSpot || inTransition ? 1000000 : 0;
            animator.SetFloat(AnimatorParameters.TurnOnSpotDirection, direction, directionDampTime, Time.deltaTime);
        }

        protected virtual void GetTurnOnSpotDirection(Transform root, Transform camera, ref float _speed, ref float _direction, Vector2 input)
        {
            Vector3 rootDirection = root.forward;
            Vector3 stickDirection = new Vector3(input.x, 0, input.y);

            // Get camera rotation.    
            Vector3 CameraDirection = camera.forward;
            CameraDirection.y = 0.0f; // kill Y
            Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

            // Convert joystick input in Worldspace coordinates            
            // Vector3 moveDirection = rotateByWorld ? stickDirection : referentialShift * stickDirection;
            Vector3 moveDirection = referentialShift * stickDirection;

            Vector2 speedVec = new Vector2(input.x, input.y);
            _speed = Mathf.Clamp(speedVec.magnitude, 0, 1);

            if (_speed > 0.01f) // dead zone
            {
                Vector3 axis = Vector3.Cross(rootDirection, moveDirection);
                _direction = Vector3.Angle(rootDirection, moveDirection) / 180.0f * (axis.y < 0 ? -1 : 1);
            }
            else
            {
                _direction = 0.0f;
            }
        }

        #endregion

        #region Generic Animations Methods

        public virtual void SetActionState(int value)
        {
            animator.SetInteger(AnimatorParameters.ActionState, value);
        }

        public virtual bool IsAnimatorTag(string tag)
        {
            if (animator == null) return false;
            if (animatorStateInfos != null)
            {
                if (animatorStateInfos.HasTag(tag))
                {
                    return true;
                }
            }
            if (baseLayerInfo.IsTag(tag)) return true;
            if (underBodyInfo.IsTag(tag)) return true;
            if (rightArmInfo.IsTag(tag)) return true;
            if (leftArmInfo.IsTag(tag)) return true;
            if (upperBodyInfo.IsTag(tag)) return true;
            if (fullBodyInfo.IsTag(tag)) return true;
            return false;
        }

        public virtual void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget target, MatchTargetWeightMask weightMask, float normalisedStartTime, float normalisedEndTime)
        {
            if (animator.isMatchingTarget || animator.IsInTransition(0))
                return;

            float normalizeTime = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);

            if (normalizeTime > normalisedEndTime)
                return;

            animator.MatchTarget(matchPosition, matchRotation, target, weightMask, normalisedStartTime, normalisedEndTime);
        }

        #endregion

    }

    public static partial class AnimatorParameters
    {
        public static int InputHorizontal = Animator.StringToHash("inputHorizontal");
        public static int InputVertical = Animator.StringToHash("inputVertical");
        public static int InputMagnitude = Animator.StringToHash("inputMagnitude");

        public static int TurnOnSpotDirection = Animator.StringToHash("TurnOnSpotDirection");
        public static int ActionState = Animator.StringToHash("ActionState");
        public static int ResetState = Animator.StringToHash("ResetState");

        public static int IsDead = Animator.StringToHash("isDead");
        public static int IsGrounded = Animator.StringToHash("isGrounded");
        public static int IsCrouching = Animator.StringToHash("isCrouching");
        public static int IsStrafing = Animator.StringToHash("isStrafing");
        public static int IsSprinting = Animator.StringToHash("isSprinting");
        public static int IsSliding = Animator.StringToHash("isSliding");
        
        public static int GroundDistance = Animator.StringToHash("GroundDistance");
        public static int GroundAngle = Animator.StringToHash("GroundAngle");
        public static int VerticalVelocity = Animator.StringToHash("VerticalVelocity");

        public static int IdleRandom = Animator.StringToHash("idleRandomID");
        public static int IdleRandomTrigger = Animator.StringToHash("idleRandomTrigger");
    }
}

