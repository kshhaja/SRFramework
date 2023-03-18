using UnityEngine;
using System.Collections.Generic;
using SRFramework.Effect;
using SRFramework.Attribute.StatsContainers;


// will be remove
namespace SRFramework.Attribute
{
    [CreateAssetMenu(menuName = "Gameplay/Stats/Adjustment Collection")]
    public class AttributeAdjustmentCollection : ScriptableObject
    {
        protected string randomId;

        [Tooltip("ID used to add and remove stats. Failure to provide an ID will create a random GUID")]
        [SerializeField]
        public string id;

        [Tooltip("Available adjustments")]
        [SerializeField]
        public List<GameplayModifierInfo> adjustment = new List<GameplayModifierInfo>();

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

        public virtual void ApplyAdjustment(AttributeSet target, float index = 0)
        {
            foreach (var adj in adjustment)
            {
                if (adj == null || !adj.IsValid) 
                    continue;
                
                target.SetModifier(adj.operatorType, adj.definition, Id, adj.GetValue(index));
            }
        }

        public virtual void RemoveAdjustment(AttributeSet target)
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
