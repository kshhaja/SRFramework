using SlimeRPG.Framework.StatsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEditor
{
    [CustomEditor(typeof(StatDefinition), true)]
    public class StatDefinitionEditor : Editor
    {


        private void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
