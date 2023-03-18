using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SRFramework.Attribute.StatsContainers
{
    [CreateAssetMenu(fileName = "StatsContainer", menuName = "Gameplay/Stats/Container")]
    public class AttributeSet : ScriptableObject, AttributeSetBase
    {
        private class StatRecordEvent : UnityEvent<AttributeRecord> { }
        private Dictionary<AttributeRecord, StatRecordEvent> events = new Dictionary<AttributeRecord, StatRecordEvent>();



        [Tooltip("This collection will be combined with the default stats from settings")]
        public AttributeDefinitionCollection collection;
        
        public AttributeDefinitionOverrideCollection overrides;
        public AttributeRecordCollection records = new AttributeRecordCollection();

        public bool IsSetup { get; private set; }


        public void Setup()
        {
            if (IsSetup)
                return;

            List<AttributeDefinition> definitions;

            if (collection == null)
                definitions = AttributesSettings.Current.DefinitionsCompiled.GetDefaults();
            else
                definitions = AttributesSettings.Current.DefinitionsCompiled.Get(collection);

            if (definitions == null)
            {
                Debug.Assert(!Application.isPlaying, "IStatsContainer.Setup works only runtime.");
                return;
            }

            foreach (var statDefinition in definitions)
            {
                var @override = overrides.Get(statDefinition);
                var val = @override == null ? null : @override.value;

                records.Set(new AttributeRecord(statDefinition, val));
            }

            foreach (var definition in definitions)
                SetupDefinition(definition);

            IsSetup = true;
        }

        public void SetupDefinition(AttributeDefinition definition)
        {
            // for DerivedStatDefinition and RuntimeStatDefinition
            definition.Setup(this);
        }

        public AttributeRecord GetRecord(string definitionId)
        {
            return records.Get(definitionId);
        }

        public AttributeRecord GetRecord(AttributeDefinition definition)
        {
            return records.Get(definition);
        }

        public bool HasRecord(string definitionId)
        {
            return records.Has(definitionId);
        }

        public bool HasRecord(AttributeDefinition definition)
        {
            return records.Has(definition);
        }

        public float GetStat(AttributeRecord record, float index = 0)
        {
            if (record == null) 
                return 0;

            return record.GetValue(index);
        }

        public float GetStat(AttributeDefinition definition, float index = 0)
        {
            return GetStat(GetRecord(definition), index);
        }

        public float GetStat(string definitionId, float index = 0)
        {
            return GetStat(GetRecord(definitionId), index);
        }

        public AttributeDefinition GetDefinition(string definitionID)
        {
            return GetRecord(definitionID).Definition;
        }

        public void SetStat(AttributeDefinition definition, GameplayEffectModifierMagnitude value = null)
        {
            records.Set(new AttributeRecord(definition, value));
        }

        public int GetStatInt(AttributeRecord record, float index = 0)
        {
            if (record == null) 
                return 0;

            return Mathf.RoundToInt(record.GetValue(index));
        }

        public int GetStatInt(AttributeDefinition definition, float index = 0)
        {
            return GetStatInt(GetRecord(definition), index);
        }

        public int GetStatInt(string definitionId, float index = 0)
        {
            return GetStatInt(GetRecord(definitionId), index);
        }

        public void SetModifier(GameplayModifierOperator operation, AttributeRecord record, string modifierId, float value)
        {
            if (record == null) 
                return;

            record.GetModifier(operation).Set(modifierId, value);
            TriggerEvent(record);
        }

        public void SetModifier(GameplayModifierOperator operation, AttributeDefinition definition, string modifierId, float value)
        {
            SetModifier(operation, GetRecord(definition), modifierId, value);
        }

        public void SetModifier(GameplayModifierOperator operation, string definitionId, string modifierId, float value)
        {
            SetModifier(operation, GetRecord(definitionId), modifierId, value);
        }

        public float GetModifier(GameplayModifierOperator operation, AttributeRecord record, string modifierId)
        {
            if (record == null) 
                return 0;

            var modifier = record.GetModifier(operation).Get(modifierId);
            if (modifier == null) 
                return 0;

            return modifier.value;
        }

        public float GetModifier(GameplayModifierOperator operation, AttributeDefinition definition, string modifierId)
        {
            return GetModifier(operation, GetRecord(definition), modifierId);
        }

        public float GetModifier(GameplayModifierOperator operation, string definitionId, string modifierId)
        {
            return GetModifier(operation, GetRecord(definitionId), modifierId);
        }

        public bool RemoveModifier(GameplayModifierOperator operation, AttributeRecord record, string modifierId)
        {
            if (record == null) 
                return false;

            var result = record.GetModifier(operation).Remove(modifierId);
            TriggerEvent(record);

            return result;
        }

        public bool RemoveModifier(GameplayModifierOperator operation, AttributeDefinition definition, string modifierId)
        {
            return RemoveModifier(operation, GetRecord(definition), modifierId);
        }

        public bool RemoveModifier(GameplayModifierOperator operation, string definitionId, string modifierId)
        {
            return RemoveModifier(operation, GetRecord(definitionId), modifierId);
        }

        public void ClearAllModifiers(AttributeRecord record, string modifierId)
        {
            RemoveModifier(GameplayModifierOperator.Add, record, modifierId);
            RemoveModifier(GameplayModifierOperator.Subtract, record, modifierId);
            RemoveModifier(GameplayModifierOperator.Multiply, record, modifierId);
            RemoveModifier(GameplayModifierOperator.Divide, record, modifierId);

            TriggerEvent(record);
        }

        public void ClearAllModifiers(string modifierId)
        {
            records.records.ForEach(r => ClearAllModifiers(r, modifierId));
        }

        public void ClearAllModifiers(List<string> modifierIds)
        {
            if (modifierIds == null) 
                return;
            
            modifierIds.ForEach(m => ClearAllModifiers(m));
        }

        public AttributeSet CreateRuntimeCopy()
        {
            var copy = Instantiate(this);
            copy.Setup();

            return copy;
        }

        private void TriggerEvent(AttributeRecord record)
        {
            if (record != null && events.TryGetValue(record, out var @event))
                @event.Invoke(record);
        }

        public void OnStatChangeSubscribe(AttributeRecord record, UnityAction<AttributeRecord> callback)
        {
            if (!events.TryGetValue(record, out var @event))
            {
                @event = new StatRecordEvent();
                events[record] = @event;
            }

            @event.AddListener(callback);
        }

        public void OnStatChangeSubscribe(AttributeDefinition definition, UnityAction<AttributeRecord> callback)
        {
            var record = GetRecord(definition);
            OnStatChangeSubscribe(record, callback);
        }

        public void OnStatChangeSubscribe(string definitionId, UnityAction<AttributeRecord> callback)
        {
            var record = GetRecord(definitionId);
            OnStatChangeSubscribe(record, callback);
        }

        public void OnStatChangeUnsubscribe(AttributeRecord record, UnityAction<AttributeRecord> callback)
        {
            events[record].RemoveListener(callback);
        }

        public void OnStatChangeUnsubscribe(AttributeDefinition definition, UnityAction<AttributeRecord> callback)
        {
            var record = GetRecord(definition);
            OnStatChangeUnsubscribe(record, callback);
        }

        public void OnStatChangeUnsubscribe(string definitionId, UnityAction<AttributeRecord> callback)
        {
            var record = GetRecord(definitionId);
            OnStatChangeUnsubscribe(record, callback);
        }
    }
}
