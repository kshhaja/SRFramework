using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Stats/Execution Collection")]
    public class GameplayExecutionCollection : ScriptableObject
    {
        public ExecutionCalculationBase calculationClass;
        public List<GameplayModifierInfo> calculationModifiers = new List<GameplayModifierInfo>();

        public void TryExecute(AbilitySystemComponent target, float index)
        {
            if (calculationClass == null)
                return;

            calculationClass.Execute(calculationModifiers, target, index, out var callbackData);
            callbackData.ApplyAdjustment(target.StatsContainer);
        }
    }
}
