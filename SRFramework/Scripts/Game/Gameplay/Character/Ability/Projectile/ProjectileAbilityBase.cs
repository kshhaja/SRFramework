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
            //return AscHasAllTags(Owner, this.Ability.AbilityTags.OwnerTags.RequireTags) &&
            //    AscHasNoneTags(Owner, this.Ability.AbilityTags.OwnerTags.IgnoreTags) &&
            //    AscHasAllTags(Owner, this.Ability.AbilityTags.SourceTags.RequireTags) &&
            //    AscHasNoneTags(Owner, this.Ability.AbilityTags.SourceTags.IgnoreTags);
            return true;
        }

        protected override IEnumerator PreCast()
        {
            yield return base.PreCast();
        }

        protected override IEnumerator CastAbility()
        {
            yield return base.CastAbility();

            // 애니메이션 특정 시점에 이벤트가 발생할때까지 대기를 하려면 메카님은 제약이 많다...
            // Playable로 변경예정... 작업량이 너무 많아서 기약없음.
            // yield return owner.animationController.PlayAnimationClip(clip);
            yield return owner.animationController.PlayAttackAnimation();

            GameObject go = null;
            if (castPoint)
                go = Instantiate(projectile.gameObject, castPoint.transform.position, castPoint.transform.rotation);
            else
                go = Instantiate(projectile.gameObject, owner.transform.position + (Vector3.up * 1.5f), owner.transform.rotation);

            // 메시 / 히트 이펙트등 여기서 설정. 
            var projectileInstance = go.GetComponent<Projectile>();
            projectileInstance.effect = effect;
            projectileInstance.secondaryAbility = secondaryAbility as AbilityBase;

            projectileInstance.source = owner;
            
            //    //float damageDone = damageValue.CurrentValue * apValue.CurrentValue / dpValue.CurrentValue;

            //    // 어떤 식으로 데미지를 줄지 여러가지를 구현하여 선택할 수 있도록 해야한다.
            //    // ex. 단일, 범위 등.

            EndAbility();
            yield break;
        }
    }
}