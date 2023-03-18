﻿using UnityEngine;


namespace SlimeRPG.Framework.StatsSystem
{
    public enum OperatorTypeNone
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide
    }

    public static class OperatorTypeNoneMethods
    {
        public static GameplayModifierOperator ToOperatorType(this OperatorTypeNone type)
        {
            var i = (int)type;
            i -= 1;
            i = Mathf.Max(0, i);

            return (GameplayModifierOperator)i;
        }
    }
}