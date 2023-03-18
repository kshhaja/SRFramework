using System;
using UnityEngine;


namespace SRFramework.Attribute
{
    [Serializable]
    public class GameplayEffectModifierMagnitude
    {
        public enum MagnitudeCalculation
        {
            scalableFloat,
            attributeBased,
            customCalculationClass,
            setByCaller,
        }

        [SerializeField] protected MagnitudeCalculation magnitudeCalculation;
        [SerializeField] protected bool roundToInt = false;
        [SerializeField] protected float multiplier = 1f;
        [SerializeField] protected GameplayMagnitudeBase magnitude;


        public bool RoundToInt => roundToInt;
        
        
        public float GetValue(float index)
        {
            var value = magnitude.GetValue(index);
            if (roundToInt)
                return Mathf.RoundToInt(value);

            return value;
        }

        public (float, float) MinMax(float index)
        {
            var value = magnitude.MinMax(index);
            if (roundToInt)
                return (Mathf.RoundToInt(value.Item1), Mathf.RoundToInt(value.Item2));

            return value;
        }
    }
}
