using SlimeRPG.Framework.Tag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class GameplayAbility : ScriptableObject
    {
        public AbilityTags tags;
        public GameplayEffect cost;
        public GameplayEffect coolDown;
        public List<AbilityTriggerData> abilityTriggers;
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