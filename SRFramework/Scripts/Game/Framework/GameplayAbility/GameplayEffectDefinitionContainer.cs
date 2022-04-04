using System;
using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectDefinitionContainer
    {
        public GameplayEffectDuration duration;
        // ModContainer로 변경예정.
        public StatsAdjustment adjustment;
        public List<ConditionalGameplayEffectContainer> conditionalGameplayEffects;
    }

}
