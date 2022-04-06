using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem.StatsContainers;
using SlimeRPG.Framework.Tag;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class AbilitySystemCharacter : MonoBehaviour
    {
        private List<GameplayEffectSpec> appliedSpecs = new List<GameplayEffectSpec>();
        private List<AbstractAbilityScriptableObject> grantedAbilities = new List<AbstractAbilityScriptableObject>();
        public StatsContainer container { get; private set; }

        public List<GameplayEffectSpec> AppliedSpecs => appliedSpecs;
        

        public void GrantAbility(AbstractAbilityScriptableObject ability)
        {
            grantedAbilities.Add(ability);
        }

        public void RemoveAbilitiesWithTag(GameplayTag tag)
        {
            foreach (var ability in grantedAbilities)
            {
                if (ability.abilityTags.assetTag == tag)
                {
                    grantedAbilities.Remove(ability);
                }
            }
        }

        public virtual bool ApplyGameplayEffect(GameplayEffectSpec ge)
        {
            if (ge == null)
                return true;

            switch (ge.effect.duration.policy)
            {
                case Duration.duration:
                case Duration.infinite:
                    ApplyDurationalGameplayEffect(ge.effect);
                    break;
                case Duration.instant:
                    ApplyInstantGameplayEffect(ge.effect);
                    return true;
            }

            return true;
        }

        protected virtual void ApplyInstantGameplayEffect(GameplayEffectScriptableObject ge)
        {
            // ge spec으로 변경
            ge.modifiers.ApplyAdjustment(container);
        }

        protected virtual void ApplyDurationalGameplayEffect(GameplayEffectScriptableObject ge)
        {
            Debug.Log("apply durational GE");
        }

        public GameplayEffectSpec MakeOutgoingSpec(GameplayEffectScriptableObject effect, float level)
        {
            return GameplayEffectSpec.CreateNew(effect, this, level);
        }
    }
}
