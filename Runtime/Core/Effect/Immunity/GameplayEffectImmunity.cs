using System;
namespace SRFramework.Effect
{
    [Serializable]
    public struct GameplayEffectImmunity
    {
        public GameplayTagRequirements grantedApplicationImmunityTags;
        public GameplayEffectQuery grantedApplicationImmunityQuery;
    }
}
