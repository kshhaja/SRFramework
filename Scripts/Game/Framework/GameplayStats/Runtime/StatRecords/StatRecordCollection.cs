using System.Collections.Generic;


namespace SlimeRPG.Framework.StatsSystem
{
    public class StatRecordCollection
    {
        private Dictionary<string, StatRecord> recordsById = new Dictionary<string, StatRecord>();
        private Dictionary<StatDefinition, StatRecord> recordsByDefinition = new Dictionary<StatDefinition, StatRecord>();
        public List<StatRecord> records = new List<StatRecord>();


        public StatRecord Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            recordsById.TryGetValue(id, out var result);
            return result;
        }

        public StatRecord Get(StatDefinition definition)
        {
            if (definition == null) 
                return null;

            recordsByDefinition.TryGetValue(definition, out var result);
            return result;
        }

        public bool Has(string id)
        {
            if (string.IsNullOrEmpty(id)) 
                return false;
            
            return recordsById.ContainsKey(id);
        }

        public bool Has(StatDefinition definition)
        {
            if (definition == null) 
                return false;
            
            return recordsByDefinition.ContainsKey(definition);
        }

        public void Set(StatRecord record)
        {
            if (record == null || record.Definition == null || Has(record.Definition))
                return;

            if (!string.IsNullOrEmpty(record.Definition.Id))
                recordsById[record.Definition.Id] = record;

            recordsByDefinition[record.Definition] = record;
            records.Add(record);
        }
    }
}
