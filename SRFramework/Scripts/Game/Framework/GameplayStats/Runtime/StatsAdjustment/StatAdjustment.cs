using System;
using UnityEngine;

namespace SlimeRPG.Framework.StatsSystem
{
    [Serializable]
    public class StatAdjustment
    {
        public StatDefinition definition;
        public StatValueSelector value = new StatValueSelector();
        public OperatorType operatorType;

        public bool IsValid => definition != null && value != null;


        public float GetValue(float index)
        {
            if (!IsValid) 
                return 0;

            if (value.RoundToInt)
                return value.EvaluateInt(index);
            
            return value.Evaluate(index);
        }

        public (float, float)? MinMax(float index)
        {
            if (IsValid == false)
                return null;

            if (value.RoundToInt)
                return value.MinMaxInt(index);

            return value.MinMax(index);
        }

        public static StatAdjustment CreateConstant(StatDefinition definition, OperatorType type, float value)
        {
            var stat = new StatAdjustment();
            stat.definition = definition;
            stat.operatorType = type;
            stat.value.value.mode = ParticleSystemCurveMode.Constant;
            stat.value.value.constant = value;

            return stat;
        }
    }
}
