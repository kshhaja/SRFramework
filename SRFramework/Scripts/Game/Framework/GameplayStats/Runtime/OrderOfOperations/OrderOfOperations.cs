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
            new Operator(OperatorType.Add, true),
            new Operator(OperatorType.Subtract, true),
            new Operator(OperatorType.Multiply, false),
            new Operator(OperatorType.Divide, false)
        };
    }
}
