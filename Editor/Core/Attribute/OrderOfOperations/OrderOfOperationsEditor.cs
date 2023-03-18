using UnityEngine;
using UnityEditorInternal;
using SRFramework.Attribute;


namespace UnityEditor
{
	[CustomEditor(typeof(OrderOfOperations))]
	public class OrderOfOperationsEditor : Editor
	{
		private ReorderableList reorderableList;
		private SerializedProperty operatorsProperty;


		private void OnEnable()
		{
			operatorsProperty = serializedObject.FindProperty(nameof(OrderOfOperations.operators));

			if (operatorsProperty != null && operatorsProperty.isArray)
			{
				reorderableList = new ReorderableList(serializedObject, operatorsProperty, true, true, true, true);
				reorderableList.drawHeaderCallback = DisplayHeaderCallback;
				reorderableList.drawElementCallback = DrawElementCallback;
				reorderableList.elementHeightCallback = ElementHeightCallback;
				reorderableList.onCanAddCallback = list => false;
				reorderableList.onCanRemoveCallback = list => false;
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			reorderableList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void DisplayHeaderCallback(Rect rect)
		{
			if (operatorsProperty == null)
				return;

			int iIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			EditorGUI.LabelField(rect, operatorsProperty.displayName);
			EditorGUI.indentLevel = iIndent;
		}

		protected void DrawElementCallback(Rect rect, int index, bool activated, bool focused)
		{
			var element = operatorsProperty.GetArrayElementAtIndex(index);
			var typeProperty = element.FindPropertyRelative(nameof(Operator.type));
			var autoRoundProperty = element.FindPropertyRelative(nameof(Operator.modifierAutoRound));

			// type은 수정할 수 없다.
			GUI.enabled = false;
			EditorGUI.PropertyField(rect, typeProperty, new GUIContent(""));
			GUI.enabled = true;

			rect.y += EditorGUIUtility.standardVerticalSpacing + 5;
			autoRoundProperty.boolValue = EditorGUI.ToggleLeft(rect, autoRoundProperty.displayName, autoRoundProperty.boolValue);
		}

		protected float ElementHeightCallback(int index)
		{
			return EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight * 2;
		}
	}
}

