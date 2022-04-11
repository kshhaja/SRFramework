using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.StatsSystem.StatsContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemStatsAdjustment", menuName = "Gameplay/Stats/ItemStatsAdjustment")]
public class ItemStatsAdjustment : StatAdjustmentCollection
{
    // mode 로 대체될 예정
    private bool HasDefiniiton(StatDefinition definition)
    {
        return adjustment.Find(x => x.definition == definition) != null;
    }

    public void SetupWith(StatDefinitionCollection collection)
    {
        adjustment.Clear();

        foreach (var def in collection.GetDefinitions())
        {
            var mod = new GameplayModifierInfo();
            mod.definition = def;
            mod.modifierMagnitude = def.Value;
            adjustment.Add(mod);
        }
    }

    public override void ApplyAdjustment(StatsContainer target, float index = 0)
    {
        foreach (var id in adjustment)
        {
            if (target.HasRecord(id.definition) == false)
                target.SetStat(id.definition);
        }

        base.ApplyAdjustment(target, index);
    }
}
