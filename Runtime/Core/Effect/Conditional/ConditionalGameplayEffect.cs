using SlimeRPG.Framework.Tag;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct ConditionalGameplayEffect
    {
        public GameplayEffect effectClass;
        public GameplayTagContainer requiredSourceTags;
    }
}