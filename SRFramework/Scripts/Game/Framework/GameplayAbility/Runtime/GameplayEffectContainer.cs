using SlimeRPG.Framework.Tag;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public class GameplayEffectContainer
    {
        public GameplayTag tag;
        public TargetType targetType;
        public List<GameplayEffect> targetGameplayEffects;
    }
}
