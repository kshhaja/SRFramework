using SRFramework.Tag;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Effect
{
    [Serializable]
    public struct ConditionalGameplayEffect
    {
        public GameplayEffect effectClass;
        public GameplayTagContainer requiredSourceTags;
    }
}