using SRFramework.Ability;


namespace UnityEditor
{
    [CustomEditor(typeof(ProjectileAbilityBase), true)]
    public class ProjectileAbilityBaseEditor : AbilityBaseEditor
    {
        protected SerializedProperty projectileProperty;


        protected override void OnEnable()
        {
            base.OnEnable();
            projectileProperty = serializedObject.FindProperty(nameof(ProjectileAbilityBase.projectile));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(projectileProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
