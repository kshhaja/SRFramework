using System;
using System.Collections.Generic;
using SRFramework.Attribute;


namespace SRFramework.Effect
{
    [Serializable]
    public class ApplicationInfo
    {
        public GameplayEffectModifierMagnitude chanceToApplyToTarget;
        public List<GameplayEffectCustomApplicationRequirement> applicationRequirements;
    }
}
