using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    public struct GameplayEffectExecutionModifierInfo
    {
        // definition to CapturedAttribute
        public StatDefinition definition;
        public GameplayModifierOperator operatorType;

        public GameplayEffectModifierMagnitude modifierMagnitude;
        public GameplayTagRequirements sourceTags;
        public GameplayTagRequirements targetTags;
    }
}
