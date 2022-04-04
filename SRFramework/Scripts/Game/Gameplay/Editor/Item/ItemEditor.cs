using UnityEngine;
using SlimeRPG.Gameplay.Item;


namespace UnityEditor
{
    public class ItemEditor
    {
        SerializedProperty nameProperty;
        SerializedProperty iconProperty;
        SerializedProperty descriptionProperty;
        SerializedProperty rarityProperty;
        SerializedProperty lootObjectProperty;


        public void Init(SerializedObject target)
        {
            nameProperty = target.FindProperty(nameof(Item.itemName));
            iconProperty = target.FindProperty(nameof(Item.icon));
            descriptionProperty = target.FindProperty(nameof(Item.description));
            rarityProperty = target.FindProperty(nameof(Item.rarity));
            lootObjectProperty = target.FindProperty(nameof(Item.lootObject));
        }

        public void OnGUI()
        {
            
            EditorGUILayout.PropertyField(nameProperty);
            EditorGUILayout.PropertyField(iconProperty);
            if (iconProperty.objectReferenceValue)
            {
                var preview = AssetPreview.GetAssetPreview(iconProperty.objectReferenceValue);
                GUILayout.Label(preview, GUILayout.Height(64));
                EditorGUILayout.Space();
            }
            EditorGUILayout.PropertyField(descriptionProperty, GUILayout.MinHeight(128));
            EditorGUILayout.PropertyField(rarityProperty);
            EditorGUILayout.PropertyField(lootObjectProperty);

            if (lootObjectProperty.objectReferenceValue)
            {
                var preview = AssetPreview.GetAssetPreview(lootObjectProperty.objectReferenceValue);
                GUILayout.Label(preview, GUILayout.Height(64));
                EditorGUILayout.Space();
            }
        }
            
    }
}
