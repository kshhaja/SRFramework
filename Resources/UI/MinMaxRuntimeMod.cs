using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


public class MinMaxRuntimeMod : DerivedStatDefinition
{
    protected override void DerivesStatChanged(StatRecord rec)
    {
        base.DerivesStatChanged(rec);

        if (containerWeakRef.TryGetTarget(out var container))
        {
            foreach (var adj in stats.adjustment)
            {
                var value = container.GetStat(adj.definition, 1);
            }
        }
    }
}
