using System.Linq;
using UnityEngine;
using UnityEditorInternal;
using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.StatsSystem.StatsContainers;

namespace UnityEditor
{
    public class StatDefinitionOverrideCollectionEditor
    {
        public const int PICKER_CONTROL_ID = 236534;
        private StatsContainer container;
        private StatDefinitionOverrideCollection targetObject;

        private ReorderableList reorderableList;
        private SerializedProperty definitionsProperty;
        private SerializedProperty overridesProperty;


        public void Init(SerializedObject target)
        {
            overridesProperty = target.FindProperty(nameof(StatDefinitionOverrideCollection.overrides));
        }

        public void SetContainer(StatsContainer container)
        {
            this.container = container;
        }

        public void OnGUI()
        {

            // statsContainer 개조하는게 가장 좋은 방법일듯.
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

        protected void DrawElementCallback(Rect rect, int index, bool activated, bool focused)
        {
            var element = overridesProperty.GetArrayElementAtIndex(index);
            var stat = element.objectReferenceValue as StatDefinition;

            if (stat)
            {
                var propRect = rect;
                //var propDef = element.FindPropertyRelative("definition");
                //propRect.y += 4; // 헤더 스페이싱
                //propRect.height = EditorGUIUtility.singleLineHeight;
                //EditorGUI.PropertyField(propRect, propDef);

                var propDef = new SerializedObject(element.objectReferenceValue);
                //GUI.Label();

                var propValue = propDef.FindProperty("value").FindPropertyRelative("value");
                propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(propRect, propValue);

                var propBool = propDef.FindProperty("value").FindPropertyRelative("roundToInt");
                propRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(propRect, propBool);
            }
        }

        protected float ElementHeightCallback(int index)
        {
            var elementCount = 2;
            var spacing = 8;
            return (EditorGUIUtility.singleLineHeight * elementCount) + (EditorGUIUtility.standardVerticalSpacing * elementCount) + spacing;
        }
    }
}
