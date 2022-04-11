using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public abstract class GameplayEffectCustomApplicationRequirement : ScriptableObject
    {
        public abstract bool CanApplyGameplayEffect();
    }
}