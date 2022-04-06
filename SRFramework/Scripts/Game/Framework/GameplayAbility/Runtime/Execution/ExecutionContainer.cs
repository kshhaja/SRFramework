using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class ExecutionContainer : ScriptableObject
    {
        public ExecutionCalculationBase calculationClass;
        public List<GameplayExecution> calculationModifiers = new List<GameplayExecution>();

        public void Execute(AbilitySystemCharacter target)
        {
            calculationClass?.Execute(calculationModifiers, target);
        }
    }
}
