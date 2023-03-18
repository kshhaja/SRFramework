using SRFramework.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Effect
{
    public abstract class GameplayEffectCustomApplicationRequirement : ScriptableObject
    {
        public abstract bool CanApplyGameplayEffect(AbilitySystemComponent source);
    }
}