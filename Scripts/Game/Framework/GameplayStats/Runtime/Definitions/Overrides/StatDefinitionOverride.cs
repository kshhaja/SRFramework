using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [System.Serializable]
    public class StatDefinitionOverride
    {
        [Tooltip("Definition the override will target")]
        public StatDefinition definition;

        [Tooltip("Value of the override")]
        public GameplayEffectModifierMagnitude value = new GameplayEffectModifierMagnitude();

        public bool IsValid => definition != null && value != null;
    }
}
