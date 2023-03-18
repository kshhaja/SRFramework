using UnityEngine;
using System;
using SRFramework.Tag;


namespace SRFramework.Ability
{
    [Serializable]
    public struct AbilityTags
    {
        public GameplayTagContainer abilityTags;
        public GameplayTagContainer cancelAbilitiesWithTag;
        public GameplayTagContainer blockAbilitiesWithTag;
        
        public GameplayTagContainer activationOwnedTags;
        public GameplayTagContainer activationRequiredTags;
        public GameplayTagContainer activationBlockedTags;

        public GameplayTagContainer sourceRequiredTags;
        public GameplayTagContainer sourceBlockedTags;

        public GameplayTagContainer targetRequiredTags;
        public GameplayTagContainer targetBlockedTags;
    }
}