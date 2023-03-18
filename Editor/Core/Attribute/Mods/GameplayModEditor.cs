using System;
using UnityEngine;
using UnityEditorInternal;
using SRFramework.Attribute;
using SRFramework.Effect;


namespace UnityEditor
{
    [CustomEditor(typeof(GameplayMod))]
    public class GameplayModEditor : Editor
    {
        public const int PICKER_CONTROL_ID = 428643;
        protected const int HEADER_SPACING = 4;

        private ReorderableList reorderableList;
        private SerializedProperty idProperty;
        private SerializedProperty modifiersProperty;

        public static int previewLevel = 1;
        private SystemLanguage systemLanguage;


        protected virtual void OnEnable()
        {
            systemLanguage = Application.systemLanguage;
            idProperty = serializedObject.FindProperty(nameof(GameplayMod.id));
            modifiersProperty = serializedObject.FindProperty(nameof(GameplayMod.modifiers));

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
            EditorGUILayout.PropertyField(idProperty);
            EditorGUILayout.Space();
            reorderableList.DoLayoutList();
            WritePickedObject();

            DrawPreviewGroup();
            
            serializedObject.ApplyModifiedProperties();
        }

        public void DrawPreviewGroup()
        {
            EditorGUILayout.Space();

            GUILayout.Label("Preview");
            GUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;

            GUILayout.Label("Level");
            previewLevel = EditorGUILayout.IntSlider(previewLevel, 1, 20);

            GUILayout.Label($"Localized Text (SystemLanguage={systemLanguage})");

            try
            {
                EditorGUILayout.HelpBox(TextSample.Instance.ModText(target as GameplayMod, previewLevel), MessageType.None);
            }
            catch (FormatException e)
            {
                EditorGUILayout.HelpBox(e.Message, MessageType.Error);
            }

            EditorGUI.indentLevel--;
            GUILayout.EndVertical();

            EditorGUILayout.Space();
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
            if (obj == null || !(obj is AttributeDefinition))
                return;

            modifiersProperty.arraySize += 1;

            var element = modifiersProperty.GetArrayElementAtIndex(modifiersProperty.arraySize - 1);
            var def = (AttributeDefinition)obj;
            var propDef = element.FindPropertyRelative(nameof(GameplayModifierInfo.definition));
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
            EditorGUIUtility.ShowObjectPicker<AttributeDefinition>(null, false, null, PICKER_CONTROL_ID);
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
            EditorGUI.PropertyField(propRect, propDef);

            var propOperator = element.FindPropertyRelative("operatorType");
            propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(propRect, propOperator);

            var propValue = element.FindPropertyRelative("modifierMagnitude");
            propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(propRect, propValue);
        }

        protected float ElementHeightCallback(int index)
        {
            var elementCount = 6;
            var spacing = 8;
            return (EditorGUIUtility.singleLineHeight * elementCount) + (EditorGUIUtility.standardVerticalSpacing * elementCount) + spacing;
        }
    }
}
