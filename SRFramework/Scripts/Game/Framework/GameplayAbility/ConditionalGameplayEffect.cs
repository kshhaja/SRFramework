using SlimeRPG.Framework.Tag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public struct ConditionalGameplayEffect
    {
        public GameplayEffect effectClass;
        public GameplayTagContainer requiredSourceTags;
    }
}