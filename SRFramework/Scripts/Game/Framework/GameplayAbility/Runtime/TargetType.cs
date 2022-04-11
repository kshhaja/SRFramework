using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public abstract class TargetType : ScriptableObject
    {
        public abstract List<GameObject> GetTargets(GameObject targetingGameObject);
    }
}