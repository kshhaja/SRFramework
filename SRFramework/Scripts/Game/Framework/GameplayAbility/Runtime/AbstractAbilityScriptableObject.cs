using System.Collections;
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
        public Sprite icon;
        public string description;

        public AbilityTags abilityTags;
        
        public GameplayEffectScriptableObject effect;
        public GameplayEffectScriptableObject cost;
        public GameplayEffectScriptableObject coolDown;
        public AbstractAbilityScriptableObject secondaryAbility;


        public abstract bool CanActivateAbility();
        public abstract IEnumerator TryActivateAbility();
        public abstract IEnumerator PreActivateAbility();
        public abstract IEnumerator ActivateAbility();
        public abstract void CancelAbility();
        public abstract void EndAbility();
    }
}