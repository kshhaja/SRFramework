using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Gameplay.Item;
using System.Linq;

namespace UnityEditor
{
    [CustomEditor(typeof(ArmourItem))]
    public class ArmourItemEditor : EquipmentItemEditor
    {
        protected SerializedProperty typeProperty;
        protected SerializedProperty damageTakenEffectsProperty;

        protected List<string> availableDamageTakenEffects;


        protected override void OnEnable()
        {
            base.OnEnable();

            typeProperty = serializedObject.FindProperty(nameof(ArmourItem.type));
            damageTakenEffectsProperty = serializedObject.FindProperty(nameof(ArmourItem.damageTakenEffects));

            var lookup = typeof(ArmourItem.DamageTakenEffect);
            availableDamageTakenEffects = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
                .Select(type => type.Name)
                .ToList();
        }

        protected override void OnDefaultGUI()
        {
            base.OnDefaultGUI();
            EditorGUILayout.PropertyField(typeProperty, true);
        }

        protected override void OnEffectGUI()
        {
            base.OnEffectGUI();

            int choice = EditorGUILayout.Popup("Add new DamageTaken Effect", -1, availableDamageTakenEffects.ToArray());

            if (choice != -1)
            {
                var newInstance = CreateInstance(availableDamageTakenEffects[choice]);

                AssetDatabase.AddObjectToAsset(newInstance, target);
                AssetDatabase.SaveAssets();

                damageTakenEffectsProperty.InsertArrayElementAtIndex(damageTakenEffectsProperty.arraySize);
                damageTakenEffectsProperty.GetArrayElementAtIndex(damageTakenEffectsProperty.arraySize - 1).objectReferenceValue = newInstance;
            }

            int toDelete = -1;
            for (int i = 0; i < damageTakenEffectsProperty.arraySize; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                var item = damageTakenEffectsProperty.GetArrayElementAtIndex(i);

                Editor ed = null;
                CreateCachedEditor(item.objectReferenceValue, null, ref ed);
                ed.OnInspectorGUI();
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("-", GUILayout.Width(32)))
                    toDelete = i;

                EditorGUILayout.EndHorizontal();
            }

            if (toDelete != -1)
            {
                var item = damageTakenEffectsProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
                DestroyImmediate(item, true);
                AssetDatabase.SaveAssets();

                damageTakenEffectsProperty.DeleteArrayElementAtIndex(toDelete);
                damageTakenEffectsProperty.DeleteArrayElementAtIndex(toDelete);
            }
        }
    }
}
