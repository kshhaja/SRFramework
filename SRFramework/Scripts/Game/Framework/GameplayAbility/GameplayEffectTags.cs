using System;
using UnityEngine;
using SlimeRPG.Framework.Tag;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectTags
    {
        /// <summary>
        /// The tag that defines this gameplay effect
        /// </summary>
        [SerializeField] public GameplayTag AssetTag;

        /// <summary>
        /// The tags this GE grants to the ability system character
        /// </summary>
        [SerializeField] public GameplayTagContainer GrantedTags;

        /// <summary>
        /// These tags determine if the GE is considered 'on' or 'off'
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer OngoingTagRequirements;

        /// <summary>
        /// These tags must be present for this GE to be applied
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer ApplicationTagRequirements;

        /// <summary>
        /// Tag requirements that will remove this GE
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer RemovalTagRequirements;

        /// <summary>
        /// Remove GE that match these tags
        /// </summary>
        [SerializeField] public GameplayTagContainer RemoveGameplayEffectsWithTag;
    }

}
