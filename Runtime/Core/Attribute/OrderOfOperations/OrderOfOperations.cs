using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [CreateAssetMenu(fileName = "OrderOfOperations", menuName = "Gameplay/Stats/Settings/Order Of Operations")]
    public class OrderOfOperations : ScriptableObject
    {
        [Tooltip("The order of operations")]
        [SerializeField]
        public List<Operator> operators = new List<Operator> {
            new Operator(GameplayModifierOperator.Add, true),
            new Operator(GameplayModifierOperator.Subtract, true),
            new Operator(GameplayModifierOperator.Multiply, false),
            new Operator(GameplayModifierOperator.Divide, false)
        };
    }
}
