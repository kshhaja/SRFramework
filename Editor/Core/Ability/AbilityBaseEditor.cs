using SRFramework.Ability;


namespace UnityEditor
{
    [CustomEditor(typeof(AbilityBase), true)]
    public class AbilityBaseEditor : Editor
    {
        protected AbilityBase targetCache;
        protected AbstractAbilityEditor editor;

        protected SerializedProperty levelProperty;
        protected SerializedProperty qualityProperty;
        protected SerializedProperty clipProperty;
        protected SerializedProperty effectProperty;


        protected virtual void OnEnable()
        {
            targetCache = target as AbilityBase;
            
            levelProperty = serializedObject.FindProperty(nameof(AbilityBase.level));
            qualityProperty = serializedObject.FindProperty(nameof(AbilityBase.quality));
            clipProperty = serializedObject.FindProperty(nameof(AbilityBase.clip));
            effectProperty = serializedObject.FindProperty(nameof(AbilityBase.effectContainers));

            editor = new AbstractAbilityEditor();
            editor.Init(targetCache, serializedObject);
        }

        public override void OnInspectorGUI()
        {
            editor.OnBaseGUI();

            EditorGUILayout.PropertyField(levelProperty);
            EditorGUILayout.PropertyField(qualityProperty);
            EditorGUILayout.PropertyField(clipProperty);
            EditorGUILayout.PropertyField(effectProperty);

            editor.OnAbilityGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
