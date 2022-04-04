﻿using SlimeRPG.Framework.Ability;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UnityEditor
{
    public class AbstractAbilityEditor
    {
        AbstractAbilityScriptableObject target;

        SerializedObject targetObject;
        SerializedProperty nameProperty;
        SerializedProperty tagProperty;
        SerializedProperty effectProperty;
        SerializedProperty costProperty;
        SerializedProperty cooldownProperty;
        SerializedProperty secondaryAbilityProperty;

        protected List<string> availableGameplayEffectType;


        public void Init(AbstractAbilityScriptableObject target, SerializedObject targetObject)
        {
            this.target = target;
            this.targetObject = targetObject;

            nameProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.abilityName));
            tagProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.abilityTags));
            effectProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.effect));
            costProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.cost));
            cooldownProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.coolDown));
            secondaryAbilityProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.secondaryAbility));


            var lookup = typeof(GameplayEffectScriptableObject);
            availableGameplayEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && (x.IsSubclassOf(lookup) || x == lookup))
                .Select(type => type.Name)
                .ToList();
        }

        public void OnBaseGUI()
        {
            // 아이콘, 디스크립션 등 추가될 듯.
            EditorGUILayout.PropertyField(nameProperty);
        }

        public void OnAbilityGUI()
        {
            EditorGUILayout.PropertyField(tagProperty);

            OnGameplayEffectGUI(effectProperty);
            OnGameplayEffectGUI(costProperty);
            OnGameplayEffectGUI(cooldownProperty);
            
            EditorGUILayout.PropertyField(secondaryAbilityProperty);
        }

        private void OnGameplayEffectGUI(SerializedProperty property)
        {
            if (property.objectReferenceValue == null)
            {
                int choice = EditorGUILayout.Popup(property.displayName, -1, availableGameplayEffectType.ToArray());

                if (choice != -1)
                {
                    var newInstance = ScriptableObject.CreateInstance(availableGameplayEffectType[choice]);
                    newInstance.name = property.displayName;

                    AssetDatabase.AddObjectToAsset(newInstance, target);
                    property.objectReferenceValue = newInstance;
                }
            }
            else
            {
                EditorGUILayout.Space();
                GUILayout.Label(property.displayName);
                GUILayout.BeginVertical("HelpBox");
                EditorGUI.indentLevel++;
                Editor ed = null;
                Editor.CreateCachedEditor(property.objectReferenceValue, null, ref ed);
                ed.OnInspectorGUI();

                EditorGUILayout.Space();
                if (GUILayout.Button("Remove " + property.displayName))
                {
                    Object.DestroyImmediate(property.objectReferenceValue, true);
                    property.objectReferenceValue = null;
                }
                EditorGUI.indentLevel--;
                GUILayout.EndVertical();
            }
        }
    }
}