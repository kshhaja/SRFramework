using SlimeRPG.Framework.Tag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class EffectInflictableGameplayAbility : GameplayAbility
    {
        protected Dictionary<GameplayTag, GameplayEffectContainer> effectContainerMap;
    }
}
