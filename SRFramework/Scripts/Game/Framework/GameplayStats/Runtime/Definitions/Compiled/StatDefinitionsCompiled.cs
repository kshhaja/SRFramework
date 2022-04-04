using System.Collections.Generic;


namespace SlimeRPG.Framework.StatsSystem
{
    public class StatDefinitionsCompiled
    {
        private Dictionary<StatDefinitionCollection, List<StatDefinition>> compiled = new Dictionary<StatDefinitionCollection, List<StatDefinition>>();


        public List<StatDefinition> Get(StatDefinitionCollection id)
        {
            if (id == null) 
                return null;

            List<StatDefinition> definitions;
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

        public List<StatDefinition> GetDefaults()
        {
            var id = StatsSettings.Current.DefaultStats;
            if (id == null) 
                return null;

            List<StatDefinition> definitions;
            if (compiled.TryGetValue(id, out definitions))
                return definitions;

            definitions = id.GetDefinitions();

            compiled[id] = definitions;

            return definitions;
        }

        public static List<StatDefinition> GetDefinitions(StatDefinitionCollection id)
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

        public static List<StatDefinition> GetDefinitionDefaults()
        {
            var id = StatsSettings.Current.DefaultStats;
            if (id == null) 
                return null;

            return id.GetDefinitions();
        }
    }
}
