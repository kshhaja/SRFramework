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
        public StatsContainer container;

        public List<GameplayEffectSpec> AppliedSpecs => appliedSpecs;


        private void Awake()
        {
            container = container.CreateRuntimeCopy();

            container.SetStat("current_health", 20);
            container.OnStatChangeSubscribe("current_health", (record) =>
            {
                var v = record.GetValue();
                Debug.Log("Current Health = " + v.ToString());
                if (v <= 0)
                    Debug.LogError("Dead");
            });
        }

        public int GrantAbility(AbstractAbilityScriptableObject ability)
        {
            grantedAbilities.Add(ability);
            return grantedAbilities.LastIndexOf(ability);
        }

        public void ActivateAbility(int index)
        {
            StartCoroutine(grantedAbilities[index].TryActivateAbility());
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
                    ApplyDurationalGameplayEffect(ge);
                    break;
                case Duration.instant:
                    ApplyInstantGameplayEffect(ge);
                    return true;
            }

            return true;
        }

        protected virtual void ApplyInstantGameplayEffect(GameplayEffectSpec ge)
        {
            ge.ApplyEffectTo(this);
        }

        protected virtual void ApplyDurationalGameplayEffect(GameplayEffectSpec ge)
        {
            Debug.Log("apply durational GE");
        }

        public GameplayEffectSpec MakeOutgoingSpec(GameplayEffectScriptableObject effect, float level)
        {
            return GameplayEffectSpec.CreateNew(effect, this, level);
        }
    }
}
