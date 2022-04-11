﻿using SlimeRPG.Framework.StatsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public struct ApplicationInfo
    {
        public ScalableMagnitude chanceToApplyToTarget;
        public List<GameplayEffectCustomApplicationRequirement> applicationRequirements;
    }
}
