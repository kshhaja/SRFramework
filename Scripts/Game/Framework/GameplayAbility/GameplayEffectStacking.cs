using System;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectStacking
    {
        [SerializeField] public StackingType stackingType;
        [SerializeField] public int stackLimitCount;
        [SerializeField] public StackingDurationPolicy stackDurationRefreshPolicy;
        [SerializeField] public StackingPeriodPolicy stackPeriodResetPolicy;
        [SerializeField] public StackingExpirationPolicy stackExpirationPolicy;
    }
}
