using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.Ability;
using System.Linq;
using UnityEditorInternal;
using SlimeRPG.Framework.StatsSystem;

namespace UnityEditor
{
    [CustomEditor(typeof(GameplayExecutionCollection))]
    public class GameplayExecutionCollectionEditor : Editor
    {
        public const int PICKER_CONTROL_ID = 234358;
        protected const int HEADER_SPACING = 4;
        protected const int VERTICAL_SPACING = 8;

        private ReorderableList reorderableList;
        protected SerializedProperty classProperty;
        protected SerializedProperty modifiersProperty;

        protected List<string> availableClasses;


        private void OnEnable()
        {
            classProperty = serializedObject.FindProperty(nameof(GameplayExecutionCollection.calculationClass));
            modifiersProperty = serializedObject.FindProperty(nameof(GameplayExecutionCollection.calculationModifiers));

            var lookup = typeof(ExecutionCalculationBase);
            availableClasses = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && (x.IsSubclassOf(lookup) || x == lookup))
                .Select(type => type.Name)
                .ToList();

            if (modifiersProperty != null && modifiersProperty.isArray)
            {
                reorderableList = new ReorderableList(serializedObject, modifiersProperty, true, true, true, true);
                reorderableList.drawHeaderCallback = DisplayHeaderCallback;
                reorderableList.onAddCallback = OnAddCallback;
                reorderableList.onRemoveCallback = OnRemoveCallback;
                reorderableList.drawElementCallback = DrawElementCallback;
                reorderableList.elementHeightCallback = ElementHeightCallback;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(classProperty);
            EditorGUI.EndDisabledGroup();

            if (classProperty.objectReferenceValue == null)
            {
                int choice = EditorGUILayout.Popup("", -1, availableClasses.ToArray());

                if (choice != -1)
                {
                    var newInstance = CreateInstance(availableClasses[choice]);
                    newInstance.name = classProperty.displayName;

                    AssetDatabase.AddObjectToAsset(newInstance, target);
                    AssetDatabase.SaveAssets();
                    
                    classProperty.objectReferenceValue = newInstance;
                }
            }
            else
            {
                if (GUILayout.Button("Remove " + classProperty.displayName))
                {
                    DestroyImmediate(classProperty.objectReferenceValue, true);
                    AssetDatabase.SaveAssets();
                    
                    classProperty.objectReferenceValue = null;
                }
            }

            EditorGUILayout.Space();
            reorderableList.DoLayoutList();
            WritePickedObject();

            serializedObject.ApplyModifiedProperties();
        }

        void WritePickedObject()
        {
            if (Event.current.commandName != "ObjectSelectorClosed"
                || Event.current.type != EventType.ExecuteCommand)
                return;

            var id = EditorGUIUtility.GetObjectPickerControlID();
            if (id != PICKER_CONTROL_ID)
                return;

            var obj = EditorGUIUtility.GetObjectPickerObject();
            if (obj == null || !(obj is StatDefinition))
                return;

            modifiersProperty.arraySize += 1;

            var element = modifiersProperty.GetArrayElementAtIndex(modifiersProperty.arraySize - 1);
            var def = (StatDefinition)obj;
            var propDef = element.FindPropertyRelative(nameof(StatAdjustment.definition));
            propDef.objectReferenceValue = def;
        }

        protected virtual void DisplayHeaderCallback(Rect rect)
        {
            if (modifiersProperty == null)
                return;

            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(rect, modifiersProperty.displayName);
            EditorGUI.indentLevel = indentLevel;
        }

        protected void OnAddCallback(ReorderableList list)
        {
            EditorGUIUtility.ShowObjectPicker<StatDefinition>(null, false, null, PICKER_CONTROL_ID);
        }

        protected void OnRemoveCallback(ReorderableList list)
        {
            modifiersProperty.DeleteArrayElementAtIndex(list.index);
        }

        protected void DrawElementCallback(Rect rect, int index, bool activated, bool focused)
        {
            var element = modifiersProperty.GetArrayElementAtIndex(index);

            var propDef = element.FindPropertyRelative("definition");
            var propRect = rect;
            propRect.y += HEADER_SPACING; // 헤더 스페이싱
            propRect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(propRect, propDef);
            EditorGUI.EndDisabledGroup();

            var propValue = element.FindPropertyRelative("value.value");
            propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(propRect, propValue);

            var propBool = element.FindPropertyRelative("value.roundToInt");
            propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(propRect, propBool);

            var propOperator = element.FindPropertyRelative("operatorType");
            propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(propRect, propOperator);
        }

        protected float ElementHeightCallback(int index)
        {
            var elementCount = 4;
            return (EditorGUIUtility.singleLineHeight * elementCount) + (EditorGUIUtility.standardVerticalSpacing * elementCount) + VERTICAL_SPACING;
        }
    }
}
