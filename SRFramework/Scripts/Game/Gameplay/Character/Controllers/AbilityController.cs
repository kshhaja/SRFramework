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
            public StatAdjustmentCollection modifiers;
        }

        protected AttributeController attribute;

        public GameplayTagContainer grantedTags;
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
            return grantedTags.HasAllTags(ge.gameplayEffectTags.ApplicationTagRequirements.requireTags)
                && grantedTags.HasNoneTags(ge.gameplayEffectTags.ApplicationTagRequirements.ignoreTags);
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
                if (ability.ability.effect.duration.policy == Duration.instant)
                    continue;


            }
        }

        void Update()
        {

        }
    }
}
