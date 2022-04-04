using UnityEngine;
using System;
using SlimeRPG.Framework.Tag;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct AbilityTags
    {
        public GameplayTag assetTag;
        public GameplayTagContainer cancelAbilitiesWithTags;
        public GameplayTagContainer blockAbilitiesWithTags;
        public GameplayTagContainer activationOwnedTags;
        public GameplayTagRequireIgnoreContainer ownerTags;
        public GameplayTagRequireIgnoreContainer sourceTags;
        public GameplayTagRequireIgnoreContainer targetTags;
    }
}