using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using SlimeRPG.Framework.Tag;


namespace UnityEditor
{
    [Serializable]
    [CustomPropertyDrawer(typeof(GameplayTagContainer))]
    public class GameplayTagContainerDrawer : PropertyDrawer
    {
        [SerializeField] 
        private SerializedProperty serializedProperty;
        
        [SerializeField] 
        private ReorderableList reorderableList;
        
        [SerializeField] 
        private int selection = -1;

        protected GenericMenu menu;

        public bool Initialized => 
            serializedProperty != null && 
            reorderableList != null && 
            reorderableList.count == serializedProperty.FindPropertyRelative("list").arraySize;


        protected void Initialize(SerializedProperty property)
        {
            // 리스트는 캐싱하는게 낫겠다.
            SerializedProperty list = property.FindPropertyRelative("list");

            if (list != null && list.isArray)
            {
                serializedProperty = property;

                List<SerializedProperty> elements = new List<SerializedProperty>();
                if (list.isArray)
                {
                    for (int i = 0; i < list.arraySize; ++i)
                    {
                        elements.Add(list.GetArrayElementAtIndex(i));
                    }
                }

                if (elements != null)
                {
                    reorderableList = new ReorderableList(elements, typeof(SerializedProperty), true, true, true, true);
                    reorderableList.drawHeaderCallback = DisplayHeaderCallback;
                    reorderableList.drawElementCallback = DrawElementCallback;
                    reorderableList.onAddCallback = AddCallback;
                    reorderableList.onRemoveCallback = RemoveCallback;
                    reorderableList.onCanAddCallback = CanAddCallback;
                    reorderableList.onCanRemoveCallback = CanRemoveCallback;
                    reorderableList.onReorderCallback = ReorderCallback;
                    reorderableList.elementHeightCallback = ElementHeightCallback;
                    reorderableList.onSelectCallback = SelectCallback;
                }

                OnInitialize(property);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!Initialized) 
                Initialize(property);

            if (Initialized)
            {
                if (property.isExpanded)
                    return EditorGUIUtility.singleLineHeight + reorderableList.GetHeight();
                else
                    return EditorGUIUtility.singleLineHeight;
            }

            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!Initialized) 
                Initialize(property);

            if (Initialized)
            {
                Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

                if (property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, new GUIContent(property.isExpanded?"":property.displayName)))
                {
                    Rect listRect = new Rect(position.x, position.y, position.width, reorderableList.GetHeight());
                    listRect = EditorGUI.IndentedRect(listRect);

                    reorderableList.DoList(listRect);
                }
            }
        }

        void DropdownClickHandler(object target)
        {
            if (serializedProperty != null && target != null)
            {
                SerializedProperty tagList = serializedProperty.FindPropertyRelative("list");
                tagList.arraySize += 1;
                tagList.GetArrayElementAtIndex(tagList.arraySize - 1).objectReferenceValue = (GameplayTag)target;

                serializedProperty.serializedObject.ApplyModifiedProperties();
                Initialize(serializedProperty);
            }
        }

        protected void DrawElementCallback(Rect rect, int index, bool activated, bool focused)
        {
            SerializedProperty list = serializedProperty.FindPropertyRelative("list");
            SerializedProperty tag = list.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(new Rect(rect.x, rect.y + 1, rect.width, rect.height - 2), tag, GUIContent.none);
        }

        protected void AddCallback(ReorderableList list)
        {
            if (menu != null) 
                menu.ShowAsContext();
        }

        protected void RemoveCallback(ReorderableList list)
        {
            SerializedProperty serializedList = serializedProperty.FindPropertyRelative("list");
            serializedList.GetArrayElementAtIndex(list.index).objectReferenceValue = null;
            serializedList.DeleteArrayElementAtIndex(list.index);
            serializedList.serializedObject.ApplyModifiedProperties();
            
            Initialize(serializedProperty);
        }

        protected void OnInitialize(SerializedProperty property)
        {
            menu = new GenericMenu();
            List<GameplayTag> tags = new List<GameplayTag>(Resources.FindObjectsOfTypeAll(typeof(GameplayTag)) as GameplayTag[]);
            tags.Sort();

            for (int i = 0; i < tags.Count; ++i)
            {
                if (tags[i].Depth > -1)
                {
                    menu.AddItem(new GUIContent(tags[i].GetFullPath() + " "), false, DropdownClickHandler, tags[i]);
                }
            }
        }

        protected bool CanAddCallback(ReorderableList list)
        {
            return menu != null && menu.GetItemCount() > 0;
        }

        protected virtual void DisplayHeaderCallback(Rect rect)
        {
            if (serializedProperty == null) 
                return;

            int iIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(rect, serializedProperty.displayName);
            EditorGUI.indentLevel = iIndent;
        }

        protected virtual bool CanRemoveCallback(ReorderableList list)
        {
            return true;
        }

        protected virtual void ReorderCallback(ReorderableList list)
        {
            SerializedProperty serializedList = serializedProperty.FindPropertyRelative("list");
            serializedList.MoveArrayElement(selection, list.index);
        }

        protected virtual float ElementHeightCallback(int index)
        {
            SerializedProperty list = serializedProperty.FindPropertyRelative("list");
            return EditorGUI.GetPropertyHeight(list.GetArrayElementAtIndex(index), true) + 2.0f;
        }

        protected virtual void SelectCallback(ReorderableList list)
        {
            selection = list.index;
        }
    }
}
