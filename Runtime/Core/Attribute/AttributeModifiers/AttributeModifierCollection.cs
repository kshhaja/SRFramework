using System;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Attribute
{
    public class AttributeModifierCollection
    {
        private GameplayModifierOperator type;

        private Dictionary<string, AttributeModifier> dicValues = new Dictionary<string, AttributeModifier>();
        private List<AttributeModifier> listValues = new List<AttributeModifier>();

        public bool forceRound;

        public delegate void Action();
        public event Action onDirty;

        // listValues를 직접 수정하면 StatModifierGroup 데이터 오염으로 인한 크래시가 발생할 것.
        public List<AttributeModifier> ListValues => listValues;

        public AttributeModifierCollection(GameplayModifierOperator type)
        {
            this.type = type;
        }

        public AttributeModifier Get(string id)
        {
            if (string.IsNullOrEmpty(id) || dicValues.TryGetValue(id, out var output) == false)
                return null;

            return output;
        }

        public void Set(string id, float value)
        {
            if (string.IsNullOrEmpty(id))
                return;

            if (forceRound)
                value = Mathf.Round(value);

            if (dicValues.TryGetValue(id, out var mod))
                mod.value = value;
            else
            {
                mod = new AttributeModifier(id, value);

                dicValues[id] = mod;
                listValues.Add(mod);
            }

            if (onDirty != null) 
                onDirty.Invoke();
        }

        public bool Remove(string id)
        {
            var target = Get(id);

            if (target == null)
                return false;

            dicValues.Remove(id);
            listValues.Remove(target);

            if (onDirty != null)
                onDirty.Invoke();

            return true;
        }

        public float ModifyValue(float original)
        {
            if (type == GameplayModifierOperator.Override)
            {
                // ensure policy.
                return original;
            }

            if (type == GameplayModifierOperator.Multiply)
            {
                var multiplier = 0f;
                foreach (var statModifier in listValues)
                    multiplier += statModifier.value;

                return Mathf.Max(0, original + original * multiplier);
            }

            var newVal = original;
            foreach (var statModifier in listValues)
            {
                switch (type)
                {
                    case GameplayModifierOperator.Add:
                        newVal += statModifier.value;
                        break;
                    case GameplayModifierOperator.Subtract:
                        newVal -= statModifier.value;
                        break;
                    case GameplayModifierOperator.Divide:
                        newVal /= statModifier.value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return newVal;
        }
    }
}
