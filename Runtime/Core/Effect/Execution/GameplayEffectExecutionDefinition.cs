using SRFramework.Attribute;
using System;
using System.Collections.Generic;


namespace SRFramework.Effect
{
    [Serializable]
    public class GameplayEffectExecutionDefinition
    {
        public GameplayEffectExecutionCalculation calculationClass;
        public List<GameplayModifierInfo> calculationModifiers;
        public List<ConditionalGameplayEffect> conditionalGameplayEffects;
    }
}
