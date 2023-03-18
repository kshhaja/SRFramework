using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SRFramework.Attribute;


namespace SRFramework.Effect
{
    public abstract class ExecutionCalculationBase : ScriptableObject
    {
        public abstract void Execute(List<GameplayModifierInfo> executionModifiers, AbilitySystemComponent target, float index, out AttributeAdjustmentCollection executionCallbackData);
    }
}
