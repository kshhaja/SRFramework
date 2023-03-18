using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Attribute
{
    public abstract class AttributeDefinitionBase : ScriptableObject
    {
        public List<AttributeDefinition> GetDefinitions()
        {
            return GetDefinitions(new HashSet<AttributeDefinitionBase>());
        }

        public abstract List<AttributeDefinition> GetDefinitions(HashSet<AttributeDefinitionBase> visited);
    }
}
