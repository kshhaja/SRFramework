using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using SlimeRPG.Gameplay.Item;
using SlimeRPG.Gameplay.Item.Mod;


namespace UnityEditor
{
    public class EquipmentItemEditor : Editor
    {
        protected ItemEditor itemEditor;

        protected SerializedProperty levelProperty;
        protected SerializedProperty qualityProperty;
        protected SerializedProperty requirementProperty;
        protected SerializedProperty statProperty;
        protected SerializedProperty equippedEffectsProperty;

        protected List<string> availableEquippedEffects;


        protected virtual void OnEnable()
        {
            levelProperty = serializedObject.FindProperty(nameof(EquipmentItem.level));
            qualityProperty = serializedObject.FindProperty(nameof(EquipmentItem.quality));
            requirementProperty = serializedObject.FindProperty(nameof(EquipmentItem.requirement));
            statProperty = serializedObject.FindProperty(nameof(EquipmentItem.stat));
            equippedEffectsProperty = serializedObject.FindProperty(nameof(EquipmentItem.equippedEffects));

            itemEditor = new ItemEditor();
            itemEditor.Init(serializedObject);

            var lookup = typeof(EquipmentItem.EquippedEffect);
            availableEquippedEffects = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
                .Select(type => type.Name)
                .ToList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            OnDefaultGUI();
            EditorGUILayout.Space();

            OnContentGUI();
            EditorGUILayout.Space();

            OnEffectGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnDefaultGUI()
        {
            itemEditor.OnGUI();

            EditorGUILayout.PropertyField(levelProperty);
            EditorGUILayout.PropertyField(qualityProperty);
        }

        protected virtual void OnContentGUI()
        {
            EditorGUILayout.PropertyField(requirementProperty);
            
            if (statProperty.objectReferenceValue)
            {
                
                if (GUILayout.Button("Remove"))
                {
                    var explicitProperty = new SerializedObject((target as EquipmentItem).stat).FindProperty("explicits");
                    var implicitProperty = new SerializedObject((target as EquipmentItem).stat).FindProperty("implicits");
                    if (explicitProperty.objectReferenceValue)
                        DestroyImmediate(explicitProperty.objectReferenceValue, true);
                    if (implicitProperty.objectReferenceValue)
                        DestroyImmediate(implicitProperty.objectReferenceValue, true);

                    DestroyImmediate(statProperty.objectReferenceValue, true);
                    AssetDatabase.SaveAssets();
                    
                    statProperty.objectReferenceValue = null;
                }
            }
            else
            {
                if (GUILayout.Button("Create New"))
                {
                    var instance = CreateInstance<ItemModContainer>();
                    instance.name = "Stats";
                    statProperty.serializedObject.ApplyModifiedProperties();

                    AssetDatabase.AddObjectToAsset(instance, target);
                    AssetDatabase.SaveAssets();

                    statProperty.objectReferenceValue = instance;
                }
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(statProperty);
            EditorGUI.EndDisabledGroup();
        }

        protected virtual void OnEffectGUI()
        {
            int choice = EditorGUILayout.Popup("Add new Effect", -1, availableEquippedEffects.ToArray());

            if (choice != -1)
            {
                var newInstance = CreateInstance(availableEquippedEffects[choice]);

                AssetDatabase.AddObjectToAsset(newInstance, target);

                equippedEffectsProperty.InsertArrayElementAtIndex(equippedEffectsProperty.arraySize);
                equippedEffectsProperty.GetArrayElementAtIndex(equippedEffectsProperty.arraySize - 1).objectReferenceValue = newInstance;
            }

            Editor ed = null;
            int toDelete = -1;
            for (int i = 0; i < equippedEffectsProperty.arraySize; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                var item = equippedEffectsProperty.GetArrayElementAtIndex(i);

                CreateCachedEditor(item.objectReferenceValue, null, ref ed);

                ed.OnInspectorGUI();
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("-", GUILayout.Width(32)))
                {
                    toDelete = i;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (toDelete != -1)
            {
                var item = equippedEffectsProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
                DestroyImmediate(item, true);

                //need to do it twice, first time just nullify the entry, second actually remove it.
                equippedEffectsProperty.DeleteArrayElementAtIndex(toDelete);
                equippedEffectsProperty.DeleteArrayElementAtIndex(toDelete);
            }
        }
    }
}
