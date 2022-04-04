using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [System.Serializable]
    public class Operator
    {
        [Tooltip("Type of operations")]
        [SerializeField]
        public OperatorType type;

        [Tooltip("Automatically round this operation as a modifier if `forceRound` is enabled on the definition")]
        [SerializeField]
        public bool modifierAutoRound;


        public Operator(OperatorType type, bool modifierAutoRound)
        {
            this.type = type;
            this.modifierAutoRound = modifierAutoRound;
        }
    }
}
