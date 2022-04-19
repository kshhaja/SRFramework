using System;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [CreateAssetMenu(fileName = "ScalableMagnitude", menuName = "Gameplay/Ability/Magnitude")]
    public class ScalableMagnitude : GameplayMagnitudeBase
    {
        public ParticleSystem.MinMaxCurve value;

        public override float GetValue(float index)
        {
            return value.Evaluate(index);
        }

        public override (float, float) MinMax(float index)
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
    }
}