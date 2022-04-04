using SlimeRPG.Character;
using UnityEngine;


namespace SlimeRPG.Gameplay.Character.Controller
{
	[RequireComponent(typeof(PlayerCharacter))]
    public class PlayerMovementController : MovementController
    {
		[Tooltip("Layers that the character can walk on")]
		public LayerMask groundLayer = 1 << 0;
		[Tooltip("What objects can make the character auto crouch")]
		public LayerMask autoCrouchLayer = 1 << 0;
		[Tooltip("[SPHERECAST] ADJUST IN PLAY MODE - White Spherecast put just above the head, this will make the character Auto-Crouch if something hit the sphere.")]
		public float headDetect = 0.95f;
		[Tooltip("Select the layers the your character will stop moving when close to")]
		public LayerMask stopMoveLayer;
		[Tooltip("[RAYCAST] Stopmove Raycast Height")]
		public float stopMoveHeight = 0.65f;
		[Tooltip("[RAYCAST] Stopmove Raycast Distance")]
		public float stopMoveDistance = 0.1f;

		public bool useRootMotion = false;

		public enum LocomotionType
		{
			FreeWithStrafe,
			OnlyStrafe,
			OnlyFree,
		}
		public LocomotionType locomotionType = LocomotionType.FreeWithStrafe;

		public MovementSpeed freeSpeed, strafeSpeed;
		[Tooltip("Use this to rotate the character using the World axis, or false to use the camera axis - CHECK for Isometric Camera")]
		public bool rotateByWorld = false;

		
		[Tooltip("Check This to use sprint on press button to your Character run until the stamina finish or movement stops\nIf uncheck your Character will sprint as long as the SprintInput is pressed or the stamina finishes")]
		public bool useContinuousSprint = true;
		[Tooltip("Check this to sprint always in free movement")]
		public bool sprintOnlyFree = true;
		[Range(1, 2.5f)]
		public float crouchHeightReduction = 1.5f;


		public bool IsStrafing
		{
			get
			{
				return sprintOnlyFree && isSprinting ? false : isStrafing;
			}
			set
			{
				isStrafing = value;
			}
		}

		// movement bools
		
		public virtual bool IsCrouching
		{
			get
			{
				return isCrouching;
			}
			set
			{
				if (value != isCrouching)
				{
					//if (value)
					//	OnCrouch.Invoke();
					//else
					//	OnStandUp.Invoke();
				}
				isCrouching = value;
			}
		}
		// action bools
		
		internal bool customAction;

		#region Hide vars
		public bool ragdolled { get; set; }

		// internal Animator animator;
		internal Rigidbody rb;
		internal CapsuleCollider col;
		internal PhysicMaterial frictionPhysics, maxFrictionPhysics, slippyPhysics;         // create PhysicMaterial for the Rigidbody
		public PhysicMaterial currentMaterialPhysics { get; protected set; }

		private bool isCrouching;

		internal bool isStrafing;                          // internally used to set the strafe movement
        internal float jumpMultiplier = 1;                  // internally used to set the jumpMultiplier
		internal float heightReached;                       // max height that character reached in air;

		// fall
		internal float verticalVelocity;                    // set the vertical velocity of the rigidbody

		// speed
		internal float verticalSpeed;                       // set the verticalSpeed based on the verticalInput
		internal float horizontalSpeed;                     // set the horizontalSpeed based on the horizontalInput       
		public const float walkSpeed = 0.5f;
		public const float runningSpeed = 1f;
		public const float sprintSpeed = 1.5f;

		internal float inputMagnitude;                      // sets the inputMagnitude to update the animations in the animator controller
		
		// rotation 에서 쓰는건데 필요없을듯.
		internal bool keepDirection;                        // keeps the character direction even if the camera direction changes

		internal Vector3 colliderCenter;                     // storage the center of the capsule collider info                
		internal float colliderRadius, colliderHeight;      // storage capsule collider extra information                       

		internal bool blockApplyFallDamage = false;

		
		[HideInInspector] public bool applyingStepOffset;   // internally used to apply the StepOffset       

		internal bool lockMovement = false;                 // lock the movement of the controller (not the animation)
		internal bool lockRotation = false;                 // lock the rotation of the controller (not the animation)
		internal bool lockSetMoveSpeed = false;             // locks the method to update the moveset based on the locomotion type, so you can modify externally
		internal bool lockInStrafe;                         // locks the controller to only used the strafe locomotion type        


