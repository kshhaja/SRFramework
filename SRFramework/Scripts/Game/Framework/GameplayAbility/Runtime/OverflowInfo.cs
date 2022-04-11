using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class OverflowInfo
    {
        public List<GameplayEffect> overflowEffects;
        public bool denyOverlowApplication;
        public bool clearStackOnOverflow;
    }
}
