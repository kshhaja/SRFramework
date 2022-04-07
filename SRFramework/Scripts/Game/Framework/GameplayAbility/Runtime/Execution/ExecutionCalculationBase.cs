﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Framework.Ability
{
    public abstract class ExecutionCalculationBase : ScriptableObject
    {
        public abstract StatAdjustmentCollection Execute(List<StatAdjustment> executionModifiers, AbilitySystemCharacter target, float index);
    }
}
