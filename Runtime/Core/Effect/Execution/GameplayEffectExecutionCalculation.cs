using SRFramework.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Effect
{
    public abstract class GameplayEffectExecutionCalculation : ScriptableObject
    {
        public abstract void Execute(AbilitySystemComponent source, AbilitySystemComponent target, out List<GameplayModifierInfo> modifiers);
    }
}