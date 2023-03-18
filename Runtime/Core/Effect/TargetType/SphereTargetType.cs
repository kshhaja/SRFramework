using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRFramework.Effect
{
    public class SphereTargetType : TargetType
    {
        public Vector3 origin;
        public float radius;
        public Vector3 direction;
        public float maxDistance;
        public LayerMask layerMask;
        public QueryTriggerInteraction queryTriggerInteraction;


        public override RaycastHit[] GetTargets(GameObject targetingGameObject)
        {
            var baseOrigin = targetingGameObject.transform.position + origin;
            return Physics.SphereCastAll(baseOrigin, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
        }
    }
}
