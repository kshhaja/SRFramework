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
        
        [SerializeField]
        protected StatsContainer container;

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
            appliedSpecs.Add(ge);
        }

        public GameplayEffectSpec MakeOutgoingSpec(GameplayEffectScriptableObject effect, float level)
        {
            return GameplayEffectSpec.CreateNew(effect, this, level);
        }
    }
}
