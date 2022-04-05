using UnityEngine;
using SlimeRPG.Framework.StatsSystem;


namespace UnityEditor
{
    [CustomEditor(typeof(ItemModContainer))]
    public class ItemModContainerEditor : Editor
    {
        // protected SerializedProperty statCollectionProperty;
        protected SerializedProperty explicitProperty;
        protected SerializedProperty implicitProperty;

        private void OnEnable()
        {
            // statCollectionProperty = serializedObject.FindProperty(nameof(ItemModContainer.collection));
            explicitProperty = serializedObject.FindProperty(nameof(ItemModContainer.explicits));
            implicitProperty = serializedObject.FindProperty(nameof(ItemModContainer.implicits));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

#if false //Collection 프리셋을 base로 만들어주는 부분.
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(statCollectionProperty);

            if (EditorGUI.EndChangeCheck())
            {
                if (baseStats.objectReferenceValue)
                {
                    var item = baseStats.objectReferenceValue;
                    DestroyImmediate(item, true);

                    baseStats.objectReferenceValue = null;
                }

                if (statCollectionProperty.objectReferenceValue)
                {
                    var newInstance = CreateInstance(typeof(ItemStatsAdjustment));
                    newInstance.name = "base";
                    if (statCollectionProperty.objectReferenceValue)
                        (newInstance as ItemStatsAdjustment).SetupWith(statCollectionProperty.objectReferenceValue as StatDefinitionCollection);

                    AssetDatabase.AddObjectToAsset(newInstance, target);

                    baseStats.objectReferenceValue = newInstance;
                }
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(baseStats);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if (additionalStats.objectReferenceValue == null)
            {
                if (GUILayout.Button("Append Addtional Stats"))
                {
                    var newInstance = CreateInstance(typeof(AddtionalStatsAdjustment));
                    newInstance.name = "additional";
                    AssetDatabase.AddObjectToAsset(newInstance, target);

                    additionalStats.objectReferenceValue = newInstance;
                }
            }
            else
            {
                if (GUILayout.Button("Remove Additional Stats"))
                {
                    var item = additionalStats.objectReferenceValue;
                    DestroyImmediate(item, true);

                    additionalStats.objectReferenceValue = null;
                }

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(additionalStats);
                EditorGUI.EndDisabledGroup();
            }
#endif
            if (explicitProperty.objectReferenceValue == null)
            {
                var newInstance = CreateInstance(typeof(GameplayModContainer));
                newInstance.name = "explicits";

                AssetDatabase.AddObjectToAsset(newInstance, target);
                AssetDatabase.SaveAssets();

                explicitProperty.objectReferenceValue = newInstance;
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(explicitProperty);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if (implicitProperty.objectReferenceValue == null)
            {
                if (GUILayout.Button("Add Implicits"))
                {
                    var newInstance = CreateInstance(typeof(GameplayModContainer));
                    newInstance.name = "implicits";
                    AssetDatabase.AddObjectToAsset(newInstance, target);
                    AssetDatabase.SaveAssets();

                    implicitProperty.objectReferenceValue = newInstance;
                }
            }
            else
            {
                if (GUILayout.Button("Remove Implicits"))
                {
                    var item = implicitProperty.objectReferenceValue;
                    DestroyImmediate(item, true);
                    AssetDatabase.SaveAssets();

                    implicitProperty.objectReferenceValue = null;
                }

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(implicitProperty);
                EditorGUI.EndDisabledGroup();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}