using System.Collections.Generic;
using UnityEngine.Events;


namespace SRFramework.Attribute.StatsContainers
{
    public interface AttributeSetBase
    {
        AttributeSet CreateRuntimeCopy();

        AttributeRecord GetRecord(string definitionId);
        AttributeRecord GetRecord(AttributeDefinition definition);

        bool HasRecord(string definitionId);
        bool HasRecord(AttributeDefinition definition);

        float GetStat(AttributeRecord record, float index = 0);
        float GetStat(AttributeDefinition definition, float index = 0);
        float GetStat(string definitionId, float index = 0);

        int GetStatInt(AttributeRecord record, float index = 0);
        int GetStatInt(AttributeDefinition definition, float index = 0);
        int GetStatInt(string definitionId, float index = 0);

        void SetModifier(GameplayModifierOperator operation, AttributeRecord record, string modifierId, float value);
        void SetModifier(GameplayModifierOperator operation, AttributeDefinition definition, string modifierId, float value);
        void SetModifier(GameplayModifierOperator operation, string definitionId, string modifierId, float value);

        float GetModifier(GameplayModifierOperator operation, string definitionId, string modifierId);
        float GetModifier(GameplayModifierOperator operation, AttributeDefinition definition, string modifierId);

        bool RemoveModifier(GameplayModifierOperator operation, AttributeRecord record, string modifierId);
        bool RemoveModifier(GameplayModifierOperator operation, AttributeDefinition definition, string modifierId);
        bool RemoveModifier(GameplayModifierOperator operation, string definitionId, string modifierId);

        void ClearAllModifiers(AttributeRecord record, string modifierId);
        void ClearAllModifiers(string modifierId);
        void ClearAllModifiers(List<string> modifierIds);

        void OnStatChangeSubscribe(AttributeDefinition definition, UnityAction<AttributeRecord> callback);
        void OnStatChangeSubscribe(string definitionId, UnityAction<AttributeRecord> callback);
        void OnStatChangeSubscribe(AttributeRecord record, UnityAction<AttributeRecord> callback);

        void OnStatChangeUnsubscribe(AttributeRecord record, UnityAction<AttributeRecord> callback);
        void OnStatChangeUnsubscribe(AttributeDefinition definition, UnityAction<AttributeRecord> callback);
        void OnStatChangeUnsubscribe(string definitionId, UnityAction<AttributeRecord> callback);
    }
}
