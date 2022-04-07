using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.Ability;
using SlimeRPG.Framework.StatsSystem;

namespace UnityEditor
{
    [CustomEditor(typeof(GameplayEffectScriptableObject))]
    public class GameplayEffectScriptableObjectEditor : Editor
    {
        protected SerializedProperty durationProperty;
        protected SerializedProperty modifiersProperty;
        protected SerializedProperty executionProperty;
        protected SerializedProperty conditionalEffectsProperty;
        protected SerializedProperty gameplayTagProperty;
        protected SerializedProperty periodProperty;

        protected List<string> availableModifierType;
        protected List<string> availableExecutionType;



        private void OnEnable()
        {
            durationProperty = serializedObject.FindProperty(nameof(GameplayEffectScriptableObject.duration));
            modifiersProperty = serializedObject.FindProperty(nameof(GameplayEffectScriptableObject.modifiers));
            executionProperty = serializedObject.FindProperty(nameof(GameplayEffectScriptableObject.execution));
            conditionalEffectsProperty = serializedObject.FindProperty(nameof(GameplayEffectScriptableObject.conditionalEffects));
            gameplayTagProperty = serializedObject.FindProperty(nameof(GameplayEffectScriptableObject.gameplayEffectTags));
            periodProperty = serializedObject.FindProperty(nameof(GameplayEffectScriptableObject.period));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(durationProperty);

            OnCollectionGUI(modifiersProperty, typeof(StatAdjustmentCollection));
            OnCollectionGUI(executionProperty, typeof(GameplayExecutionCollection));

            EditorGUILayout.PropertyField(conditionalEffectsProperty);
            EditorGUILayout.PropertyField(gameplayTagProperty);
            EditorGUILayout.PropertyField(periodProperty);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnCollectionGUI(SerializedProperty property, System.Type type)
        {
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(property);
            EditorGUI.EndDisabledGroup();

            if (property.objectReferenceValue == null)
            {
                if (GUILayout.Button("Add " + property.displayName))
                {
                    var newInstance = CreateInstance(type);
                    newInstance.name = target.name + "_" + property.displayName;

                    AssetDatabase.AddObjectToAsset(newInstance, target);
                    AssetDatabase.SaveAssets();

                    property.objectReferenceValue = newInstance;
                }
            }
            else
            {
                if (GUILayout.Button("Remove " + property.displayName))
                {
                    DestroyImmediate(property.objectReferenceValue, true);
                    AssetDatabase.SaveAssets();

                    property.objectReferenceValue = null;
                }
            }
        }
    }
}
