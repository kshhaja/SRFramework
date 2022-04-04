using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    /// <summary>
    /// Ability를 구성하는 가장 기초적인 형태의 abstract 클래스
    /// 
    /// Issue   : Inspector.ContextMenu.Reset 버튼을 눌렀을 때,
    ///           하위에 붙은 GameplayEffectScriptableObject가 남아있다.
    ///           당분간 Reset은 금지.
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