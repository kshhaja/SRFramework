using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.Tag;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public class GameplayEffectExecutionDefinition
    {
        public GameplayEffectExecutionCalculation calculationClass;
        public List<GameplayModifierInfo> calculationModifiers;
        public List<ConditionalGameplayEffect> conditionalGameplayEffects;
    }
}
