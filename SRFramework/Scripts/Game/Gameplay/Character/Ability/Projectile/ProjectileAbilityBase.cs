using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SlimeRPG.Gameplay.Character.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Ability/Projectile/Projectile Action Base", order = 1)]
    public class ProjectileAbilityBase : AbilityBase
    {
        public Projectile projectile;

        private Transform castPoint;
        
        
        public override void CancelAbility()
        {

        }

        public override bool CheckGameplayTags()
        {
            return Instigator.abilityController.grantedTags.HasAllTags(abilityTags.ownerTags.requireTags)
                && Instigator.abilityController.grantedTags.HasNoneTags(abilityTags.ownerTags.ignoreTags)
                && Instigator.abilityController.grantedTags.HasAllTags(abilityTags.sourceTags.requireTags)
                && Instigator.abilityController.grantedTags.HasNoneTags(abilityTags.sourceTags.ignoreTags);
        }

        protected override IEnumerator PreCast()
        {
            // optional behavior ex> set targets homing missile
            yield return base.PreCast();
        }

        protected override IEnumerator CastAbility()
        {
            yield return base.CastAbility();

            // 애니메이션 특정 시점에 이벤트가 발생할때까지 대기를 하려면 메카님은 제약이 많다...
            // Playable로 변경예정... 작업량이 너무 많아서 기약없음.
            // yield return owner.animationController.PlayAnimationClip(clip);
            yield return Instigator.animationController.PlayAttackAnimation();

            GameObject go = null;
            if (castPoint)
                go = Instantiate(projectile.gameObject, castPoint.transform.position, castPoint.transform.rotation);
            else
                go = Instantiate(projectile.gameObject, Instigator.transform.position + (Vector3.up * 1.5f), Instigator.transform.rotation);

            // 메시 / 히트 이펙트등 여기서 설정. 
            var projectileInstance = go.GetComponent<Projectile>();
            projectileInstance.effect = effect;
            projectileInstance.secondaryAbility = secondaryAbility as AbilityBase;
            projectileInstance.source = Instigator;

            EndAbility();
            yield break;
        }
    }
}