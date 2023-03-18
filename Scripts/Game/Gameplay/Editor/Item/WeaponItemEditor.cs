using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using SlimeRPG.Gameplay.Item;


namespace UnityEditor
{
    [CustomEditor(typeof(WeaponItem))]
    public class WeaponItemEditor : EquipmentItemEditor
    {
        protected SerializedProperty typeProperty;
        protected SerializedProperty hitSoundsProperty;
        protected SerializedProperty swingSoundsProperty;
        protected SerializedProperty attackEffectsProperty;

        protected List<string> availableAttackEffects;


        protected override void OnEnable()
        {
            base.OnEnable();

            typeProperty = serializedObject.FindProperty(nameof(WeaponItem.type));
            attackEffectsProperty = serializedObject.FindProperty(nameof(WeaponItem.attackEffects));
            hitSoundsProperty = serializedObject.FindProperty(nameof(WeaponItem.hitSounds));
            swingSoundsProperty = serializedObject.FindProperty(nameof(WeaponItem.swingSounds));

            var lookup = typeof(WeaponItem.WeaponAttackEffect);
            availableAttackEffects = System.AppDomain.CurrentDomain.GetAssemblies()
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

        protected override void OnContentGUI()
        {
            base.OnContentGUI();
            EditorGUILayout.PropertyField(hitSoundsProperty, true);
            EditorGUILayout.PropertyField(swingSoundsProperty, true);
        }

        protected override void OnEffectGUI()
        {
            base.OnEffectGUI();

            int choice = EditorGUILayout.Popup("Add new Weapon Attack Effect", -1, availableAttackEffects.ToArray());

            if (choice != -1)
            {
                var newInstance = CreateInstance(availableAttackEffects[choice]);

                AssetDatabase.AddObjectToAsset(newInstance, target);
                AssetDatabase.SaveAssets();

                attackEffectsProperty.InsertArrayElementAtIndex(attackEffectsProperty.arraySize);
                attackEffectsProperty.GetArrayElementAtIndex(attackEffectsProperty.arraySize - 1).objectReferenceValue = newInstance;
            }

            int toDelete = -1;
            for (int i = 0; i < attackEffectsProperty.arraySize; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                var item = attackEffectsProperty.GetArrayElementAtIndex(i);

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
                var item = attackEffectsProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
                DestroyImmediate(item, true);
                AssetDatabase.SaveAssets();

                //need to do it twice, first time just nullify the entry, second actually remove it.
                attackEffectsProperty.DeleteArrayElementAtIndex(toDelete);
                attackEffectsProperty.DeleteArrayElementAtIndex(toDelete);
            }
        }
    }
}