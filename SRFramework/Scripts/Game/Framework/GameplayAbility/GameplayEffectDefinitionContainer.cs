using System;
using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectDefinitionContainer
    {
        public GameplayEffectDuration duration;
        public GameplayModContainer modContainer;
        public List<ConditionalGameplayEffectContainer> conditionalGameplayEffects;
    }

}
