using System;
using UnityEngine;


namespace SRFramework.Effect
{
    [Serializable]
    public struct GameplayEffectStacking
    {
        public StackingType stackingType;
        public int stackLimitCount;
        public StackingDurationPolicy stackDurationRefreshPolicy;
        public StackingPeriodPolicy stackPeriodResetPolicy;
        public StackingExpirationPolicy stackExpirationPolicy;
    }
}
