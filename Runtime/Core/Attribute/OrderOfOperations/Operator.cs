using UnityEngine;


namespace SRFramework.Attribute
{
    [System.Serializable]
    public class Operator
    {
        [Tooltip("Type of operations")]
        [SerializeField]
        public GameplayModifierOperator type;

        [Tooltip("Automatically round this operation as a modifier if `forceRound` is enabled on the definition")]
        [SerializeField]
        public bool modifierAutoRound;


        public Operator(GameplayModifierOperator type, bool modifierAutoRound)
        {
            this.type = type;
            this.modifierAutoRound = modifierAutoRound;
        }
    }
}
