using System.Collections;
using System.Collections.Generic;
using SlimeRPG.Character;
using UnityEngine;


namespace SlimeRPG.Gameplay.Character.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Ability/Locomotion/Reach")]
    public class LocomotionReach : LocomotionAction
    {
        public float maxHeight = 0;
        public float midHeight = 0;

        private float height = 0;

        private Vector3 leftHandPosition;
        public string handAnimVariableName;

        private DetectionController detection;
        private Rigidbody rb;
        private RaycastHit[] hits = new RaycastHit[3];


        //public void Setup(CharacterBase owner)
        //{
        //    //base.Setup(owner);
        //    detection = owner.Detection;
        //    rb = owner.GetComponent<Rigidbody>();
        //}

        ////public override bool CanActivateAbility()
        ////{
        ////    return base.CanActivateAbility() & CheckAction();
        ////}

        //bool CheckAction()
        //{
        //    if (Instigator.Movement.isVaulting == false)
        //    {
        //        Vector3 origin = Instigator.transform.position + kneeRaycastOrigin;

        //        if (detection.ThrowRayOnDirection(origin, Instigator.transform.forward, kneeRaycastLength, out hits[0], layer))
        //        {
        //            if (hits[0].collider.gameObject.tag != tag)
        //                return false;

        //            Vector3 origin2 = hits[0].point + (-hits[0].normal * (landOffset)) + new Vector3(0, 5, 0);
        //            detection.ThrowRayOnDirection(Instigator.transform.position, Vector3.down, 1, out hits[2]);
        //            if (detection.ThrowRayOnDirection(origin2, Vector3.down, 10, out hits[1], layer)) //Ground Hit
        //            {
        //                height = hits[1].point.y - Instigator.transform.position.y;

        //                if (height > maxHeight
        //                    || hits[0].collider.gameObject.tag != tag
        //                    || hits[1].collider != hits[0].collider
        //                    || hits[2].collider == hits[1].collider)
        //                    return false;

        //                if (hits[1].collider)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        ////public override IEnumerator PreActivateAbility()
        ////{
        ////    yield return base.PreActivateAbility();

        ////    if (height <= 1)
        ////        animator.CrossFade("Reach", 0.1f);
        ////    else
        ////        animator.CrossFade("Reach High", 0.1f);

        ////    startPos = Instigator.transform.position;
        ////    targetPos = hits[1].point;
        ////    targetRot = Quaternion.LookRotation(-hits[0].normal);
        ////    locomoTime = 0f;
        ////    animLength = clip.length + startDelay;
        ////    //owner.LockInput(true);

        ////    rb.isKinematic = true;

        ////    Vector3 right = Vector3.Cross(hits[0].normal, Vector3.up);
        ////    leftHandPosition = hits[0].point + (right * -0.5f);
        ////    leftHandPosition.y = hits[1].point.y;
        ////}

        ////public override IEnumerator ActivateAbility()
        ////{
        ////    yield return base.ActivateAbility();

        ////    while (Update())
        ////        yield return null;

        ////    EndAbility();
        ////    yield return null;
        ////}

        //public override bool Update()
        //{
        //    bool ret = false;
        //    if (Instigator.Movement.isVaulting)
        //    {
        //        // owner.cc.disableCheckGround = true;

        //        ret = true;
        //        float actualSpeed = Time.deltaTime / animLength;
        //        var animState = animator.GetCurrentAnimatorStateInfo(0);
        //        //var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //        locomoTime += actualSpeed * animState.speed;
        //        Instigator.transform.rotation = Quaternion.Lerp(startRot, targetRot, locomoTime);

        //        if (animState.IsName("Reach") || animState.IsName("Reach High"))
        //        {
        //            if (height <= 1)
        //                SetMatchTarget(AvatarTarget.Root, animState, targetPos, targetRot, Vector3.zero, 0, 1.0f);
        //            else
        //                SetMatchTarget(AvatarTarget.Root, animState, targetPos, targetRot, Vector3.zero, 0, 0.25f);

        //            if (animator.IsInTransition(0) && locomoTime > 0.5f)
        //            {
        //                rb.isKinematic = false;

        //                // owner.movement.ToggleWalk();
        //                // owner.LockInput(false);
        //                // owner.cc.rigidbody.isKinematic = false;
        //                // owner.cc.disableCheckGround = false;

        //                height = 0;
        //                ret = false;
        //            }
        //        }

        //        if (locomoTime > 1)
        //        {
        //            // owner.LockBasicInput(false);
        //        }
        //        else
        //        {
        //            if (locomoTime >= 0)
        //            {
        //                Instigator.transform.rotation = Quaternion.Lerp(startRot, targetRot, locomoTime * 4);
        //                Instigator.transform.position = Vector3.Lerp(startPos, targetPos, locomoTime);
        //            }
        //            return true;
        //        }
        //    }

        //    return ret;
        //}

        //private void SetMatchTarget(AvatarTarget avatarTarget, AnimatorStateInfo animState, Vector3 targetPos, Quaternion targetRot, Vector3 offset, float startNormalizedTime, float targetNormalizedTime)
        //{
        //    if (animator.isMatchingTarget)
        //        return;

        //    float normalizeTime = Mathf.Repeat(animState.normalizedTime, 1f);

        //    if (normalizeTime > targetNormalizedTime)
        //        return;
        //    MatchTargetWeightMask matchTargetWeightMask = new MatchTargetWeightMask(Vector3.one, 0);
        //    animator.SetTarget(avatarTarget, targetNormalizedTime); //Just for our reference. Not used here.
        //    animator.MatchTarget(targetPos + offset, targetRot, avatarTarget, matchTargetWeightMask, startNormalizedTime, targetNormalizedTime, true);
        //}

        //public override void OnAnimatorIK(int layerIndex)
        //{
        //    if (height <= 1 || Instigator.Movement.isVaulting == false)
        //        return;

        //    float curve = animator.GetFloat(handAnimVariableName);

        //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, curve);
        //    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPosition);
        //}

        //void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationHodler, ref float lastFootPositionY)
        //{
        //    Vector3 targetIKPosition = animator.GetIKPosition(foot);

        //    if (positionIKHolder != Vector3.zero)
        //    {
        //        targetIKPosition = Instigator.transform.InverseTransformPoint(targetIKPosition);
        //        positionIKHolder = Instigator.transform.InverseTransformPoint(positionIKHolder);

        //        float yVariable = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, 0.25f);
        //        lastFootPositionY = yVariable;
        //        targetIKPosition.y += yVariable;
        //        targetIKPosition = Instigator.transform.TransformPoint(targetIKPosition);
        //    }

        //    animator.SetIKRotation(foot, rotationHodler);
        //    animator.SetIKPosition(foot, targetIKPosition);
        //}

        //public override void DrawGizmos()
        //{
        //    Gizmos.color = Color.magenta;
        //    Gizmos.DrawSphere(leftHandPosition, 0.07f);
        //    Gizmos.DrawSphere(targetPos, 0.07f);
        //}
    }
}