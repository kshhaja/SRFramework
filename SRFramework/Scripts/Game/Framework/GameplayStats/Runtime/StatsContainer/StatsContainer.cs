using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SlimeRPG.Framework.StatsSystem.StatsContainers
{
    [CreateAssetMenu(fileName = "StatsContainer", menuName = "Gameplay/Stats/Container")]
    public class StatsContainer : ScriptableObject, IStatsContainer
    {
        private class StatRecordEvent : UnityEvent<StatRecord> { }
        private Dictionary<StatRecord, StatRecordEvent> events = new Dictionary<StatRecord, StatRecordEvent>();



        [Tooltip("This collection will be combined with the default stats from settings")]
        public StatDefinitionCollection collection;
        
        public StatDefinitionOverrideCollection overrides;
        public StatRecordCollection records = new StatRecordCollection();

        public bool IsSetup { get; private set; }


        public void Setup()
        {
            if (IsSetup)
                return;

            List<StatDefinition> definitions;

            if (collection == null)
                definitions = StatsSettings.Current.DefinitionsCompiled.GetDefaults();
            else
                definitions = StatsSettings.Current.DefinitionsCompiled.Get(collection);

            if (definitions == null)
            {
                Debug.Assert(!Application.isPlaying, "IStatsContainer.Setup works only runtime.");
                return;
            }

            foreach (var statDefinition in definitions)
            {
                var @override = overrides.Get(statDefinition);
                var val = @override == null ? null : @override.value;

                records.Set(new StatRecord(statDefinition, val));
            }

            foreach (var definition in definitions)
                SetupDefinition(definition);

            IsSetup = true;
        }

        public void SetupDefinition(StatDefinition definition)
        {
            // for DerivedStatDefinition and RuntimeStatDefinition
            definition.Setup(this);
        }

        public StatRecord GetRecord(string definitionId)
        {
            return records.Get(definitionId);
        }

        public StatRecord GetRecord(StatDefinition definition)
        {
            return records.Get(definition);
        }

        public bool HasRecord(string definitionId)
        {
            return records.Has(definitionId);
        }

        public bool HasRecord(StatDefinition definition)
        {
            return records.Has(definition);
        }

        public float GetStat(StatRecord record, float index = 0)
        {
            if (record == null) 
                return 0;

            return record.GetValue(index);
        }

        public float GetStat(StatDefinition definition, float index = 0)
        {
            return GetStat(GetRecord(definition), index);
        }

        public float GetStat(string definitionId, float index = 0)
        {
            return GetStat(GetRecord(definitionId), index);
        }

        public StatDefinition GetDefinition(string definitionID)
        {
            return GetRecord(definitionID).Definition;
        }

        public void SetStat(StatDefinition definition, GameplayEffectModifierMagnitude value = null)
        {
            records.Set(new StatRecord(definition, value));
        }

        public int GetStatInt(StatRecord record, float index = 0)
        {
            if (record == null) 
                return 0;

            return Mathf.RoundToInt(record.GetValue(index));
        }

        public int GetStatInt(StatDefinition definition, float index = 0)
        {
            return GetStatInt(GetRecord(definition), index);
        }

        public int GetStatInt(string definitionId, float index = 0)
        {
            return GetStatInt(GetRecord(definitionId), index);
        }

        public void SetModifier(GameplayModifierOperator operation, StatRecord record, string modifierId, float value)
        {
            if (record == null) 
                return;

            record.GetModifier(operation).Set(modifierId, value);
            TriggerEvent(record);
        }

        public void SetModifier(GameplayModifierOperator operation, StatDefinition definition, string modifierId, float value)
        {
            SetModifier(operation, GetRecord(definition), modifierId, value);
        }

        public void SetModifier(GameplayModifierOperator operation, string definitionId, string modifierId, float value)
        {
            SetModifier(operation, GetRecord(definitionId), modifierId, value);
        }

        public float GetModifier(GameplayModifierOperator operation, StatRecord record, string modifierId)
        {
            if (record == null) 
                return 0;

            var modifier = record.GetModifier(operation).Get(modifierId);
            if (modifier == null) 
                return 0;

            return modifier.value;
        }

        public float GetModifier(GameplayModifierOperator operation, StatDefinition definition, string modifierId)
        {
            return GetModifier(operation, GetRecord(definition), modifierId);
        }

        public float GetModifier(GameplayModifierOperator operation, string definitionId, string modifierId)
        {
            return GetModifier(operation, GetRecord(definitionId), modifierId);
        }

        public bool RemoveModifier(GameplayModifierOperator operation, StatRecord record, string modifierId)
        {
            if (record == null) 
                return false;

            var result = record.GetModifier(operation).Remove(modifierId);
            TriggerEvent(record);

            return result;
        }

        public bool RemoveModifier(GameplayModifierOperator operation, StatDefinition definition, string modifierId)
        {
            return RemoveModifier(operation, GetRecord(definition), modifierId);
        }

        public bool RemoveModifier(GameplayModifierOperator operation, string definitionId, string modifierId)
        {
            return RemoveModifier(operation, GetRecord(definitionId), modifierId);
        }

        public void ClearAllModifiers(StatRecord record, string modifierId)
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

        public StatsContainer CreateRuntimeCopy()
        {
            var copy = Instantiate(this);
            copy.Setup();

            return copy;
        }

        private void TriggerEvent(StatRecord record)
        {
            if (record != null && events.TryGetValue(record, out var @event))
                @event.Invoke(record);
        }

        public void OnStatChangeSubscribe(StatRecord record, UnityAction<StatRecord> callback)
        {
            if (!events.TryGetValue(record, out var @event))
            {
                @event = new StatRecordEvent();
                events[record] = @event;
            }

            @event.AddListener(callback);
        }

        public void OnStatChangeSubscribe(StatDefinition definition, UnityAction<StatRecord> callback)
        {
            var record = GetRecord(definition);
            OnStatChangeSubscribe(record, callback);
        }

        public void OnStatChangeSubscribe(string definitionId, UnityAction<StatRecord> callback)
        {
            var record = GetRecord(definitionId);
            OnStatChangeSubscribe(record, callback);
        }

        public void OnStatChangeUnsubscribe(StatRecord record, UnityAction<StatRecord> callback)
        {
            events[record].RemoveListener(callback);
        }

        public void OnStatChangeUnsubscribe(StatDefinition definition, UnityAction<StatRecord> callback)
        {
            var record = GetRecord(definition);
            OnStatChangeUnsubscribe(record, callback);
        }

        public void OnStatChangeUnsubscribe(string definitionId, UnityAction<StatRecord> callback)
        {
            var record = GetRecord(definitionId);
            OnStatChangeUnsubscribe(record, callback);
        }
    }
}
