using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.Ability;
using SlimeRPG.Framework.Tag;
using SlimeRPG.Gameplay.Character.Ability;


namespace SlimeRPG.Gameplay.Character.Controller
{
    public class AbilityController : AbstractCharacterController
    {
        public class GameplayEffectContainer
        {
            public AbilityBase ability;
            public StatsAdjustment modifiers;
        }

        protected AttributeController attribute;

        public List<GameplayEffectContainer> appliedGameplayEffects = new List<GameplayEffectContainer>();
        public List<AbilityBase> grantedAbilities = new List<AbilityBase>();

        public int GrantAbility(AbilityBase ability)
        {
            grantedAbilities.Add(ability);
            return grantedAbilities.LastIndexOf(ability);
        }

        public void CastAbility(int index)
        {
            StartCoroutine(grantedAbilities[index].TryCastAbility());
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

        protected virtual bool CheckTagRequirementsMet(GameplayEffectScriptableObject ge)
        {
            /// Build temporary list of all gametags currently applied
            //var appliedTags = new List<GameplayTagScriptableObject>();
            //for (var i = 0; i < appliedGameplayEffects.Count; i++)
            //{
            //    appliedTags.AddRange(appliedGameplayEffects[i].ability.effect.gameplayEffectTags.GrantedTags);
            //}

            //// Every tag in the ApplicationTagRequirements.RequireTags needs to be in the character tags list
            //// In other words, if any tag in ApplicationTagRequirements.RequireTags is not present, requirement is not met
            //for (var i = 0; i < ge.gameplayEffectTags.ApplicationTagRequirements.RequireTags.Length; i++)
            //{
            //    if (!appliedTags.Contains(ge.gameplayEffectTags.ApplicationTagRequirements.RequireTags[i]))
            //    {
            //        return false;
            //    }
            //}

            //// No tag in the ApplicationTagRequirements.IgnoreTags must in the character tags list
            //// In other words, if any tag in ApplicationTagRequirements.IgnoreTags is present, requirement is not met
            //for (var i = 0; i < ge.gameplayEffectTags.ApplicationTagRequirements.IgnoreTags.Length; i++)
            //{
            //    if (appliedTags.Contains(ge.gameplayEffectTags.ApplicationTagRequirements.IgnoreTags[i]))
            //    {
            //        return false;
            //    }
            //}

            return true;
        }

        public bool ApplyGameplayEffect(GameplayEffectScriptableObject ge)
        {
            if (ge == null)
                return true;

            if (CheckTagRequirementsMet(ge))
                return false;

            attribute.ApplyGameplayEffect(ge);

            return true;
        }

        protected virtual void UpdateAttributeSystem()
        {
            foreach (var ge in appliedGameplayEffects)
            {
                foreach (var mod in ge.modifiers.adjustment)
                {
                    mod.GetValue(0);
                }
            }
        }

        void UpdateAbilities()
        {
            foreach (var ability in appliedGameplayEffects)
            {
                if (ability.ability.effect.gameplayEffect.duration.policy == EDurationPolicy.Instant)
                    continue;

                // ability.ability.
            }
        }

        void Update()
        {

        }
    }
}
