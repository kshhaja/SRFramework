using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Stats/Execution Collection")]
    public class GameplayExecutionCollection : ScriptableObject
    {
        public ExecutionCalculationBase calculationClass;
        public List<StatAdjustment> calculationModifiers = new List<StatAdjustment>();

        public void Execute(AbilitySystemCharacter target)
        {
            calculationClass?.Execute(calculationModifiers, target);
        }
    }
}
