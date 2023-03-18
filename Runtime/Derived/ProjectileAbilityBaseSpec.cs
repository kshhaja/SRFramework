using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SRFramework.Ability.Object;
using SRFramework.Effect.Spec;


namespace SRFramework.Ability.Spec
{
    public class ProjectileAbilityBaseSpec : GameplayAbilitySpec<ProjectileAbilityBase>
    {
        public ProjectileAbilityBaseSpec(ProjectileAbilityBase ability, AbilitySystemComponent instigator, AbilitySystemComponent source, float level) : base(ability, instigator, source, level)
        {
        }

        public override IEnumerator ActivateAbility()
        {
            var go = GameObject.Instantiate(ability.projectile.gameObject, Source.transform.position + (Vector3.up * 1.5f), Source.transform.rotation);
            go.transform.forward = Source.transform.forward;
            var projectile = go.GetComponent<Projectile>();
            projectile.weakSource = weakSource;
            projectile.weakInstigator = weakInstigator;
            projectile.weakTargets = weakTargets;

            // spec을 여기서 만드는게 맞는가?
            List<GameplayEffectSpec> specs = new List<GameplayEffectSpec>();
            foreach (var item in ability.effectContainers)
            {
                foreach (var effect in item.targetGameplayEffects)
                {
                    specs.Add(Source.MakeOutgointEffectSpec(effect, level));
                }
            }
            projectile.effects = specs;
            yield return null;
        }
    }
}
