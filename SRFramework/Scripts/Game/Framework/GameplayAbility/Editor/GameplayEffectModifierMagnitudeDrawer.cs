using System;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(GameplayEffectModifierMagnitude), true)]
    public class GameplayEffectModifierMagnitudeDrawer : PropertyDrawer
    {
        private SerializedProperty serializedProperty;
        private SerializedProperty valueProperty;

        private SerializedProperty ValueProperty
        {
            get
            {
                if (serializedProperty == null)
                    return null;

                if (valueProperty != null)
                    if (valueProperty.serializedObject.targetObject != null)
                        return valueProperty;

                valueProperty = new SerializedObject(serializedProperty.FindPropertyRelative("magnitude").objectReferenceValue)
                    .FindProperty("value");
                return valueProperty;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            serializedProperty = property;

            // 에셋을 자동으로 추가해줘야하는데... 가능한가 여기서?
            EditorGUI.BeginProperty(position, label, property);
            var lineHeight = EditorGUIUtility.singleLineHeight;
            position.height = lineHeight;

            var magnitudeCalculation = property.FindPropertyRelative("magnitudeCalculation");
            var roundToInt = property.FindPropertyRelative("roundToInt");
            var multiplier = property.FindPropertyRelative("multiplier");
            var magnitude = property.FindPropertyRelative("magnitude");

            bool calculationChanged = false;
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, magnitudeCalculation);
            if (EditorGUI.EndChangeCheck())
                calculationChanged = true;

            position.y += lineHeight;
            EditorGUI.PropertyField(position, roundToInt);

            position.y += lineHeight;
            EditorGUI.PropertyField(position, multiplier);

            var calculationType = (GameplayEffectModifierMagnitude.MagnitudeCalculation)Enum.ToObject(typeof(GameplayEffectModifierMagnitude.MagnitudeCalculation), magnitudeCalculation.enumValueIndex);
            if (calculationChanged || magnitude.objectReferenceValue == null)
                ChangeMagnitude(magnitude, calculationType);

            switch (calculationType)
            {
                case GameplayEffectModifierMagnitude.MagnitudeCalculation.scalableFloat:
                    if (magnitude != null && magnitude.objectReferenceValue)
                    {
                        position.y += lineHeight;
                        EditorGUI.PropertyField(position, ValueProperty);
                        ValueProperty.serializedObject.ApplyModifiedProperties();
                    }
                    break;
            }

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }

        void ChangeMagnitude(SerializedProperty magnitude, GameplayEffectModifierMagnitude.MagnitudeCalculation calculation)
        {
            if (magnitude.objectReferenceValue)
            {
                UnityEngine.Object.DestroyImmediate(magnitude.objectReferenceValue, true);
                AssetDatabase.SaveAssets();
            
                valueProperty = null;
                magnitude.objectReferenceValue = null;
            }

            switch (calculation)
            {
                case GameplayEffectModifierMagnitude.MagnitudeCalculation.scalableFloat:
                    var newInstance = ScriptableObject.CreateInstance<ScalableMagnitude>();
                    newInstance.name = "ScalableMagnitude";
                    AssetDatabase.AddObjectToAsset(newInstance, serializedProperty.serializedObject.targetObject);
                    AssetDatabase.SaveAssets();
                    
                    magnitude.objectReferenceValue = newInstance;
                    break;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4;
        }
    }
}
