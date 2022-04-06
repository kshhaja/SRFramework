using System;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectDuration
    {
        public Duration policy;
        // modifier도 Evaluate가능하게 변경해야함.
        public float modifier;
        public float multiplier;
    }
}