		[HideInInspector] public Vector3 inputSmooth;        // generate smooth input based on the inputSmooth value       
		internal Vector3 input;                              // generate raw input for the controller
		internal Vector3 oldInput;                           // used internally to identify oldinput from the current input
		

		internal float groundDistance;
		public RaycastHit groundHit;


		#endregion

		// 아마도 detection
		public enum GroundCheckMethod
		{
			Low, High
		}
		
		[Header("Ground")]
		[Tooltip("Ground Check Method To check ground Distance and ground angle\n*Simple: Use just a single Raycast\n*Normal: Use Raycast and SphereCast\n*Complex: Use SphereCastAll")]
		public GroundCheckMethod groundCheckMethod = GroundCheckMethod.High;
		[Tooltip("The length of the Ray cast to detect ground ")]
		public float groundDetectionDistance = 10f;
		[Tooltip("Snaps the capsule collider to the ground surface, recommend when using complex terrains or inclined ramps")]
		public bool useSnapGround = true;
		[Range(0, 1)]
		public float snapPower = 0.5f;
		[Tooltip("Distance to became not grounded")]
		[Range(0, 10)]
		public float groundMinDistance = 0.1f;
		[Range(0, 10)]
		public float groundMaxDistance = 0.5f;
		[Tooltip("Max angle to walk")]
		[Range(30, 80)]
		public float slopeLimit = 75f;

		[Header("Slide Slopes")]
        public bool useSlide = true;
        [Tooltip("Velocity to slide down when on a slope limit ramp")]
        [Range(0, 30)]
        public float slideDownVelocity = 7f;
        [Tooltip("Velocity to slide sideways when on a slope limit ramp")]
        [Range(0, 15)]
        public float slideSidewaysVelocity = 5f;
        [Range(0.1f, 1f)]
        [Tooltip("Delay to start sliding once the character is standing on a slope")]
        public float slidingEnterTime = 0.1f;
        internal float internalSlidingEnterTime;


		[Header("Step Offset")]
		public bool useStepOffset = true;
		[Tooltip("Offset max height to walk on steps - YELLOW Raycast in front of the legs")]
		[Range(0, 1)]
		public float stepOffsetMaxHeight = 0.5f;
		[Tooltip("Offset min height to walk on steps. Make sure to keep slight above the floor - YELLOW Raycast in front of the legs")]
		[Range(0, 1)]
		public float stepOffsetMinHeight = 0f;
		[Tooltip("Offset distance to walk on steps - YELLOW Raycast in front of the legs")]
		[Range(0, 1)]
		public float stepOffsetDistance = 0.1f;

		public RaycastHit stepOffsetHit;

		[Header("Falling")]

		[Tooltip("Speed that the character will move while airborne")]
		public float airSpeed = 5f;
		[Tooltip("Smoothness of the direction while airborne")]
		public float airSmooth = 6f;
		[Tooltip("Apply extra gravity when the character is not grounded")]
		public float extraGravity = -10f;
		[Tooltip("Limit of the vertival velocity when Falling")]
		public float limitFallVelocity = -15f;
		[Tooltip("Turn the Ragdoll On when falling at high speed (check VerticalVelocity) - leave the value with 0 if you don't want this feature")]
		public float ragdollVelocity = -15f;
		[Header("Fall Damage")]
		public float fallMinHeight = 6f;
		public float fallMinVerticalVelocity = -10f;

		// 이런 고정적인 데미지 종류는 글로벌 statsContainer에서 가져오도록 만들자.
		public float fallDamage = 10f;


		// 여기 필요없어야 될듯.
		[Header("Jump")]
		[Tooltip("Use the currently Rigidbody Velocity to influence on the Jump Distance")]
		public bool jumpWithRigidbodyForce = false;
		[Tooltip("Rotate or not while airborne")]
		public bool jumpAndRotate = true;
		[Tooltip("How much time the character will be jumping")]
		public float jumpTimer = 0.3f;
		internal float jumpCounter;
		[Tooltip("Add Extra jump height, if you want to jump only with Root Motion leave the value with 0.")]
		public float jumpHeight = 4f;


		// 이것도 결국엔 액션으로...
		public bool useRollRootMotion = true;
		[Tooltip("Animation Transition from current animation to Roll")]
		public float rollTransition = .25f;
		[Tooltip("Can control the Roll Direction")]
		public bool rollControl = true;
		[Tooltip("Speed of the Roll Movement")]
		public float rollSpeed = 0f;
		[Tooltip("Speed of the Roll Rotation")]
		public float rollRotationSpeed = 20f;
		public bool rollUseGravity = true;
		[Tooltip("Normalized Time of the roll animation to enable gravity influence")]
		public float rollUseGravityTime = 0.2f;
		[Tooltip("Use the normalized time of the animation to know when you can roll again")]
		[Range(0, 1)]
		public float timeToRollAgain = 0.75f;
		[Tooltip("Ignore all damage while is rolling, include Damage that ignore defence")]
		public bool noDamageWhileRolling = true;
		[Tooltip("Ignore damage that needs to activate ragdoll")]
		public bool noActiveRagdollWhileRolling = true;

