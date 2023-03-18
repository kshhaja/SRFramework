using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Attribute
{
    [System.Serializable]
    public class AttributeDefinitionOverride
    {
        [Tooltip("Definition the override will target")]
        public AttributeDefinition definition;

        [Tooltip("Value of the override")]
        public GameplayEffectModifierMagnitude value = new GameplayEffectModifierMagnitude();

        public bool IsValid => definition != null && value != null;
    }
}
