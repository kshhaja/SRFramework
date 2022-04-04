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

            if (CheckTagRequirementsMet(ge) == false) 
                return false;


            switch (ge.gameplayEffect.duration.policy)
            {
                case EDurationPolicy.HasDuration:
                case EDurationPolicy.Infinite:
                    ApplyDurationalGameplayEffect(ge);
                    break;
                case EDurationPolicy.Instant:
                    ApplyInstantGameplayEffect(ge);
                    return true;
            }

            return true;
        }

        protected virtual void ApplyInstantGameplayEffect(GameplayEffectScriptableObject ge)
        {
            ge.gameplayEffect.adjustment?.ApplyAdjustment(container);
        }

        protected virtual void ApplyDurationalGameplayEffect(GameplayEffectScriptableObject ge)
        {
            Debug.Log("apply durational GE");
        }

        protected virtual bool CheckTagRequirementsMet(GameplayEffectScriptableObject ge)
        {
            //var appliedTags = new List<GameplayTagScriptableObject>();
            
            //foreach (var effect in appliedGameplayEffects)
            //{
            //    appliedTags.AddRange(effect.ge.gameplayEffectTags.GrantedTags);
            //}

            //// require tags
            //foreach (var tag in ge.gameplayEffectTags.ApplicationTagRequirements.RequireTags)
            //{
            //    if (appliedTags.Contains(tag) == false)
            //        return false;
            //}

            //// ignore tags
            //foreach (var tag in ge.gameplayEffectTags.ApplicationTagRequirements.IgnoreTags)
            //{
            //    if (appliedTags.Contains(tag))
            //        return false;
            //}

            return true;
        }

        protected virtual void UpdateAttributeSystem()
        {
            //for (var i = 0; i < appliedGameplayEffects.Count; i++)
            {

                //var modifiers = appliedGameplayEffects[i].modifiers;
                //for (var m = 0; m < modifiers.Length; m++)
                //{
                //    var modifier = modifiers[m];
                //    //AttributeSystem.UpdateAttributeModifiers(modifier.Attribute, modifier.Modifier, out _);
                //}
            }
        }

        void TickGameplayEffects()
        {
            //for (var i = 0; i < appliedGameplayEffects.Count; i++)
            //{
            //    var ge = appliedGameplayEffects[i];

            //    // Can't tick instant GE
            //    //if (ge.GameplayEffect.gameplayEffect.DurationPolicy == EDurationPolicy.Instant) continue;

            //    //// Update time remaining.  Stritly, it's only really valid for durational GE, but calculating for infinite GE isn't harmful
            //    //ge.UpdateRemainingDuration(Time.deltaTime);

            //    //// Tick the periodic component
            //    //ge.TickPeriodic(Time.deltaTime, out var executePeriodicTick);
            //    //if (executePeriodicTick)
            //    //{
            //    //    ApplyInstantGameplayEffect(ge);
            //    //}
            //}
        }

        void CleanGameplayEffects()
        {
            // appliedGameplayEffects.RemoveAll(x => x.ge.gameplayEffect.DurationPolicy == EDurationPolicy.HasDuration /*&& x.ge.DurationRemaining <= 0*/);
        }

        public void Update()
        {
            UpdateAttributeSystem();

            TickGameplayEffects();
            CleanGameplayEffects();
        }
    }
}
