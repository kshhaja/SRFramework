using SRFramework.Attribute.StatsContainers;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Attribute
{
    [CreateAssetMenu(fileName = "DerivedStatDefinition", menuName = "Gameplay/Stats/Definitions/Derived")]
    public class DerivedAttributeDefinition : AttributeDefinition
    {
        // DerivedStatDefinition will automatically update according to stat changes.
        [SerializeField]
        protected AttributeAdjustmentCollection stats;
        
        protected WeakReference<AttributeSet> containerWeakRef;
        protected float internalValue;
        protected AttributeRecord record;


        public override void Setup(AttributeSet container)
        {
            containerWeakRef = new WeakReference<AttributeSet>(container);
            record = new AttributeRecord(this, value);
            // internalValue = value.value.Evaluate(1);
            
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

        void SetModifier(GameplayModifierOperator operation, string modifierId, float value)
        {
            record.GetModifier(operation).Set(modifierId, value);
        }

        protected virtual void DerivesStatChanged(AttributeRecord rec)
        {
            if (containerWeakRef.TryGetTarget(out var container))
            {
                foreach (var adj in stats.adjustment)
                {
                    // how to set index?
                    var value = container.GetStat(adj.definition, 1);
                    SetModifier(adj.operatorType, adj.definition.Id, value);
                }
                
                // value.value.mode = ParticleSystemCurveMode.Constant;
                internalValue = record.GetValue(1);

                Debug.Log(this.DisplayName + " is " + internalValue.ToString());
                // calculated result == DerivedStat's base value
                // container.SetBaseStat(this, Value);
            }
        }

        public override List<AttributeDefinition> GetDefinitions(HashSet<AttributeDefinitionBase> visited)
        {
			if (visited == null || visited.Contains(this))
			{
				if (Application.isPlaying)
					Debug.LogWarningFormat("Duplicate StatDefinition detected {0}", name);

				return new List<AttributeDefinition>();
			}

			visited.Add(this);
			return new List<AttributeDefinition> { this };
		}
    }
}
