using SlimeRPG.Framework.Ability;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UnityEditor
{
    public class AbstractAbilityEditor
    {
        GameplayAbility target;

        SerializedObject targetObject;
        SerializedProperty tagProperty;
        SerializedProperty costProperty;
        SerializedProperty cooldownProperty;
        SerializedProperty abilityTriggersProperty;

        protected List<string> availableGameplayEffectType;
        
        // for independent foldout states
        protected Dictionary<SerializedProperty, Editor> cachedEditor = new Dictionary<SerializedProperty, Editor>();


        public void Init(GameplayAbility target, SerializedObject targetObject)
        {
            this.target = target;
            this.targetObject = targetObject;

            tagProperty = this.targetObject.FindProperty(nameof(GameplayAbility.tags));
            costProperty = this.targetObject.FindProperty(nameof(GameplayAbility.cost));
            cooldownProperty = this.targetObject.FindProperty(nameof(GameplayAbility.coolDown));
            abilityTriggersProperty = this.targetObject.FindProperty(nameof(GameplayAbility.abilityTriggers));

            var lookup = typeof(GameplayEffect);
            availableGameplayEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && (x.IsSubclassOf(lookup) || x == lookup))
                .Select(type => type.Name)
                .ToList();
        }

        public void OnBaseGUI()
        {
            // 아이콘, 디스크립션 등 추가될 듯.
        }

        public void OnAbilityGUI()
        {
            EditorGUILayout.PropertyField(costProperty);
            EditorGUILayout.PropertyField(cooldownProperty);
            EditorGUILayout.PropertyField(tagProperty);

            // OnGameplayEffectGUI(costProperty);
            // OnGameplayEffectGUI(cooldownProperty);
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
                    AssetDatabase.SaveAssets();

                    property.objectReferenceValue = newInstance;
                }
            }
            else
            {
                EditorGUILayout.Space();
                GUILayout.Label(property.displayName);
                GUILayout.BeginVertical("HelpBox");
                EditorGUI.indentLevel++;

                Editor ed;
                if (cachedEditor.TryGetValue(property, out var editor))
                    ed = editor;
                else
                    cachedEditor[property] = ed = Editor.CreateEditor(property.objectReferenceValue);

                ed.OnInspectorGUI();

                EditorGUILayout.Space();
                if (GUILayout.Button("Remove " + property.displayName))
                {
                    Object.DestroyImmediate(property.objectReferenceValue, true);
                    AssetDatabase.SaveAssets();
                    
                    property.objectReferenceValue = null;
                }
                EditorGUI.indentLevel--;
                GUILayout.EndVertical();
            }
        }
    }
}
