using UnityEngine;
using SlimeRPG.Gameplay.Character.Ability;
using SlimeRPG.Gameplay.Character.Controller;
using SlimeRPG.Framework.Ability;
using SlimeRPG.Framework.Tag;
using SlimeRPG.Framework.StatsSystem.StatsContainers;

namespace SlimeRPG.Gameplay.Character
{
    [System.Serializable]
    public class MovementSpeed
    {
        [Range(1f, 20f)]
        public float movementSmooth = 6f;
        [Range(0f, 1f)]
        public float animationSmooth = 0.2f;
        public float rotationSpeed = 20f;
        public bool walkByDefault = false;
        public bool rotateWithCamera = false;
        public float walkSpeed = 2f;
        public float runningSpeed = 4f;
        public float sprintSpeed = 6f;
        public float crouchSpeed = 2f;
    }

    public class CharacterBase : MonoBehaviour
    {
        [Header("Controllers")]
        protected MovementController movementController;
        protected AnimationController animationController;
        protected DetectionController detectionController;
        protected AbilitySystemCharacter abilitySystem;
        protected GameplayTagContainer grantedTags = new GameplayTagContainer();

        public MovementController Movement => movementController;
        public AnimationController Animation => animationController;
        public DetectionController Detection => detectionController;
        public AbilitySystemCharacter AbilitySystem => abilitySystem;
        public GameplayTagContainer GrantedTags => grantedTags;
        public StatsContainer StatsContainer => AbilitySystem.StatsContainer;


        public bool isRolling,
                    isJumping,
                    isInAirborne,
                    isTurningOnSpot,
                    isStrafing, isSprinting, isCrouching, isSliding, isDead;

        public Transform rotateTarget;


        protected virtual void Awake()
        {
            movementController = GetComponent<MovementController>();
            animationController = GetComponent<AnimationController>();
            detectionController = GetComponent<DetectionController>();
            abilitySystem = GetComponent<AbilitySystemCharacter>();
        }

        public void SetAimPoint(Vector3 point)
        {
            movementController.aimPoint = point;
        }

        // 캐릭터는 기본적으로 WASD 이동 외에는 아무것도 할 수 없다. 다른 모든 액션들은 어빌리티를 이용하도록 한다.
        public void MoveTo(Vector3 direction)
        {
            movementController.moveDirection = direction;
        }

        public void Stop()
        {
            movementController.Stop();
        }

        public virtual int GrantAbility(AbilityBase ability)
        {
            ability.Setup(this);
            return abilitySystem.GrantAbility(ability);
        }

        public virtual void CastAbility(int index)
        {
            abilitySystem.ActivateAbility(index);
        }

        public void FootstepFrame()
        {
            Vector3 pos = transform.position;

            //m_CharacterAudio.Step(pos);

            //SFXManager.PlaySound(SFXManager.Use.Player, new SFXManager.PlayData()
            //{
            //    Clip = SpurSoundClips[Random.Range(0, SpurSoundClips.Length)],
            //    Position = pos,
            //    PitchMin = 0.8f,
            //    PitchMax = 1.2f,
            //    Volume = 0.3f
            //});

            //VFXManager.PlayVFX(VFXType.StepPuff, pos);
        }

        public void AttackFrame()
        {
            //if (targetData == null)
            //{
            //    m_ClearPostAttack = false;
            //    return;
            //}

            ////if we can't reach the target anymore when it's time to damage, then that attack miss.
            //if (Data.CanAttackReach(targetData))
            //{
            //    Data.Attack(targetData);

            //    var attackPos = targetData.transform.position + transform.up * 0.5f;
            //    VFXManager.PlayVFX(VFXType.Hit, attackPos);
            //    SFXManager.PlaySound(m_CharacterAudio.UseType, new SFXManager.PlayData() { Clip = Data.Equipment.Weapon.GetHitSound(), PitchMin = 0.8f, PitchMax = 1.2f, Position = attackPos });
            //}

            //if (m_ClearPostAttack)
            //{
            //    m_ClearPostAttack = false;
            //    targetData = null;
            //    m_TargetInteractable = null;
            //}

            //m_CurrentState = State.DEFAULT;
        }
    }
}
