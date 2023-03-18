using System.Collections.Generic;


namespace SRFramework.Attribute
{
    [System.Serializable]
    public class AttributeDefinitionOverrideCollection
    {
        private Dictionary<AttributeDefinition, AttributeDefinitionOverride> overridesByDefinition;
        public List<AttributeDefinitionOverride> overrides;


        public Dictionary<AttributeDefinition, AttributeDefinitionOverride> OverridesByDefinition
        {
            get
            {
                if (overridesByDefinition == null)
                    Clean();

                return overridesByDefinition;
            }
        }

        public void Add(AttributeDefinitionOverride @override)
        {
            if (@override == null || Has(@override) || !@override.IsValid)
                return;

            overrides.Add(@override);
            OverridesByDefinition[@override.definition] = @override;
        }

        public AttributeDefinitionOverride Get(AttributeDefinitionOverride @override)
        {
            if (@override == null) 
                return null;

            return Get(@override.definition);
        }

        public AttributeDefinitionOverride Get(AttributeDefinition def)
        {
            if (def == null) 
                return null;

            AttributeDefinitionOverride o;
            OverridesByDefinition.TryGetValue(def, out o);

            return o;
        }

        public bool Has(AttributeDefinitionOverride @override)
        {
            if (@override == null)
            {
                return false;
            }

            return Has(@override.definition);
        }

        public bool Has(AttributeDefinition def)
        {
            if (def == null)
            {
                return false;
            }

            return OverridesByDefinition.ContainsKey(def);
        }
        
        public bool Remove(AttributeDefinition def)
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
            overridesByDefinition = new Dictionary<AttributeDefinition, AttributeDefinitionOverride>();

            var cleanedOverrides = new List<AttributeDefinitionOverride>();
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
