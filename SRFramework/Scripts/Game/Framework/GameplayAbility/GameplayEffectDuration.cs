using System;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectDuration
    {
        public Duration policy;
        public float modifier;
        public float multiplier;
        public GameplayEffectPeriod period;
    }
}