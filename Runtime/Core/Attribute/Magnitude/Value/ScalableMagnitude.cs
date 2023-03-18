using System;
using UnityEngine;


namespace SRFramework.Attribute
{
    [CreateAssetMenu(fileName = "ScalableMagnitude", menuName = "Gameplay/Ability/Magnitude")]
    [Serializable]
    public class ScalableMagnitude : GameplayMagnitudeBase
    {
        [SerializeField] public ParticleSystem.MinMaxCurve curve;

        public override float GetValue(float index)
        {
            return curve.Evaluate(index);
        }

        public override (float, float) MinMax(float index)
        {
            switch (curve.mode)
            {
                case ParticleSystemCurveMode.Constant:
                    var c = curve.constant;
                    return (c, c);
                case ParticleSystemCurveMode.TwoConstants:
                    return (curve.constantMin, curve.constantMax);
                case ParticleSystemCurveMode.Curve:
                    var v = curve.curve.Evaluate(index);
                    return (v, v);
                case ParticleSystemCurveMode.TwoCurves:
                    return (curve.curveMin.Evaluate(index), curve.curveMax.Evaluate(index));
                default:
                    throw new ArgumentOutOfRangeException("StatValueSelector.value mode error");
            }
        }
    }
}