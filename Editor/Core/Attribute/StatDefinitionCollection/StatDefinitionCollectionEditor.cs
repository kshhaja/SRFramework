using System.Linq;
using UnityEngine;
using UnityEditorInternal;
using SlimeRPG.Framework.StatsSystem;


namespace UnityEditor
{
    [CustomEditor(typeof(StatDefinitionCollection))]
    public class StatDefinitionCollectionEditor : Editor 
    {
        public const int PICKER_CONTROL_ID = 236534;
        private StatDefinitionCollection targetObject;

        private ReorderableList reorderableList;
        private SerializedProperty definitionsProperty;


        private void OnEnable () 
        {
            targetObject = (StatDefinitionCollection)target;
            definitionsProperty = serializedObject.FindProperty(nameof(StatDefinitionCollection.definitions));

            if (definitionsProperty != null && definitionsProperty.isArray)
            {
                reorderableList = new ReorderableList(serializedObject, definitionsProperty, true, true, true, true);
				reorderableList.drawHeaderCallback = DisplayHeaderCallback;
                reorderableList.onAddCallback = OnAddCallback;
                reorderableList.onRemoveCallback = OnRemoveCallback;
                reorderableList.drawElementCallback = DrawElementCallback;
            }
            
            targetObject.definitions = targetObject.definitions.FindAll(d => d != null)
                .GroupBy(d => d.name)
                .Select(d => d.First())
                .ToList();
        }

        public override void OnInspectorGUI () {
            serializedObject.Update();
            reorderableList.DoLayoutList();
            WritePickedObject();
            serializedObject.ApplyModifiedProperties();
        }

        void WritePickedObject () {
            if (Event.current.commandName != "ObjectSelectorClosed"
                || Event.current.type != EventType.ExecuteCommand) 
                return;

            var id = EditorGUIUtility.GetObjectPickerControlID();
            if (id != PICKER_CONTROL_ID) 
                return;

            var obj = EditorGUIUtility.GetObjectPickerObject();
            if (obj == null || !(obj is AttributeDefinitionBase)) 
                return;

            var def = (AttributeDefinitionBase)obj;
            if (targetObject.definitions.Contains(def)) 
                return;

            definitionsProperty.arraySize += 1;
            definitionsProperty.GetArrayElementAtIndex(definitionsProperty.arraySize - 1).objectReferenceValue = def;
        }

        protected virtual void DisplayHeaderCallback(Rect rect)
        {
            if (definitionsProperty == null)
                return;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(rect, definitionsProperty.displayName);
            EditorGUI.indentLevel = indent;
        }

        protected void OnAddCallback(ReorderableList list)
        {
            EditorGUIUtility.ShowObjectPicker<AttributeDefinitionBase>(null, false, null, PICKER_CONTROL_ID);
        }

        protected void OnRemoveCallback(ReorderableList list)
        {
            // null 처리
            if (definitionsProperty.GetArrayElementAtIndex(list.index) != null)
                definitionsProperty.DeleteArrayElementAtIndex(list.index);
            // 제거
            definitionsProperty.DeleteArrayElementAtIndex(list.index);
        }

        protected void DrawElementCallback(Rect rect, int index, bool activated, bool focused)
        {
            rect.height = EditorGUIUtility.singleLineHeight;
            GUI.enabled = false;
            EditorGUI.ObjectField(rect, definitionsProperty.GetArrayElementAtIndex(index), new GUIContent(""));
            GUI.enabled = true;
        }
    }
}
