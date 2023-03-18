using System.Collections.Generic;


namespace SRFramework.Attribute
{
    public class AttributeDefinitionsCompiled
    {
        private Dictionary<AttributeDefinitionCollection, List<AttributeDefinition>> compiled = new Dictionary<AttributeDefinitionCollection, List<AttributeDefinition>>();


        public List<AttributeDefinition> Get(AttributeDefinitionCollection id)
        {
            if (id == null) 
                return null;

            List<AttributeDefinition> definitions;
            if (compiled.TryGetValue(id, out definitions))
                return definitions;

            definitions = id.GetDefinitions();

            var defaults = GetDefaults();
            if (defaults != null)
            {
                foreach (var statDefinition in defaults)
                {
                    if (definitions.Contains(statDefinition)) 
                        continue;

                    definitions.Add(statDefinition);
                }
            }

            compiled[id] = definitions;

            return definitions;
        }

        public List<AttributeDefinition> GetDefaults()
        {
            var id = AttributesSettings.Current.DefaultStats;
            if (id == null) 
                return null;

            List<AttributeDefinition> definitions;
            if (compiled.TryGetValue(id, out definitions))
                return definitions;

            definitions = id.GetDefinitions();

            compiled[id] = definitions;

            return definitions;
        }

        public static List<AttributeDefinition> GetDefinitions(AttributeDefinitionCollection id)
        {
            if (id == null) 
                return null;

            var definitions = id.GetDefinitions();

            var defaults = GetDefinitionDefaults();
            if (defaults == null) 
                return definitions;

            foreach (var statDefinition in defaults)
            {
                if (definitions.Contains(statDefinition)) 
                    continue;
                
                definitions.Add(statDefinition);
            }

            return definitions;
        }

        public static List<AttributeDefinition> GetDefinitionDefaults()
        {
            var id = AttributesSettings.Current.DefaultStats;
            if (id == null) 
                return null;

            return id.GetDefinitions();
        }
    }
}
