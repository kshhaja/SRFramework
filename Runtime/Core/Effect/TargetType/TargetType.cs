using System;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Effect
{
    [Serializable]
    public abstract class TargetType : ScriptableObject
    {
        public abstract RaycastHit[] GetTargets(GameObject targetingGameObject);
    }
}
