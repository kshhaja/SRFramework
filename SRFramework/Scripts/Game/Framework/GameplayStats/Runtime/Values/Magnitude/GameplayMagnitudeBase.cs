using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [Serializable]
    public abstract class GameplayMagnitudeBase : ScriptableObject
    {
        public abstract float GetValue(float index);

        public abstract (float, float) MinMax(float index);
    }
}
