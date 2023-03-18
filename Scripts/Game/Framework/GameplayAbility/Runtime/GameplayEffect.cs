using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Effect/Base Definition", order = 1)]
    [System.Serializable]
    public class GameplayEffect : ScriptableObject
    {
        public GameplayEffectDuration duration;
        public GameplayEffectPeriod period;

        public List<GameplayModifierInfo> modifiers;
        public List<GameplayEffectExecutionDefinition> executions;

        public ApplicationInfo application;
        public List<ConditionalGameplayEffect> conditionalGameplayEffects;

        public OverflowInfo overflow;

        public ExpirationInfo expiration;
        public DisplayInfo display;

        public GameplayEffectTags tags;

        public GameplayEffectImmunity immunity;
        public GameplayEffectStacking stacking;

        public List<GameplayAbility> grantedAbilities;
    }
}
