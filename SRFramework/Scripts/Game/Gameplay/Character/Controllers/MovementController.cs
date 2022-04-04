using UnityEngine;
using UnityEngine.AI;


namespace SlimeRPG.Gameplay.Character.Controller
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : AbstractCharacterController
    {
        [HideInInspector]
        public bool isNavigating;

        public float moveSpeed = 3f;
        public float turnSpeed = 0.1f;
        public float rotationSpeed = 1.0f;

        public float movementSmoothingSpeed = 0.25f;

        public bool isVaulting = false;

        public Vector3 moveDirection;
        public Vector3 aimPoint;

        protected NavMeshAgent agent;

        private NavMeshPath calculatedPath;

        // 이 기능은 detection으로 빼는게 좋을듯
        private RaycastHit[] raycastHitCache = new RaycastHit[16];
        Vector3 lastRaycastResult;

        private float lerpTime = 0;
        private Vector3 lastDirection;
        private Vector3 targetDirection;
        private float targetLerpSpeed = 1;

        public bool isGrounded { get; set; }
        /// <summary>
        /// use to stop update the Check Ground method and return true for IsGrounded
        /// </summary>
        public bool disableCheckGround { get; set; }
        public bool inCrouchArea { get; protected set; }
        public bool isSprinting { get; set; }
        public bool isSliding { get; protected set; }
        public bool stopMove { get; protected set; }
        public bool autoCrouch { get; protected set; }


        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<CharacterBase>();
        }

        private void Start()
        {
            calculatedPath = new NavMeshPath();
        }

        protected virtual void Update()
        {
            
        }

        public void Stop()
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }

        public void SetDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        public void Warp(Vector3 location)
        {
            agent.Warp(location);
            agent.isStopped = true;
            agent.ResetPath();
        }

        public void MoveCheck(Ray screenRay)
        {
            if (calculatedPath.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetPath(calculatedPath);
                calculatedPath.ClearCorners();
            }

            if (Physics.RaycastNonAlloc(screenRay, raycastHitCache, 1000.0f/*, levelLayer*/) > 0)
            {
                Vector3 point = raycastHitCache[0].point;

                if (Vector3.SqrMagnitude(point - lastRaycastResult) > 1.0f)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(point, out hit, 0.5f, NavMesh.AllAreas))
                    {
                        lastRaycastResult = point;

                        agent.CalculatePath(hit.position, calculatedPath);
                    }
                }
            }
        }

        protected void UpdateMovement()
        {
            moveDirection.Normalize();
            if (moveDirection != lastDirection)
                lerpTime = 0;

            lastDirection = moveDirection;
            targetDirection = Vector3.Lerp(targetDirection, moveDirection,
                Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - movementSmoothingSpeed)));

            agent.SetDestination(transform.position + moveDirection);
            // agent.Move(targetDirection * agent.speed * Time.deltaTime);

            Vector3 lookDirection = moveDirection;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDirection),
                    Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - movementSmoothingSpeed))
                );
            }

            lerpTime += Time.deltaTime;
        }

        private void RotateTowardsMovementDirection()
        {
            if (agent.velocity.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(agent.velocity),
                    Time.deltaTime * agent.angularSpeed * rotationSpeed);

                Quaternion rot = transform.rotation;
                rot.eulerAngles = new Vector3(0, rot.eulerAngles.y, 0);
                transform.rotation = rot;
            }
        }
    }
}