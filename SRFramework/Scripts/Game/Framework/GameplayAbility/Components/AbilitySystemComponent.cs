using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem.StatsContainers;
using SlimeRPG.Framework.Tag;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class AbilitySystemComponent : MonoBehaviour
    {
        private List<GameplayEffectSpec> appliedSpecs = new List<GameplayEffectSpec>();
        private List<GameplayAbilitySpec> grantedAbilities = new List<GameplayAbilitySpec>();
        
        [SerializeField]
        protected StatsContainer container;

        public struct GameplayTagCountContainer
        {
            public Dictionary<GameplayTag, int> gameplayTagCountMap;

            public bool HasAnyMatchingGameplayTags(GameplayTagContainer container)
            {
                if (container.Num() == 0)
                    return false;

                bool anyMatch = false;
                foreach (var tag in container.Tags)
                {
                    if (gameplayTagCountMap.ContainsKey(tag))
                    {
                        anyMatch = true;
                        break;
                    }
                }
                return anyMatch;
            }
        }

        public GameplayTagCountContainer gameplayTagCountContainer;
        public StatsContainer StatsContainer => container;

        public List<GameplayEffectSpec> AppliedSpecs => appliedSpecs;


        private void Awake()
        {
            container = container.CreateRuntimeCopy();

            // test
            container.OnStatChangeSubscribe("currentHealth", (record) =>
            {
                var v = record.GetValue();
                Debug.Log("Current Health = " + v.ToString());
                if (v <= 0)
                    Debug.LogError("Dead");
            });

            container.OnStatChangeSubscribe("dexterity", (record) =>
            {
                var v = record.GetValue();
                Debug.Log("Dexterity = " + v.ToString());
            });
        }

        private void Update()
        {
            foreach (var spec in appliedSpecs)
            {
                if (spec.effect.duration.policy == Duration.instant)
                    continue;

                spec.UpdateRemainingDuration(Time.deltaTime);
                spec.UpdatePeriod(Time.deltaTime, out var execute);

                if (execute)
                    ApplyInstantGameplayEffect(spec);
            }
        }
        
        public int GrantAbility(GameplayAbility ability)
        {
            var v = MakeOutgoingAbilitySpec(ability, 1);
            return GrantAbility(v);
        }

        public int GrantAbility(GameplayAbilitySpec ability)
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
            foreach (var spec in grantedAbilities)
            {
                if (spec.ability.tags.abilityTags.HasTag(tag))
                {
                    grantedAbilities.Remove(spec);
                }
            }
        }

        public virtual bool ApplyGameplayEffect(GameplayEffectSpec spec)
        {
            if (spec == null)
                return true;

            // check immunity

            // check chance to apply

            // check requirements

            // check 

            switch (spec.effect.duration.policy)
            {
                case Duration.duration:
                case Duration.infinite:
                    ApplyDurationalGameplayEffect(spec);
                    break;
                case Duration.instant:
                    ApplyInstantGameplayEffect(spec);
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
            appliedSpecs.Add(ge);
        }

        public GameplayAbilitySpec MakeOutgoingAbilitySpec(GameplayAbility ability, float level)
        {
            return GameplayAbilitySpec.CreateNew(ability, null, null, level);
        }

        public GameplayEffectSpec MakeOutgoingSpec(GameplayEffect effect, float level)
        {
            return GameplayEffectSpec.CreateNew(effect, this, level);
        }
    }
}
