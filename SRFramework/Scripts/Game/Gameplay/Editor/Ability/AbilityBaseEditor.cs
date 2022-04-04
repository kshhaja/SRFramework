using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlimeRPG.Gameplay.Character.Ability;


namespace UnityEditor
{
    [CustomEditor(typeof(AbilityBase), true)]
    public class AbilityBaseEditor : Editor
    {
        protected AbilityBase targetCache;
        protected AbstractAbilityEditor editor;

        protected SerializedProperty levelProperty;
        protected SerializedProperty qualityProperty;
        protected SerializedProperty clipProperty;

        protected virtual void OnEnable()
        {
            targetCache = target as AbilityBase;
            
            levelProperty = serializedObject.FindProperty(nameof(AbilityBase.level));
            qualityProperty = serializedObject.FindProperty(nameof(AbilityBase.quality));
            clipProperty = serializedObject.FindProperty(nameof(AbilityBase.clip));

            editor = new AbstractAbilityEditor();
            editor.Init(targetCache, serializedObject);
        }

        public override void OnInspectorGUI()
        {
            editor.OnBaseGUI();

            EditorGUILayout.PropertyField(levelProperty);
            EditorGUILayout.PropertyField(qualityProperty);
            EditorGUILayout.PropertyField(clipProperty);
            EditorGUILayout.Space();

            editor.OnAbilityGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
