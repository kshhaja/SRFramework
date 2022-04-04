using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    /// <summary>
    /// Ability�� �����ϴ� ���� �������� ������ abstract Ŭ����
    /// 
    /// Issue   : Inspector.ContextMenu.Reset ��ư�� ������ ��,
    ///           ������ ���� GameplayEffectScriptableObject�� �����ִ�.
    ///           ��а� Reset�� ����.
    /// </summary>
    public abstract class AbstractAbilityScriptableObject : ScriptableObject
    {
        public string abilityName;
        public AbilityTags abilityTags;
        
        public GameplayEffectScriptableObject effect;
        public GameplayEffectScriptableObject cost;
        public GameplayEffectScriptableObject coolDown;
        public AbstractAbilityScriptableObject secondaryAbility;
    }
}