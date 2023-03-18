using System;
using SlimeRPG.Framework.Tag;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct ConditionalGameplayEffectContainer
    {
        public GameplayEffect GameplayEffect;
        public GameplayTagContainer RequiredSourceTags;
    }
}
