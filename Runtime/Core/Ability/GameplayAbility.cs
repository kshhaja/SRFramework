using SRFramework.Tag;
using SRFramework.Effect;
using System.Collections.Generic;
using UnityEngine;
using SRFramework.Ability.Spec;


namespace SRFramework.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Ability/Base Ability", order = 1)]
    public class GameplayAbility : ScriptableObject
    {
        public AbilityTags tags;
        public GameplayEffect cost;
        public GameplayEffect coolDown;
        public List<AbilityTriggerData> abilityTriggers;

        public virtual AbstractGameplayAbilitySpec CreateNew(AbilitySystemComponent instigator, AbilitySystemComponent source, float level = 1)
        {
            return new GameplayAbilitySpec<GameplayAbility>(this, instigator, source, level);
        }
    }

    public enum GameplayAbilityTriggerSouce
    {
        gameplayEvent,
        ownedTagAdded,
        ownedTagPresent,
    }

    public struct AbilityTriggerData
    {
        public GameplayTagContainer triggerTag;
        public GameplayAbilityTriggerSouce triggerSource;
    }
}