using System;


namespace SlimeRPG.Framework.StatsSystem
{
    [Serializable]
    public class ModifierGroup
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
    }
}
