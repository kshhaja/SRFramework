using SlimeRPG.Framework.Ability;
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
        SerializedProperty iconProperty;
        SerializedProperty descriptionProperty;
        SerializedProperty tagProperty;
        SerializedProperty effectProperty;
        SerializedProperty costProperty;
        SerializedProperty cooldownProperty;
        SerializedProperty secondaryAbilityProperty;

        protected List<string> availableGameplayEffectType;
        
        // for independent foldout states
        protected Dictionary<SerializedProperty, Editor> cachedEditor = new Dictionary<SerializedProperty, Editor>();


        public void Init(AbstractAbilityScriptableObject target, SerializedObject targetObject)
        {
            this.target = target;
            this.targetObject = targetObject;

            nameProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.abilityName));
            iconProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.icon));
            descriptionProperty = this.targetObject.FindProperty(nameof(AbstractAbilityScriptableObject.description));
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
            EditorGUILayout.PropertyField(iconProperty);
            if (iconProperty.objectReferenceValue)
            {
                var preview = AssetPreview.GetAssetPreview(iconProperty.objectReferenceValue);
                GUILayout.Label(preview, GUILayout.Height(64));
                EditorGUILayout.Space();
            }
            EditorGUILayout.PropertyField(descriptionProperty, GUILayout.MinHeight(128));
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
