using SRFramework.Effect;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(TargetType))]
    public class TargetTypeDrawer : PropertyDrawer
    {
        protected List<string> availableTargetTypeList = new List<string>();

        public TargetTypeDrawer() : base()
        {
            var lookup = typeof(TargetType);
            availableTargetTypeList.Add("None");
            availableTargetTypeList.AddRange(System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && (x.IsSubclassOf(lookup) || x == lookup))
                .Select(type => type.Name)
                .ToList());
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int currentChoice = 0;
            if (property.objectReferenceValue)
                currentChoice = availableTargetTypeList.IndexOf(property.objectReferenceValue.GetType().Name);

            int choice = EditorGUI.Popup(position, currentChoice, availableTargetTypeList.ToArray());

            if (choice != currentChoice)
            {
                if (property.objectReferenceValue)
                {
                    Object.DestroyImmediate(property.objectReferenceValue, true);
                    AssetDatabase.SaveAssets();

                    property.objectReferenceValue = null;
                }

                if (choice != 0)
                {
                    var newInstance = ScriptableObject.CreateInstance(availableTargetTypeList[choice]);
                    newInstance.name = availableTargetTypeList[choice];

                    AssetDatabase.AddObjectToAsset(newInstance, property.serializedObject.targetObject);
                    AssetDatabase.SaveAssets();

                    property.objectReferenceValue = newInstance;
                }
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