		[Header("--- Debug Info ---")]
		public bool debugWindow;


		
        protected virtual bool canApplyFallDamage { get { return !blockApplyFallDamage && jumpMultiplier <= 1 && !customAction; } }

		public Vector3 colliderCenterDefault
		{
			get; protected set;
		}
		public float colliderRadiusDefault
		{
			get; protected set;
		}
		/// <summary>
		/// Default Height of the Character Capsule
		/// </summary>
		public float colliderHeightDefault
		{
			get; protected set;
		}

		protected void Start()
		{
			heightReached = transform.position.y;

			character = GetComponent<CharacterBase>();
			
			// slides the character through walls and edges
			frictionPhysics = new PhysicMaterial();
			frictionPhysics.name = "frictionPhysics";
			frictionPhysics.staticFriction = .25f;
			frictionPhysics.dynamicFriction = .25f;
			frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

			// prevents the collider from slipping on ramps
			maxFrictionPhysics = new PhysicMaterial();
			maxFrictionPhysics.name = "maxFrictionPhysics";
			maxFrictionPhysics.staticFriction = 1f;
			maxFrictionPhysics.dynamicFriction = 1f;
			maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

			// air physics 
			slippyPhysics = new PhysicMaterial();
			slippyPhysics.name = "slippyPhysics";
			slippyPhysics.staticFriction = 0f;
			slippyPhysics.dynamicFriction = 0f;
			slippyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;

			// rigidbody info
			rb = GetComponent<Rigidbody>();

			// capsule collider info
			col = GetComponent<CapsuleCollider>();

			// save your collider preferences 
			colliderCenter = colliderCenterDefault = col.center;
			colliderRadius = colliderRadiusDefault = col.radius;
			colliderHeight = colliderHeightDefault = col.height;

			// avoid collision detection with inside colliders 
			Collider[] AllColliders = this.GetComponentsInChildren<Collider>();
			for (int i = 0; i < AllColliders.Length; i++)
			{
				Physics.IgnoreCollision(col, AllColliders[i]);
			}
			
			// ResetJumpMultiplier();
			isGrounded = true;
		}

        private void FixedUpdate()
        {
			Physics.SyncTransforms();

			// CheckHealth();
			// CheckStamina();
			CheckGround();
			CheckRagdoll();
			StopMove();
			ControlCapsuleHeight();
			   // ControlJumpBehaviour();
			AirControl();
			// StaminaRecovery();

			ControlLocomotionType();
			ControlRotation();
		}

		internal bool lockUpdateMoveDirection;

		public virtual void UpdateMoveDirection(Transform referenceTransform = null)
		{
			if (character.isRolling && !rollControl || input.magnitude <= 0.01)
			{
				moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
				return;
			}

			if (referenceTransform && !rotateByWorld)
			{
				//get the right-facing direction of the referenceTransform
				var right = referenceTransform.right;
				right.y = 0;
				//get the forward direction relative to referenceTransform Right
				var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
				// determine the direction the player will face based on input and the referenceTransform's right and forward directions
				moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
			}
			else
			{
				moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
			}
		}

		Transform lockTarget;
		
		public virtual void ControlKeepDirection()
		{
			// update oldInput to compare with current Input if keepDirection is true
			if (!keepDirection)
			{
				oldInput = input;
			}
			else if ((input.magnitude < 0.01f || Vector3.Distance(oldInput, input) > 0.9f) && keepDirection)
			{
				keepDirection = false;
			}
		}
		
		public virtual void ControlRotation()
		{
			var cam = Camera.main;
			if (cam && !lockUpdateMoveDirection)
			{
				if (!keepDirection)
				{
					UpdateMoveDirection(cam.transform);
				}
			}

			// 그.. 엘든링 락 시스템처럼 캐릭터가 계속 쳐다보게 하는 기술..?
			//if (tpCamera != null && tpCamera.lockTarget && isStrafing)
			if (lockTarget && isStrafing)
			{
				RotateToPosition(lockTarget.position);          // rotate the character to a specific target
			}
			else
			{
				ControlRotationType();                                   // handle the controller rotation type (strafe or free)
			}
		}

