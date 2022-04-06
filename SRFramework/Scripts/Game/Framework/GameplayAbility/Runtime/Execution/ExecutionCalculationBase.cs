using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    public class ExecutionCalculationBase : ScriptableObject
    {
        // damage execution sample.
        public virtual void Execute(List<StatAdjustment> executionModifiers, AbilitySystemCharacter target)
        {
            //var damageStats = executionModifiers.Where(x => x.definition is StatDefinition /*DamageStatDefinition*/);
            // reorder by damage type
            // ex> Dictionary<DamageType, float statvalue> dics;
            // dics[physical] 

            //var targetDefense = target.container.GetStat("all defense attributes");
            // and so on..

            // calculate each damageTypes and calculate total damage
            //float totalDamage = 0f;

            float testDamage = 0f;
            executionModifiers.ForEach(x => testDamage += x.GetValue(1));
            testDamage += target.container.GetModifier(OperatorType.Subtract, "current_health", "damage");
            // convert damage to target's currentHealth stat (subtract)
            target.container.SetModifier(OperatorType.Subtract, "current_health", "damage", testDamage);
        }
    }
}
