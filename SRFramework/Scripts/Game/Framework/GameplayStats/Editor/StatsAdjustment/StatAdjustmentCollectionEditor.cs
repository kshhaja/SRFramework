using System;
using UnityEngine;
using UnityEditorInternal;
using SlimeRPG.Framework.StatsSystem;


namespace UnityEditor
{
	[CustomEditor(typeof(StatAdjustmentCollection), true)]
	public class StatAdjustmentCollectionEditor : Editor 
	{
		public const int PICKER_CONTROL_ID = 398473;
		protected const int HEADER_SPACING = 4;
		protected const int VERTICAL_SPACING = 8;

		private ReorderableList reorderableList;

		private SerializedProperty idProperty;
		private SerializedProperty adjustmentProperty;

		protected virtual bool ShowID => true;

        protected virtual void OnEnable () 
		{
			idProperty = serializedObject.FindProperty(nameof(StatAdjustmentCollection.id));
			adjustmentProperty = serializedObject.FindProperty(nameof(StatAdjustmentCollection.adjustment));

			if (adjustmentProperty != null && adjustmentProperty.isArray)
			{
				reorderableList = new ReorderableList(serializedObject, adjustmentProperty, true, true, true, true);
				reorderableList.drawHeaderCallback = DisplayHeaderCallback;
				reorderableList.onAddCallback = OnAddCallback;
				reorderableList.onRemoveCallback = OnRemoveCallback;
				reorderableList.drawElementCallback = DrawElementCallback;
				reorderableList.elementHeightCallback = ElementHeightCallback;
			}
		}

		public override void OnInspectorGUI () 
		{
			serializedObject.Update();

			if (ShowID)
				EditorGUILayout.PropertyField(idProperty);
			
			serializedObject.ApplyModifiedProperties();

			reorderableList.DoLayoutList();
            WritePickedObject();

			serializedObject.ApplyModifiedProperties();
		}

        void WritePickedObject () 
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

			adjustmentProperty.arraySize += 1;

			var element = adjustmentProperty.GetArrayElementAtIndex(adjustmentProperty.arraySize - 1);
			var def = (StatDefinition)obj;
			var propDef = element.FindPropertyRelative(nameof(GameplayModifierInfo.definition));
			propDef.objectReferenceValue = def;
		}

		protected virtual void DisplayHeaderCallback(Rect rect)
		{
			if (adjustmentProperty == null)
				return;

			int iIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			EditorGUI.LabelField(rect, adjustmentProperty.displayName);
			EditorGUI.indentLevel = iIndent;
		}

		protected void OnAddCallback(ReorderableList list)
        {
			EditorGUIUtility.ShowObjectPicker<StatDefinition>(null, false, null, PICKER_CONTROL_ID);
		}

		protected void OnRemoveCallback(ReorderableList list)
        {
			adjustmentProperty.DeleteArrayElementAtIndex(list.index);
		}

		protected void DrawElementCallback(Rect rect, int index, bool activated, bool focused)
        {
			var element = adjustmentProperty.GetArrayElementAtIndex(index);

			var propDef = element.FindPropertyRelative("definition");
			var propRect = rect;
			propRect.y += HEADER_SPACING; // 헤더 스페이싱
			propRect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.PropertyField(propRect, propDef);
			EditorGUI.EndDisabledGroup();

			var lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			var propOperator = element.FindPropertyRelative("operatorType");
			propRect.y += lineHeight;
			EditorGUI.PropertyField(propRect, propOperator);

			var propMagnitude = element.FindPropertyRelative("modifierMagnitude");
			propRect.y += lineHeight;
			EditorGUI.PropertyField(propRect, propMagnitude);

			// foldout에 따라 사이즈가 변할수 있어야한다.
			var propSourceTags = element.FindPropertyRelative("sourceTags");
			propRect.y += lineHeight * 4;
			EditorGUI.PropertyField(propRect, propSourceTags);

			var propTargetTags = element.FindPropertyRelative("targetTags");
			propRect.y += lineHeight;
			EditorGUI.PropertyField(propRect, propTargetTags);
		}

		protected float ElementHeightCallback(int index)
		{
			var elementCount = 12;
			return (EditorGUIUtility.singleLineHeight * elementCount) + (EditorGUIUtility.standardVerticalSpacing * elementCount) + VERTICAL_SPACING;
		}
	}
}


