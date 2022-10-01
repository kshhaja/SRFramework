using SlimeRPG.Framework.Ability;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(GameplayEffectExecutionCalculation), true)]
    public class GameplayEffectExecutionCalculationDrawer : PropertyDrawer
    {
        protected List<string> availableClasses = new List<string>();

        protected List<string> AvailableClasses
        {
            get
            {
                var lookup = typeof(GameplayEffectExecutionCalculation);

                availableClasses.Clear();
                availableClasses.Add("None");
                availableClasses.AddRange(System.AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(x => x.IsClass && !x.IsAbstract && (x.IsSubclassOf(lookup) || x == lookup))
                    .Select(type => type.Name)
                    .ToList());

                return availableClasses;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();

            EditorGUI.BeginProperty(position, label, property);

            int currentChoice = 0;
            if (property.objectReferenceValue)
                currentChoice = AvailableClasses.IndexOf(property.objectReferenceValue.GetType().Name);

            bool valueChanged = false;
            EditorGUI.BeginChangeCheck();
            int choice = EditorGUI.Popup(position, label.text, currentChoice, AvailableClasses.ToArray());
            if (EditorGUI.EndChangeCheck())
                valueChanged = true;

            if (valueChanged)
            {
                ScriptableObject newInstance = null;
                if (choice > 0)
                {
                    newInstance = ScriptableObject.CreateInstance(AvailableClasses[choice]);
                    newInstance.name = AvailableClasses[choice];
                }

                if (property.objectReferenceValue)
                    Object.DestroyImmediate(property.objectReferenceValue, true);

                if (newInstance)
                {
                    AssetDatabase.AddObjectToAsset(newInstance, property.serializedObject.targetObject);
                }

                AssetDatabase.SaveAssets();
                property.objectReferenceValue = newInstance;
            }

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
