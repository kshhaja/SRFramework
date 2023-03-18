using UnityEngine;
using UnityEditorInternal;
using SRFramework.Attribute;


namespace UnityEditor
{
    [CustomEditor(typeof(GameplayModContainer))]
    public class GameplayModContainerEditor : Editor
    {
        public const int PICKER_CONTROL_ID = 723458;
        protected const int HEADER_SPACING = 4;

        private ReorderableList reorderableList;
        private SerializedProperty idProperty;
        private SerializedProperty modsProperty;

        public static int previewLevel = 1;


        protected virtual void OnEnable()
        {
            idProperty = serializedObject.FindProperty(nameof(GameplayModContainer.id));
            modsProperty = serializedObject.FindProperty(nameof(GameplayModContainer.mods));

            if (modsProperty != null && modsProperty.isArray)
            {
                reorderableList = new ReorderableList(serializedObject, modsProperty, true, true, true, true);
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

            GUILayout.Label($"Localized Text = {Application.systemLanguage}");

            foreach (var mod in (target as GameplayModContainer).mods)
                EditorGUILayout.HelpBox(TextSample.Instance.ModText(mod, previewLevel), MessageType.None);

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
            if (obj == null || !(obj is GameplayMod))
                return;

            modsProperty.arraySize += 1;

            var element = modsProperty.GetArrayElementAtIndex(modsProperty.arraySize - 1);
            var def = (GameplayMod)obj;
            element.objectReferenceValue = def;
        }

        protected virtual void DisplayHeaderCallback(Rect rect)
        {
            if (modsProperty == null)
                return;

            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(rect, modsProperty.displayName);
            EditorGUI.indentLevel = indentLevel;
        }

        protected void OnAddCallback(ReorderableList list)
        {
            EditorGUIUtility.ShowObjectPicker<GameplayMod>(null, false, null, PICKER_CONTROL_ID);
        }

        protected void OnRemoveCallback(ReorderableList list)
        {
            modsProperty.DeleteArrayElementAtIndex(list.index);
        }

        protected void DrawElementCallback(Rect rect, int index, bool activated, bool focused)
        {
            var element = modsProperty.GetArrayElementAtIndex(index);

            var propRect = rect;
            propRect.y += HEADER_SPACING; // 헤더 스페이싱
            propRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(propRect, element);
        }

        protected float ElementHeightCallback(int index)
        {
            var elementCount = 1;
            var spacing = 8;
            return (EditorGUIUtility.singleLineHeight * elementCount) + (EditorGUIUtility.standardVerticalSpacing * elementCount) + spacing;
        }
    }
}