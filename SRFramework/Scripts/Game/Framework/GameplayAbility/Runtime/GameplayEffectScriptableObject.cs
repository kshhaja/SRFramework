using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Ability/Effect Definition")]
    public class GameplayEffectScriptableObject : ScriptableObject
    {
        public GameplayEffectDuration duration;

        public StatAdjustmentCollection modifiers;
        public ExecutionContainer execution;
        public List<ConditionalGameplayEffectContainer> conditionalGameplayEffects;

        public GameplayEffectTags gameplayEffectTags;
    }
}
