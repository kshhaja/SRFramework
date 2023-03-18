using SRFramework.Attribute;
using System;


namespace SRFramework.Effect
{
    [Serializable]
    public struct GameplayEffectDuration
    {
        public Duration policy;
        public GameplayEffectModifierMagnitude magnitude;
    }
}