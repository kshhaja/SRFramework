using UnityEngine;
using System.Collections.Generic;


namespace SlimeRPG.Framework.StatsSystem
{
    [System.Serializable]
    public class StatDefinitionOverrideCollection
    {
        private Dictionary<StatDefinition, StatDefinitionOverride> overridesByDefinition;
        public List<StatDefinitionOverride> overrides;


        public Dictionary<StatDefinition, StatDefinitionOverride> OverridesByDefinition
        {
            get
            {
                if (overridesByDefinition == null)
                    Clean();

                return overridesByDefinition;
            }
        }

        public void Add(StatDefinitionOverride @override)
        {
            if (@override == null || Has(@override) || !@override.IsValid)
                return;

            overrides.Add(@override);
            OverridesByDefinition[@override.definition] = @override;
        }

        public StatDefinitionOverride Get(StatDefinitionOverride @override)
        {
            if (@override == null) 
                return null;

            return Get(@override.definition);
        }

        public StatDefinitionOverride Get(StatDefinition def)
        {
            if (def == null) 
                return null;

            StatDefinitionOverride o;
            OverridesByDefinition.TryGetValue(def, out o);

            return o;
        }

        public bool Has(StatDefinitionOverride @override)
        {
            if (@override == null)
            {
                return false;
            }

            return Has(@override.definition);
        }

        public bool Has(StatDefinition def)
        {
            if (def == null)
            {
                return false;
            }

            return OverridesByDefinition.ContainsKey(def);
        }
        
        public bool Remove(StatDefinition def)
        {
            if (def == null) 
                return false;

            var o = Get(def);
            if (o == null) 
                return false;

            overrides.Remove(o);
            overridesByDefinition.Remove(def);

            return true;
        }

        public void Clean()
        {
            overridesByDefinition = new Dictionary<StatDefinition, StatDefinitionOverride>();

            var cleanedOverrides = new List<StatDefinitionOverride>();
            foreach (var def in overrides)
            {
                if (def == null || !def.IsValid) 
                    continue;

                cleanedOverrides.Add(def);
                overridesByDefinition[def.definition] = def;
            }

            overrides = cleanedOverrides;
        }
    }
}
