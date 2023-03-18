using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    public abstract class AttributeDefinitionBase : ScriptableObject
    {
        public List<StatDefinition> GetDefinitions()
        {
            return GetDefinitions(new HashSet<AttributeDefinitionBase>());
        }

        public abstract List<StatDefinition> GetDefinitions(HashSet<AttributeDefinitionBase> visited);
    }
}
