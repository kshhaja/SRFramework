using System.Collections.Generic;
using UnityEngine.Events;


namespace SlimeRPG.Framework.StatsSystem.StatsContainers
{
    public interface IStatsContainer
    {
        StatsContainer CreateRuntimeCopy();

        StatRecord GetRecord(string definitionId);
        StatRecord GetRecord(StatDefinition definition);

        bool HasRecord(string definitionId);
        bool HasRecord(StatDefinition definition);

        float GetStat(StatRecord record, float index = 0);
        float GetStat(StatDefinition definition, float index = 0);
        float GetStat(string definitionId, float index = 0);

        int GetStatInt(StatRecord record, float index = 0);
        int GetStatInt(StatDefinition definition, float index = 0);
        int GetStatInt(string definitionId, float index = 0);

        void SetModifier(GameplayModifierOperator operation, StatRecord record, string modifierId, float value);
        void SetModifier(GameplayModifierOperator operation, StatDefinition definition, string modifierId, float value);
        void SetModifier(GameplayModifierOperator operation, string definitionId, string modifierId, float value);

        float GetModifier(GameplayModifierOperator operation, string definitionId, string modifierId);
        float GetModifier(GameplayModifierOperator operation, StatDefinition definition, string modifierId);

        bool RemoveModifier(GameplayModifierOperator operation, StatRecord record, string modifierId);
        bool RemoveModifier(GameplayModifierOperator operation, StatDefinition definition, string modifierId);
        bool RemoveModifier(GameplayModifierOperator operation, string definitionId, string modifierId);

        void ClearAllModifiers(StatRecord record, string modifierId);
        void ClearAllModifiers(string modifierId);
        void ClearAllModifiers(List<string> modifierIds);

        void OnStatChangeSubscribe(StatDefinition definition, UnityAction<StatRecord> callback);
        void OnStatChangeSubscribe(string definitionId, UnityAction<StatRecord> callback);
        void OnStatChangeSubscribe(StatRecord record, UnityAction<StatRecord> callback);

        void OnStatChangeUnsubscribe(StatRecord record, UnityAction<StatRecord> callback);
        void OnStatChangeUnsubscribe(StatDefinition definition, UnityAction<StatRecord> callback);
        void OnStatChangeUnsubscribe(string definitionId, UnityAction<StatRecord> callback);
    }
}
