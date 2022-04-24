using System;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public abstract class TargetType : ScriptableObject
    {
        public abstract RaycastHit[] GetTargets(GameObject targetingGameObject);
    }
}
