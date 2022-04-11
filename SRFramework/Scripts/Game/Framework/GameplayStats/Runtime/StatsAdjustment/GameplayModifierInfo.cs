using SlimeRPG.Framework.Tag;
using System;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [Serializable]
    public class GameplayModifierInfo
    {
        public StatDefinition definition;
        public GameplayModifierOperator operatorType;
        public GameplayEffectModifierMagnitude modifierMagnitude;

        public GameplayTagRequirements sourceTags;
        public GameplayTagRequirements targetTags;

        public bool IsValid => definition != null && modifierMagnitude != null;


        public float GetValue(float index)
        {
            if (IsValid == false) 
                return 0;

            if (modifierMagnitude.RoundToInt)
                return modifierMagnitude.EvaluateInt(index);
            
            return modifierMagnitude.Evaluate(index);
        }

        public (float, float)? MinMax(float index)
        {
            if (IsValid == false)
                return null;

            if (modifierMagnitude.RoundToInt)
                return modifierMagnitude.MinMaxInt(index);

            return modifierMagnitude.MinMax(index);
        }
    }

    // temp
    public struct GameplayTagRequirements
    {
        public GameplayTagContainer requireTags;
        public GameplayTagContainer ignoreTags;
    }
}
