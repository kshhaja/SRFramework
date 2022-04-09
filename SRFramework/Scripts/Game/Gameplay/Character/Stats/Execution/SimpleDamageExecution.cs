using SlimeRPG.Framework.Ability;
using SlimeRPG.Framework.StatsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Gameplay.Character.Stats.Calculation
{
    public class SimpleDamageExecution : ExecutionCalculationBase
    {
        public override void Execute(List<StatAdjustment> executionModifiers, AbilitySystemComponent target, float index, out StatAdjustmentCollection executionCallbackData)
        {
            StatAdjustmentCollection newCollection = new StatAdjustmentCollection();
            // Adds all stat types whether it is damage or not. 
            // captured attribute function is not ready.
            float testDamage = 0f;
            executionModifiers.ForEach(x => testDamage += x.GetValue(index));
            
            // last registered modifier
            testDamage += target.StatsContainer.GetModifier(OperatorType.Subtract, "current_health", "damage");

            newCollection.adjustment.Add(StatAdjustment.CreateConstant(
                target.StatsContainer.GetDefinition("currentHealth"), 
                OperatorType.Subtract, 
                testDamage));

            executionCallbackData = newCollection;
        }
    }
}