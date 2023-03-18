using System.Collections.Generic;


namespace SRFramework.Attribute
{
    public class AttributeRecordCollection
    {
        private Dictionary<string, AttributeRecord> recordsById = new Dictionary<string, AttributeRecord>();
        private Dictionary<AttributeDefinition, AttributeRecord> recordsByDefinition = new Dictionary<AttributeDefinition, AttributeRecord>();
        public List<AttributeRecord> records = new List<AttributeRecord>();


        public AttributeRecord Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            recordsById.TryGetValue(id, out var result);
            return result;
        }

        public AttributeRecord Get(AttributeDefinition definition)
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

        public bool Has(AttributeDefinition definition)
        {
            if (definition == null) 
                return false;
            
            return recordsByDefinition.ContainsKey(definition);
        }

        public void Set(AttributeRecord record)
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
