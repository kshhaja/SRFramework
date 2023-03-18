using SlimeRPG.Framework.StatsSystem;
using System;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectDuration
    {
        public Duration policy;
        public GameplayEffectModifierMagnitude magnitude;
    }
}