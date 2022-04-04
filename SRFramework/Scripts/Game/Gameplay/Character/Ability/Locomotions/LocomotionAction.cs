using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Gameplay.Character.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Ability/Locomotion", order = 4)]
    public class LocomotionAction : AbilityBase
    {
        protected Vector3 startPos;
        protected Quaternion startRot;
        protected Vector3 targetPos;
        protected Quaternion targetRot;

        public Vector3 kneeRaycastOrigin;
        public float kneeRaycastLength = 1.0f;
        public float landOffset = 0.7f;
        public float startDelay = 0.0f;
        public LayerMask layer;

        protected float locomoTime = 0.0f;
        protected float animLength = 0.0f;

        public string tag;

        protected override IEnumerator PreCast()
        {
            yield return base.PreCast();

            owner.movementController.isVaulting = true;
            // owner.animationController.SubscribeOnAnimatorIK(OnAnimatorIK);
            yield return null;
        }

        public override void CancelAbility()
        {
        }

        public override bool CheckGameplayTags()
        {
            return true;
        }
    }
}