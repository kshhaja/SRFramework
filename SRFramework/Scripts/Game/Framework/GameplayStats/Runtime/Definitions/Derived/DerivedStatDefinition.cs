using SlimeRPG.Framework.StatsSystem.StatsContainers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    [CreateAssetMenu(fileName = "DerivedStatDefinition", menuName = "Gameplay/Stats/Definitions/Default")]
    public class DerivedStatDefinition : StatDefinition
    {
        // DerivedStatDefinition will automatically update according to stat changes.
        protected StatAdjustmentCollection stats;
        private WeakReference<StatsContainer> containerWeakRef;
        private StatRecord record;


        public override void Setup(StatsContainer container)
        {
            containerWeakRef = new WeakReference<StatsContainer>(container);
            record = new StatRecord(this, value);

            // subscribe callback for automatically update
            foreach (var stat in stats.adjustment)
            {
                // derives stat must exists in container
                // make sure contains
                if (container.HasRecord(stat.definition) == false)
                    throw new Exception(stat.definition.DisplayName + " not found.");
                
                SetModifier(stat.operatorType, stat.definition.Id, stat.GetValue(1));
                container.OnStatChangeSubscribe(stat.definition, DerivesStatChanged);
            }
        }

        void SetModifier(OperatorType operation, string modifierId, float value)
        {
            record.GetModifier(operation).Set(modifierId, value);
        }

        private void DerivesStatChanged(StatRecord rec)
        {
            if (containerWeakRef.TryGetTarget(out var container))
            {
                foreach (var adj in stats.adjustment)
                {
                    // how to set index?
                    var value = container.GetStat(adj.definition, 1);
                    SetModifier(adj.operatorType, adj.definition.Id, value);
                }
                
                value.value.mode = ParticleSystemCurveMode.Constant;
                value.value.constant = record.GetValue(1);

                // calculated result == DerivedStat's base value
                // container.SetBaseStat(this, Value);
            }
        }

        public override List<StatDefinition> GetDefinitions(HashSet<StatDefinitionBase> visited)
        {
			if (visited == null || visited.Contains(this))
			{
				if (Application.isPlaying)
					Debug.LogWarningFormat("Duplicate StatDefinition detected {0}", name);

				return new List<StatDefinition>();
			}

			visited.Add(this);
			return new List<StatDefinition> { this };
		}
    }
}
