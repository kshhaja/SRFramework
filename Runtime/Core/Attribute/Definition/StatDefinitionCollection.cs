﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "StatDefinitionCollection", menuName = "Gameplay/Stats/Definitions/Collection")]
    public class StatDefinitionCollection : AttributeDefinitionBase
    {
        [Tooltip("List of definitions and definition collections")]
        public List<AttributeDefinitionBase> definitions = new List<AttributeDefinitionBase>();


        public override List<StatDefinition> GetDefinitions(HashSet<AttributeDefinitionBase> visited)
        {
            if (visited == null || visited.Contains(this))
            {
                if (Application.isPlaying)
                    Debug.LogWarningFormat("Duplicate StatDefinitionCollection detected {0}", name);

                return new List<StatDefinition>();
            }

            visited.Add(this);

            return definitions.SelectMany(d => {
                if (d == null) 
                    return new List<StatDefinition>();
                
                return d.GetDefinitions(visited);
            }).ToList();
        }

        // @TODO Helper method for getting all compiled StatsRecord objects from compiled definitions
    }
}