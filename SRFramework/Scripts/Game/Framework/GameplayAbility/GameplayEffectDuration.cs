using System;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectDuration
    {
        public EDurationPolicy policy;
        public float modifier;
        public float multiplier;
    }
}