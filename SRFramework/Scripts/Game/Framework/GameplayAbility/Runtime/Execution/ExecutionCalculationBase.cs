using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.StatsSystem.StatsContainers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class ExecutionCalculationBase : ScriptableObject
    {
        // damage execution sample.
        public virtual void Execute(List<GameplayExecution> sourceModifiers, AbilitySystemCharacter target)
        {
            var damageStats = sourceModifiers.Where(x => x.adjustment.definition is StatDefinition /*DamageStatDefinition*/);
            // reorder by damage type
            // ex> Dictionary<DamageType, float statvalue> dics;
            // dics[physical] 

            var targetDefense = target.container.GetStat("all defense attributes");
            // and so on..

            // calculate each damageTypes and calculate total damage
            float totalDamage = 0f;

            // convert damage to target's currentHealth stat (subtract)
            target.container.SetModifier(OperatorType.Subtract, "current_health", "temporary damage", totalDamage);
        }
    }
}
