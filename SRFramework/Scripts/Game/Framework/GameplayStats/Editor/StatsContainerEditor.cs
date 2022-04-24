using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.StatsSystem.StatsContainers;


namespace UnityEditor
{
	[CustomEditor(typeof(StatsContainer))]
	public class StatsContainerEditor : Editor
	{
		private bool debug;

		private List<StatDefinition> definitions = new List<StatDefinition>();
		private SerializedProperty collectionProperty;

		private bool IsRuntimeViewable => Application.isPlaying && Target.IsSetup;

		private StatsContainer Target => (StatsContainer)target;


		private void OnEnable()
		{
			collectionProperty = serializedObject.FindProperty("collection");
			RebuildDisplay();
		}

		public override void OnInspectorGUI()
		{
			if (Application.isPlaying)
				GUI.enabled = false;

			serializedObject.Update();

			debug = EditorGUILayout.Toggle("Debug", debug);

			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.PropertyField(collectionProperty);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);

			if (!Application.isPlaying)
			{
				foreach (var statDefinition in definitions)
				{
					EditorGUILayout.BeginHorizontal();
					DrawOverrideSelect(statDefinition);
					DrawInputOverrideOrDefault(statDefinition);
					EditorGUILayout.EndHorizontal();
				}
			}
			else if (IsRuntimeViewable)
			{
				foreach (var statDefinition in definitions)
					PrintRuntimeStats(statDefinition);
			}

			serializedObject.ApplyModifiedProperties();

			if (EditorGUI.EndChangeCheck())
				RebuildDisplay();

			GUI.enabled = true;

			if (debug)
			{
				GUI.enabled = false;
				EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
				base.OnInspectorGUI();
				GUI.enabled = true;
			}
		}

		void PrintRuntimeStats(StatDefinition definition)
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			EditorGUILayout.BeginHorizontal();
			DrawInputOverrideOrDefault(definition);
			DrawOverrideSelect(definition);
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel++;

			var record = Target.GetRecord(definition);
			if (record != null)
			{
				EditorGUI.indentLevel++;
				PrintModifier("Add", record.modifierAdd);
				PrintModifier("Subtract", record.modifierSubtract);
				PrintModifier("Multiply", record.modifierMultiply);
				PrintModifier("Divide", record.modifierDivide);
				EditorGUI.indentLevel--;
			}

			if (record != null)
				EditorGUILayout.FloatField("Last Retrieved Value", record.LastRetrievedValue);

			EditorGUI.indentLevel--;

			EditorGUILayout.EndVertical();
		}

		void PrintModifier(string title, StatModifierCollection modifiers)
		{
			if (modifiers == null || modifiers.ListValues.Count == 0) 
				return;

			EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

			foreach (var val in modifiers.ListValues)
				EditorGUILayout.FloatField(val.id, val.value);
		}

		void DrawOverrideSelect(StatDefinition definition)
		{
			var @override = false;

			if (Target.overrides.Has(definition))
				@override = true;

			var oldResult = @override;
			var result = EditorGUILayout.Toggle(@override, GUILayout.Width(25));
			
			if (result == oldResult) 
				return;

			if (result == false)
            {
				var o = Target.overrides.Get(definition);
				var overridesList = serializedObject.FindProperty("overrides").FindPropertyRelative("overrides");
				var index = Target.overrides.overrides.IndexOf(o);
				var overrideProperty = overridesList.GetArrayElementAtIndex(index);
				var magnitude = overrideProperty.FindPropertyRelative("value.magnitude");
				if (magnitude != null)
                {
					DestroyImmediate(magnitude.objectReferenceValue, true);
					AssetDatabase.SaveAssets();
					magnitude.objectReferenceValue = null;
					overridesList.DeleteArrayElementAtIndex(index);
				}
				Target.overrides.Remove(definition);
			}
			else if (oldResult == false)
				Target.overrides.Add(new StatDefinitionOverride{definition = definition, value = new GameplayEffectModifierMagnitude()});

			EditorUtility.SetDirty(target);
		}

		void DrawInputOverrideOrDefault(StatDefinition definition)
		{
			if (Target.overrides.Has(definition))
				DrawInputOverride(definition);
			else
				DrawInputDefault(definition);
		}

		void DrawInputOverride(StatDefinition definition)
		{
			if (!Target.overrides.Has(definition)) 
				return;

			var @override = Target.overrides.Get(definition);
			
			EditorGUI.BeginChangeCheck();
			DrawInputField(@override);
			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(target);
		}

		void DrawInputDefault(StatDefinition definition)
		{
			EditorGUI.BeginDisabledGroup(true);
			DrawInputField(definition);
			EditorGUI.EndDisabledGroup();
		}

		void DrawInputField(StatDefinitionOverride o)
        {
			var overridesList = serializedObject.FindProperty("overrides").FindPropertyRelative("overrides");
			if (overridesList == null)
				return;

			var index = Target.overrides.overrides.IndexOf(o);
			if (index >= overridesList.arraySize)
				return;

			// override�� Property�� ã�� �� �����Ƿ� ���� �������ش�.
			var overrideProperty = overridesList.GetArrayElementAtIndex(index);
			var selectorProperty = overrideProperty.FindPropertyRelative("value");

			EditorGUILayout.BeginVertical();
			GUILayout.Label(new GUIContent(o.definition.DisplayName));
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(selectorProperty, new GUIContent("Magnitude Value"));
			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();
		}

		void DrawInputField(StatDefinition stat)
		{
			var selectorProperty = new SerializedObject(stat).FindProperty("value");
			
			EditorGUILayout.BeginVertical();
			GUILayout.Label(new GUIContent(stat.DisplayName));
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(selectorProperty, new GUIContent("Magnitude Value"));
			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();
		}

		void RebuildDisplay()
		{
			definitions.Clear();

			List<StatDefinition> results;
			if (Target.collection == null)
				results = StatDefinitionsCompiled.GetDefinitions(StatsSettings.Current.DefaultStats);
			else
				results = StatDefinitionsCompiled.GetDefinitions(Target.collection);

			if (results == null) 
				return;

			definitions = results;
			definitions = definitions
				.OrderByDescending(d => d.SortIndex)
				.ToList();

			if (Target.overrides.overrides != null)
				Target.overrides.Clean();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
