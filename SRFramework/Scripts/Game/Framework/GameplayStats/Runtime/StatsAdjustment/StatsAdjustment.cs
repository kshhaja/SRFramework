using UnityEngine;
using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem.StatsContainers;


// will be remove
namespace SlimeRPG.Framework.StatsSystem
{
    [CreateAssetMenu(fileName = "StatsAdjustment", menuName = "Gameplay/Stats/Adjustment")]
    public class StatsAdjustment : ScriptableObject
    {
        protected string randomId;

        [Tooltip("ID used to add and remove stats. Failure to provide an ID will create a random GUID")]
        [SerializeField]
        public string id;

        [Tooltip("Apply this operator to all adjustments")]
        [SerializeField]
        public OperatorTypeNone forceOperator;

        [Tooltip("Available adjustments")]
        [SerializeField]
        public List<ModifierGroup> adjustment = new List<ModifierGroup>();

        public virtual string Id
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    if (string.IsNullOrEmpty(randomId))
                        randomId = System.Guid.NewGuid().ToString();
                    
                    return randomId;
                }

                return id;
            }
        }


        public virtual void ApplyAdjustment(StatsContainer target, float index = 0)
        {
            foreach (var adj in adjustment)
            {
                if (adj == null || !adj.IsValid) 
                    continue;
                
                target.SetModifier(adj.operatorType, adj.definition, Id, adj.GetValue(index));
            }
        }

        public virtual void RemoveAdjustment(StatsContainer target)
        {
            foreach (var adj in adjustment)
            {
                if (adj == null || !adj.IsValid) 
                    continue;
                
                target.RemoveModifier(adj.operatorType, adj.definition, Id);
            }
        }
    }
}
