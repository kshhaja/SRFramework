using System;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [Serializable]
    public class StatValueSelector
    {
        /// <summary>
        /// Deals 9 to 14 Fire Damage (= poe Fireball level 1 skill gem stat)
        /// MinMaxCurve를 사용할 수 있다면 위와 같은 스탯에 대해
        /// 데이터 작업에 대한 복잡도를 줄이고, 생산성을 높일 수 있을 것으로 예상됨.
        /// </summary>
        
        [SerializeField, Tooltip("default value type is float")]
        protected bool roundToInt = false;

        [SerializeField]
        public ParticleSystem.MinMaxCurve value;


        public bool RoundToInt => roundToInt;

        
        public float Evaluate(float index)
        {
            return value.Evaluate(index);
        }

        public int EvaluateInt(float index)
        {
            return Mathf.RoundToInt(Evaluate(index));
        }

        /// <summary>
        /// 최소 최대값 순서로 튜플 반환을 해준다.
        /// Constant경우 최소 최대값은 같다.
        /// </summary>
        public (float, float) MinMax(float index)
        {
            switch (value.mode)
            {
                case ParticleSystemCurveMode.Constant:
                    var c = value.constant;
                    return (c, c);
                case ParticleSystemCurveMode.TwoConstants:
                    return (value.constantMin, value.constantMax);
                case ParticleSystemCurveMode.Curve:
                    var v = value.curve.Evaluate(index);
                    return (v, v);
                case ParticleSystemCurveMode.TwoCurves:
                    return (value.curveMin.Evaluate(index), value.curveMax.Evaluate(index));
                default:
                    throw new ArgumentOutOfRangeException("StatValueSelector.value mode error");
            }
        }

        public (int, int) MinMaxInt(float index)
        {
            var minmax = MinMax(index);
            return (Mathf.RoundToInt(minmax.Item1), Mathf.RoundToInt(minmax.Item2));
        }
    }
}
