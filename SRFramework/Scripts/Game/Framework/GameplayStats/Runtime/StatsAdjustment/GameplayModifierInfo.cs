using SlimeRPG.Framework.Tag;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace SlimeRPG.Framework.StatsSystem
{
    [Serializable]
    public class GameplayModifierInfo
    {
        public StatDefinition definition;
        public GameplayModifierOperator operatorType;
        public GameplayEffectModifierMagnitude modifierMagnitude;

        [SerializeField] public GameplayTagRequirements sourceTags;
        public GameplayTagRequirements targetTags;

        public bool IsValid => definition != null && modifierMagnitude != null;


        public float GetValue(float index, float? @default=null)
        {
            if (IsValid == false)
            {
                if (@default != null)
                {
                    return @default.Value;
                }
            }

            return modifierMagnitude.GetValue(index);
        }

        public (float, float) MinMax(float index, (float, float)? @default=null)
        {
            if (IsValid == false)
            {
                if (@default != null)
                {
                    return @default.Value;
                }
            }

            return modifierMagnitude.MinMax(index);
        }
    }

    [Serializable]
    public struct GameplayTagRequirements
    {
        public GameplayTagContainer requireTags;
        public GameplayTagContainer ignoreTags;
    }
}
