using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [System.Serializable]
    public class StatDefinitionOverride
    {
        [Tooltip("Definition the override will target")]
        [SerializeField]
        public StatDefinition definition;

        [Tooltip("Value of the override")]
        [SerializeField]
        public GameplayEffectModifierMagnitude value = new GameplayEffectModifierMagnitude();

        public bool IsValid => definition != null && value != null;
    }
}