		public virtual Vector3 lookPosition
		{
			get; protected set;
		}
		public LayerMask mouseLayerMask = 1 << 0;

		internal Collider lookCollider;

		public virtual Vector3 WorldMousePosition(LayerMask castLayer, out Collider collider)
		{
			var mainCamera = Camera.current;
			if (!mainCamera)
			{
				if (!Camera.main)
				{
					Debug.LogWarning("Trying to get the world mouse position but a MainCamera is missing from the scene");
					collider = null;
					return Vector3.zero;
				}
				else
				{
					mainCamera = Camera.main;
					collider = null;
					return Vector3.zero;
				}
			}
			else
			{
				Ray ray = mainCamera.ScreenPointToRay(aimPoint);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit, mainCamera.farClipPlane, castLayer))
				{
					collider = hit.collider;
					return hit.point;
				}
				else
				{
					collider = null;
					return ray.GetPoint(mainCamera.farClipPlane);
				}
			}
		}

		public virtual void ControlRotationType()
		{
			///Rotate character to Mouse world position
			if (isStrafing)
			{
				lookPosition = WorldMousePosition(mouseLayerMask, out lookCollider);
				Vector3 mouseDirection = (lookPosition - transform.position).normalized;
				Debug.DrawRay(transform.position + Vector3.up, mouseDirection);
				mouseDirection.y = 0;
				Vector3 desiredForward = Vector3.RotateTowards(transform.forward, mouseDirection, strafeSpeed.rotationSpeed * Time.fixedDeltaTime, 0f);
				RotateToDirection(desiredForward);

				return;
			}
			lookPosition = transform.position + transform.forward * 100f;

			if ( /*lockAnimRotation ||*/ lockRotation || customAction || character.isRolling)
			{
				return;
			}

			bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

			if (validInput)
			{
				//if (lockAnimMovement)
				//{
				//	// calculate input smooth
				//	inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
				//}

				Vector3 dir = (isStrafing && (!isSprinting || sprintOnlyFree == false) || (freeSpeed.rotateWithCamera && input == Vector3.zero)) && character.rotateTarget ? character.rotateTarget.forward : moveDirection;
				RotateToDirection(dir);
			}
			lookPosition = transform.position + transform.forward * 100f;
		}

		public virtual void ControlLocomotionType()
		{
			if (/*lockAnimMovement ||*/ lockMovement || customAction || character.isRolling)
			{
				return;
			}

			if (!lockSetMoveSpeed)
			{
				if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !IsStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
				{
					SetControllerMoveSpeed(freeSpeed);
					SetAnimatorMoveSpeed(freeSpeed);
				}
				else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && IsStrafing)
				{
					IsStrafing = true;
					SetControllerMoveSpeed(strafeSpeed);
					SetAnimatorMoveSpeed(strafeSpeed);
				}
			}

			if (!useRootMotion)
			{
				MoveCharacter(moveDirection);
			}
		}

		public virtual void SetAnimatorMoveSpeed(MovementSpeed speed)
		{
			Vector3 relativeInput = transform.InverseTransformDirection(moveDirection);
			verticalSpeed = relativeInput.z;
			horizontalSpeed = relativeInput.x;

			var newInput = new Vector2(verticalSpeed, horizontalSpeed);

			if (speed.walkByDefault)
				inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, isSprinting ? runningSpeed : walkSpeed);
			else
				inputMagnitude = Mathf.Clamp(isSprinting ? newInput.magnitude + 0.5f : newInput.magnitude, 0, isSprinting ? sprintSpeed : runningSpeed);
		}

		public virtual void SetControllerMoveSpeed(MovementSpeed speed)
		{
			if (isCrouching)
			{
				moveSpeed = Mathf.Lerp(moveSpeed, speed.crouchSpeed, speed.movementSmooth * Time.deltaTime);
				return;
			}

			if (speed.walkByDefault)
			{
				moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.runningSpeed : speed.walkSpeed, speed.movementSmooth * Time.deltaTime);
			}
			else
			{
				moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.sprintSpeed : speed.runningSpeed, speed.movementSmooth * Time.deltaTime);
			}
		}

		public virtual void MoveCharacter(Vector3 direction)
		{
			// calculate input smooth
			inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * (useRootMotion ? TimeExtensions.deltaTime : TimeExtensions.fixedDeltaTime));
			if (isSliding || ragdolled || !isGrounded || character.isJumping)
			{
				return;
			}

			direction.y = 0;
			direction.x = Mathf.Clamp(direction.x, -1f, 1f);
			direction.z = Mathf.Clamp(direction.z, -1f, 1f);
			// limit the input
			if (direction.magnitude > 1f)
			{
				direction.Normalize();
			}

			Vector3 targetPosition = (useRootMotion ? character.animationController.animator.rootPosition : rb.position) + direction * (stopMove ? 0 : moveSpeed) * (useRootMotion ? TimeExtensions.deltaTime : TimeExtensions.fixedDeltaTime);
			Vector3 targetVelocity = (targetPosition - transform.position) / (useRootMotion ? TimeExtensions.deltaTime : TimeExtensions.fixedDeltaTime);

			bool useVerticalVelocity = true;

			SnapToGround(ref targetVelocity, ref useVerticalVelocity);
			CalculateStepOffset(direction.normalized, ref targetVelocity, ref useVerticalVelocity);

			if (useVerticalVelocity)
			{
				targetVelocity.y = rb.velocity.y;
			}

			rb.velocity = targetVelocity;
		}

		protected virtual void SnapToGround(ref Vector3 targetVelocity, ref bool useVerticalVelocity)
		{
			if (!useSnapGround || !disableCheckGround)
			{
				return;
			}

			if (groundDistance < groundMinDistance * 0.2f || applyingStepOffset)
			{
				return;
			}

			var snapConditions = isGrounded && groundHit.collider != null && GroundAngle() <= slopeLimit && !disableCheckGround && !isSliding && !character.isJumping && !customAction && input.magnitude > 0.1f && !character.isInAirborne;
			if (snapConditions)
			{
				var distanceToGround = Mathf.Max(0.0f, groundDistance - groundMinDistance);
				var snapVelocity = transform.up * (-distanceToGround * snapPower / Time.deltaTime);

				targetVelocity = (targetVelocity + snapVelocity).normalized * targetVelocity.magnitude;
				useVerticalVelocity = false;
			}

		}

		void CalculateStepOffset(Vector3 moveDir, ref Vector3 targetVelocity, ref bool useVerticalVelocity)
		{
			if (useStepOffset && isGrounded && !disableCheckGround && !isSliding && !character.isJumping && !customAction && !character.isInAirborne)
			{
				Vector3 dir = Vector3.Lerp(transform.forward, moveDir.normalized, inputSmooth.magnitude);
				float distance = col.radius + stepOffsetDistance;
				float height = (stepOffsetMaxHeight + 0.01f + col.radius * 0.5f);
				Vector3 pA = transform.position + transform.up * (stepOffsetMinHeight + 0.05f);
				Vector3 pB = pA + dir.normalized * distance;
				if (Physics.Linecast(pA, pB, out stepOffsetHit, groundLayer))
				{
					Debug.DrawLine(pA, stepOffsetHit.point);
					distance = stepOffsetHit.distance + 0.1f;
				}
				Ray ray = new Ray(transform.position + transform.up * height + dir.normalized * distance, Vector3.down);

				if (Physics.SphereCast(ray, col.radius * 0.5f, out stepOffsetHit, (stepOffsetMaxHeight - stepOffsetMinHeight), groundLayer) && stepOffsetHit.point.y > transform.position.y)
				{
					dir = (stepOffsetHit.point) - transform.position;
					dir.Normalize();
					//var v = targetVelocity;
					//v.y = 0;
					//targetVelocity = dir * v.magnitude;
					targetVelocity = Vector3.Project(targetVelocity, dir);
					applyingStepOffset = true;
					useVerticalVelocity = false;
					return;
				}
			}

			applyingStepOffset = false;
		}

		public virtual void StopCharacterWithLerp()
		{
			input = Vector3.Lerp(input, Vector3.zero, 2f * Time.deltaTime);
			rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 4f * Time.deltaTime);
			inputMagnitude = Mathf.Lerp(inputMagnitude, 0f, 2f * Time.deltaTime);
			moveSpeed = Mathf.Lerp(moveSpeed, 0f, 2f * Time.deltaTime);
		}

		public virtual void StopCharacter()
		{
			input = Vector3.zero;
			rb.velocity = Vector3.zero;
			inputMagnitude = 0f;
			moveSpeed = 0f;
		}

		public virtual void StopMove()
		{
			if (input.sqrMagnitude < 0.1)
			{
				return;
			}

			RaycastHit hitinfo;
			Ray ray = new Ray(transform.position + Vector3.up * stopMoveHeight, moveDirection.normalized);
			var hitAngle = 0f;
			if (debugWindow)
			{
				Debug.DrawRay(ray.origin, ray.direction * stopMoveDistance, Color.red);
			}

			if (Physics.Raycast(ray, out hitinfo, col.radius + stopMoveDistance, stopMoveLayer))
			{
				stopMove = true;
				return;
			}

			if (Physics.Linecast(transform.position + Vector3.up * (col.height * 0.5f), transform.position + moveDirection.normalized * (col.radius + 0.2f), out hitinfo, groundLayer))
			{
				hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);
				if (debugWindow)
				{
					Debug.DrawLine(transform.position + Vector3.up * (col.height * 0.5f), transform.position + moveDirection.normalized * (col.radius + 0.2f), (hitAngle > slopeLimit) ? Color.yellow : Color.blue, 0.01f);
				}

				var targetPoint = hitinfo.point + moveDirection.normalized * col.radius;
				if ((hitAngle > slopeLimit) && Physics.Linecast(transform.position + Vector3.up * (col.height * 0.5f), targetPoint, out hitinfo, groundLayer))
				{
					if (debugWindow)
					{
						Debug.DrawRay(hitinfo.point, hitinfo.normal);
					}

					hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

					if (hitAngle > slopeLimit && hitAngle < 85f)
					{
						if (debugWindow)
						{
							Debug.DrawLine(transform.position + Vector3.up * (col.height * 0.5f), hitinfo.point, Color.red, 0.01f);
						}

						stopMove = true;
						return;
					}
					else
					{
						if (debugWindow)
						{
							Debug.DrawLine(transform.position + Vector3.up * (col.height * 0.5f), hitinfo.point, Color.green, 0.01f);
						}
					}
				}
			}
			else if (debugWindow)
			{
				Debug.DrawLine(transform.position + Vector3.up * (col.height * 0.5f), transform.position + moveDirection.normalized * (col.radius * 0.2f), Color.blue, 0.01f);
			}

			stopMove = false;
		}

		public virtual void RotateToPosition(Vector3 position)
		{
			Vector3 desiredDirection = position - transform.position;
			RotateToDirection(desiredDirection.normalized);
		}

		public virtual void RotateToDirection(Vector3 direction)
		{
			RotateToDirection(direction, isStrafing ? strafeSpeed.rotationSpeed : freeSpeed.rotationSpeed);
		}

		public virtual void RotateToDirection(Vector3 direction, float rotationSpeed)
		{
			if (/*lockAnimRotation ||*/ customAction || (!jumpAndRotate && !isGrounded) || isSliding || ragdolled)
			{
				return;
			}

			direction.y = 0f;
			if (direction.normalized.magnitude == 0)
			{
				direction = transform.forward;
			}

			var euler = transform.rotation.eulerAngles.NormalizeAngle();
			var targetEuler = Quaternion.LookRotation(direction.normalized).eulerAngles.NormalizeAngle();

			euler.y = Mathf.LerpAngle(euler.y, targetEuler.y, rotationSpeed * Time.deltaTime);


			Quaternion _newRotation = Quaternion.Euler(euler);
			transform.rotation = _newRotation;
		}

		/// <summary>
		/// Check if <see cref="input"/> and <see cref="inputSmooth"/> has some value greater than 0.1f
		/// </summary>
		public bool hasMovementInput
		{
			get => ((inputSmooth.sqrMagnitude + input.sqrMagnitude) > 0.1f || (input - inputSmooth).sqrMagnitude > 0.1f);
		}

		public virtual void AirControl()
		{
			if ((isGrounded && !character.isJumping) || isSliding)
			{
				return;
			}
			if (transform.position.y > heightReached)
			{
				heightReached = transform.position.y;
			}

			inputSmooth = Vector3.Lerp(inputSmooth, input, airSmooth * Time.deltaTime);

			if (jumpWithRigidbodyForce && !isGrounded)
			{
				rb.AddForce(moveDirection * airSpeed * Time.deltaTime, ForceMode.VelocityChange);
				return;
			}

			moveDirection.y = 0;
			moveDirection.x = Mathf.Clamp(moveDirection.x, -1f, 1f);
			moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);

			Vector3 targetPosition = rb.position + (moveDirection * airSpeed) * Time.deltaTime;
			Vector3 targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

			targetVelocity.y = rb.velocity.y;
			rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, airSmooth * Time.deltaTime);
		}

		#region Ground Check                

		protected virtual void CheckGround()
		{
			CheckGroundDistance();
			Sliding();
			ControlMaterialPhysics();

			if (/*isDead || */customAction || disableCheckGround || isSliding)
			{
				isGrounded = true;
				heightReached = transform.position.y;
				return;
			}

			if (groundDistance <= groundMinDistance || applyingStepOffset)
			{
				CheckFallDamage();
				isGrounded = true;
				if (!useSnapGround && !applyingStepOffset && !character.isJumping && groundDistance > 0.05f && extraGravity != 0)
				{
					rb.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
				}

				heightReached = transform.position.y;
			}
			else
			{
				if (groundDistance >= groundMaxDistance)
				{
					if (!character.isRolling)
					{
						isGrounded = false;
					}

					// check vertical velocity
					verticalVelocity = rb.velocity.y;
					// apply extra gravity when falling
					if (!applyingStepOffset && !character.isJumping && extraGravity != 0)
					{
						rb.AddForce(transform.up * extraGravity * Time.deltaTime, ForceMode.VelocityChange);
					}
				}
				else if (!applyingStepOffset && !character.isJumping && extraGravity != 0)
				{
					rb.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
				}
			}
		}

		protected virtual void CheckFallDamage()
		{
			if (isGrounded || verticalVelocity > fallMinVerticalVelocity || !canApplyFallDamage || fallMinHeight == 0 || fallDamage == 0)
			{
				return;
			}

			float fallHeight = (heightReached - transform.position.y);

			fallHeight -= fallMinHeight;
			if (fallHeight > 0)
			{
				int damage = (int)(fallDamage * fallHeight);
				// 여기서 낙뎀 주는게 제일 낫겠지?
				// TakeDamage(new vDamage(damage, true));
			}
		}

		private void ControlMaterialPhysics()
		{
			// change the physics material to very slip when not grounded
			var targetMaterialPhysics = currentMaterialPhysics;

			if (isGrounded && input.magnitude < 0.1f && !isSliding && targetMaterialPhysics != maxFrictionPhysics)
			{
				targetMaterialPhysics = maxFrictionPhysics;
			}
			else if ((isGrounded && input.magnitude > 0.1f) && !isSliding && targetMaterialPhysics != frictionPhysics)
			{
				targetMaterialPhysics = frictionPhysics;
			}
			else if (targetMaterialPhysics != slippyPhysics && isSliding)
			{
				targetMaterialPhysics = slippyPhysics;
			}


			if (currentMaterialPhysics != targetMaterialPhysics)
			{
				col.material = targetMaterialPhysics;
				currentMaterialPhysics = targetMaterialPhysics;
			}
		}

		protected virtual void CheckGroundDistance()
		{
			//if (isDead)
			//{
			//	return;
			//}

			if (col != null)
			{
				// radius of the SphereCast
				float radius = col.radius * 0.9f;
				var dist = groundDetectionDistance;
				// ray for RayCast
				Ray ray2 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);
				// raycast for check the ground distance
				if (Physics.Raycast(ray2, out groundHit, (colliderHeight / 2) + dist, groundLayer) && !groundHit.collider.isTrigger)
				{
					dist = transform.position.y - groundHit.point.y;
				}
				// sphere cast around the base of the capsule to check the ground distance
				if (groundCheckMethod == GroundCheckMethod.High && dist >= groundMinDistance)
				{
					Vector3 pos = transform.position + Vector3.up * (col.radius);
					Ray ray = new Ray(pos, -Vector3.up);
					if (Physics.SphereCast(ray, radius, out groundHit, col.radius + groundMaxDistance, groundLayer) && !groundHit.collider.isTrigger)
					{
						Physics.Linecast(groundHit.point + (Vector3.up * 0.1f), groundHit.point + Vector3.down * 0.15f, out groundHit, groundLayer);
						float newDist = transform.position.y - groundHit.point.y;
						if (dist > newDist)
						{
							dist = newDist;
						}
					}
				}
				groundDistance = (float)System.Math.Round(dist, 2);
			}
		}

        #endregion

        protected virtual void Sliding()
		{
			if (useSlide && groundDistance <= groundMinDistance && GroundAngle() > slopeLimit && !disableCheckGround)
			{
				if (internalSlidingEnterTime <= 0f || isSliding)
				{
					var normal = groundHit.normal;
					normal.y = 0f;
					var dir = Vector3.ProjectOnPlane(normal.normalized, groundHit.normal).normalized;

					if (Physics.Raycast(transform.position + Vector3.up * groundMinDistance, dir, groundMaxDistance, groundLayer))
					{
						isSliding = false;
					}
					else
					{
						isSliding = true;
						SlideMovementBehavior();
					}
				}
				else
				{
					internalSlidingEnterTime -= Time.deltaTime;
				}
			}
			else
			{
				internalSlidingEnterTime = slidingEnterTime;
				isSliding = false;
			}
		}

		protected virtual void SlideMovementBehavior()
		{
			var normal = groundHit.normal;
			normal.y = 0f;
			var dir = Vector3.ProjectOnPlane(normal.normalized, groundHit.normal).normalized;
			rb.velocity = dir * slideDownVelocity;
			dir.y = 0f;

			Vector3 desiredForward = Vector3.RotateTowards(transform.forward, dir, 10f * Time.deltaTime, 0f);
			Quaternion _newRotation = Quaternion.LookRotation(desiredForward);
			rb.MoveRotation(_newRotation);

			var rightMovement = transform.InverseTransformDirection(moveDirection);
			rightMovement.y = 0f;
			rightMovement.z = 0f;
			rightMovement = transform.TransformDirection(rightMovement);

			rb.AddForce(rightMovement * slideSidewaysVelocity, ForceMode.VelocityChange);

			if (debugWindow)
			{
				Debug.DrawRay(transform.position, Vector3.ProjectOnPlane(normal.normalized, groundHit.normal).normalized, Color.blue);
				Debug.DrawRay(transform.position, Quaternion.AngleAxis(90, groundHit.normal) * Vector3.ProjectOnPlane(normal.normalized, groundHit.normal).normalized, Color.red);
				Debug.DrawRay(transform.position, transform.TransformDirection(rightMovement.normalized * 2f), Color.green);
			}
		}

		public virtual float GroundAngle()
		{
			var groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
			return groundAngle;
		}

		#region Colliders Check

		public virtual void ControlCapsuleHeight()
		{
			if (isCrouching || character.isRolling)
			{
				col.center = colliderCenter / crouchHeightReduction;
				col.height = colliderHeight / crouchHeightReduction;
				col.radius = colliderRadius;
			}
			else
			{
				// back to the original values
				col.center = colliderCenter;
				col.radius = colliderRadius;
				col.height = colliderHeight;
			}
		}

		/// <summary>
		/// Reset Capsule Height, Radius and Center to default values
		/// </summary>
		public void ResetCapsule()
		{
			colliderCenter = colliderCenterDefault;
			colliderRadius = colliderRadiusDefault;
			colliderHeight = colliderHeightDefault;
		}

		/// <summary>
		/// Disables rigibody gravity, turn the capsule collider trigger and reset all input from the animator.
		/// </summary>
		public virtual void DisableGravityAndCollision()
		{
			// animator.SetFloat("InputHorizontal", 0f);
			// animator.SetFloat("InputVertical", 0f);
			// animator.SetFloat("VerticalVelocity", 0f);
			//Disable gravity and collision
			rb.useGravity = false;
			rb.isKinematic = true;
			col.isTrigger = true;
			//Reset rigidbody velocity
			rb.velocity = Vector3.zero;
		}

		/// <summary>
		/// Turn rigidbody gravity on the uncheck the capsulle collider as Trigger
		/// </summary>      
		public virtual void EnableGravityAndCollision()
		{
			// Enable collision and gravity
			col.isTrigger = false;
			rb.useGravity = true;
			rb.isKinematic = false;
		}

		#endregion

		#region Ragdoll 

		protected virtual void CheckRagdoll()
		{
			if (ragdollVelocity == 0)
			{
				return;
			}

			// check your verticalVelocity and assign a value on the variable RagdollVel at the Player Inspector
			if (verticalVelocity <= ragdollVelocity && groundDistance <= 0.1f && canApplyFallDamage && !ragdolled)
			{
				// 래그돌 이벤트
				// onActiveRagdoll.Invoke(null);
			}
		}

		public virtual void ResetRagdoll()
		{
			StopCharacter();
			verticalVelocity = 0f;
			ragdolled = false;
			rb.WakeUp();

			rb.useGravity = true;
			rb.isKinematic = false;
			col.isTrigger = false;
			col.enabled = true;
		}

		public virtual void EnableRagdoll()
		{
			// animator.SetFloat("InputHorizontal", 0f);
			// animator.SetFloat("InputVertical", 0f);
			// animator.SetFloat("VerticalVelocity", 0f);
			ragdolled = true;
			col.isTrigger = true;
			//_capsuleCollider.enabled = false;
			rb.useGravity = false;
			rb.isKinematic = true;
			//lockAnimMovement = true;
		}

		#endregion
	}
}
