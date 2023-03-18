/*
 * @SlimeRPG Allrights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    public class SlimeRPGDamageExecution : GameplayEffectExecutionCalculation
    {
        public override void Execute(AbilitySystemComponent source, AbilitySystemComponent target, out List<GameplayModifierInfo> modifiers)
        {
            // Fill your own formulas.
            modifiers = null;
        }
    }
}