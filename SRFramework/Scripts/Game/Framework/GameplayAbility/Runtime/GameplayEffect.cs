using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.StatsSystem.StatsContainers;
using SlimeRPG.Framework.Tag;

namespace SlimeRPG.Framework.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Effect/Base Definition", order = 1)]
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

        // immunity

        // stacking

        public List<GameplayAbility> grantedAbilities;
    }
}
