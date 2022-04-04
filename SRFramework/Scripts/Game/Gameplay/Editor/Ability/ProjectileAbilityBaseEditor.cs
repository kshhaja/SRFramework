using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlimeRPG.Gameplay.Character.Ability;


namespace UnityEditor
{
    [CustomEditor(typeof(ProjectileAbilityBase), true)]
    public class ProjectileAbilityBaseEditor : AbilityBaseEditor
    {
        protected SerializedProperty projectileProperty;


        protected override void OnEnable()
        {
            base.OnEnable();
            projectileProperty = serializedObject.FindProperty(nameof(ProjectileAbilityBase.projectile));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(projectileProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
