using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem.StatsContainers;
using SlimeRPG.Framework.Tag;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class AbilitySystemCharacter : MonoBehaviour
    {
        private List<AbstractAbilityScriptableObject> grantedAbilities = new List<AbstractAbilityScriptableObject>();
        public StatsContainer container { get; private set; }

        
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

        public virtual bool ApplyGameplayEffect(GameplayEffectScriptableObject ge)
        {
            if (ge == null)
                return true;

            switch (ge.duration.policy)
            {
                case Duration.duration:
                case Duration.infinite:
                    ApplyDurationalGameplayEffect(ge);
                    break;
                case Duration.instant:
                    ApplyInstantGameplayEffect(ge);
                    return true;
            }

            return true;
        }

        protected virtual void ApplyInstantGameplayEffect(GameplayEffectScriptableObject ge)
        {
            ge.modifiers.ApplyAdjustment(container);
        }

        protected virtual void ApplyDurationalGameplayEffect(GameplayEffectScriptableObject ge)
        {
            Debug.Log("apply durational GE");
        }
    }
}
