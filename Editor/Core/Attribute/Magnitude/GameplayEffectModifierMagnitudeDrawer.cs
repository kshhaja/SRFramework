using System;
using UnityEngine;
using SRFramework.Attribute;


namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(GameplayEffectModifierMagnitude), true)]
    public class GameplayEffectModifierMagnitudeDrawer : PropertyDrawer
    {
        private SerializedProperty serializedProperty;
        private SerializedProperty curveProperty;

        const int defaultKeyCount = 40;

        private SerializedProperty CurveProperty
        {
            get
            {
                if (serializedProperty == null)
                    return null;

                if (curveProperty != null)
                    if (curveProperty.serializedObject.targetObject != null)
                        return curveProperty;

                curveProperty = new SerializedObject(serializedProperty.FindPropertyRelative("magnitude").objectReferenceValue).FindProperty("curve");
                return curveProperty;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();

            serializedProperty = property;

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
            {
                ChangeMagnitude(magnitude, calculationType);
            }

            switch (calculationType)
            {
                case GameplayEffectModifierMagnitude.MagnitudeCalculation.scalableFloat:
                    if (magnitude != null && magnitude.objectReferenceValue)
                    {
                        position.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(position, CurveProperty);
                        // CurveProperty.serializedObject.ApplyModifiedProperties();
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
            
                curveProperty = null;
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
