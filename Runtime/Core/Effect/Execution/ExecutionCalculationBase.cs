using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    public abstract class ExecutionCalculationBase : ScriptableObject
    {
        public abstract void Execute(List<GameplayModifierInfo> executionModifiers, AbilitySystemComponent target, float index, out StatAdjustmentCollection executionCallbackData);
    }
}
