using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class GameplayAttributeData
    {
        protected float baseValue;
        protected float currentValue;

        public float BaseValue => baseValue;
        public float CurrentValue => currentValue;

        public GameplayAttributeData(float defaultValue)
        {
            baseValue = defaultValue;
            currentValue = defaultValue;
        }

        public virtual void SetCurrentValue(float newValue)
        {
        }

        public virtual void SetBaseValue(float newValue)
        {
        }
    }


    public class AttributeSet
    {
        public virtual void InitFromMetaDataTable(/*DataTable table*/)
        {
        }

        public virtual bool PreGameplayEffectExecute()
        {
            return true;
        }

        public virtual void PostGameplayEffectExecute()
        {
        }

        public virtual void PreAttributeChange()
        {
        }

        public virtual void PreAttributeBaseChange()
        {
        }
    }


}
