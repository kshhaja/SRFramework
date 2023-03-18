using System;
using SRFramework.Attribute;


namespace SRFramework.Effect
{
    [Serializable]
    public class GameplayModifierInfo
    {
        public AttributeDefinition definition;
        public GameplayModifierOperator operatorType;
        public GameplayEffectModifierMagnitude modifierMagnitude;

        public GameplayTagRequirements sourceTags;
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
}
