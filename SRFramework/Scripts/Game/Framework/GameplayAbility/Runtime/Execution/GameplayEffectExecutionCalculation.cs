using SlimeRPG.Framework.StatsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public abstract class GameplayEffectExecutionCalculation : ScriptableObject
    {
        public abstract void Execute(AbilitySystemComponent source, AbilitySystemComponent target, out List<GameplayModifierInfo> modifiers);
    }
}