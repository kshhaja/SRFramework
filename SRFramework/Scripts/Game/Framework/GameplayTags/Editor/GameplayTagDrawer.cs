using UnityEngine;
using System.Collections.Generic;
using SlimeRPG.Framework.Tag;


namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(GameplayTag))]
    public class GameplayTagDrawer : PropertyDrawer
    {
        private SerializedProperty serializedProperty;


        public static GenericMenu GetTagMenu(GenericMenu.MenuFunction2 function)
        {
            GenericMenu menu = new GenericMenu();
            if (function != null)
            {
                List<GameplayTag> tags = new List<GameplayTag>(Resources.FindObjectsOfTypeAll(typeof(GameplayTag)) as GameplayTag[]);
                tags.Sort();

                menu.AddItem(new GUIContent("None"), false, function, null);

                for (int i = 0; i < tags.Count; ++i)
                {
                    if (tags[i].Depth > -1)
                        menu.AddItem(new GUIContent(tags[i].GetFullPath() + " "), false, function, tags[i]);
                }
            }
            return menu;
        }

        private void DropdownClickHandler(object obj)
        {
            serializedProperty.objectReferenceValue = obj as GameplayTag;
            serializedProperty.serializedObject.ApplyModifiedProperties();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            serializedProperty = property;

            GameplayTag tag = property.objectReferenceValue as GameplayTag;
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            Rect buttonRect = new Rect(position.x + labelRect.width, position.y, position.width - labelRect.width, position.height);

            GUIStyle buttonStyle = new GUIStyle("DropDown");
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            if (label != GUIContent.none)
                EditorGUI.LabelField(labelRect, label);
            else
                buttonRect = position;

            // 메뉴가 생각보다 불편하다... search bar + 드롭다운 리스트가 나으려나..
            if (EditorGUI.DropdownButton(buttonRect, new GUIContent(tag ? tag.GetFullPath() : "None"), FocusType.Keyboard))
            {
                GenericMenu menu = GetTagMenu(DropdownClickHandler);
                List<GameplayTag> tags = new List<GameplayTag>(Resources.FindObjectsOfTypeAll(typeof(GameplayTag)) as GameplayTag[]);

                tags.Sort();
                menu.ShowAsContext();
            }
        }
    }
}
