using System;
using UnityEngine;
using SRFramework.Tag;


namespace SRFramework.Effect
{
    [Serializable]
    public struct GameplayEffectTags
    {
        /// <summary>
        /// The tag that defines this gameplay effect
        /// </summary>
        [SerializeField] public GameplayTagContainer assetTag;

        /// <summary>
        /// The tags this GE grants to the ability system character
        /// </summary>
        [SerializeField] public GameplayTagContainer grantedTags;

        /// <summary>
        /// These tags determine if the GE is considered 'on' or 'off'
        /// </summary>
        [SerializeField] public GameplayTagRequirements ongoingTagRequirements;

        /// <summary>
        /// These tags must be present for this GE to be applied
        /// </summary>
        [SerializeField] public GameplayTagRequirements applicationTagRequirements;

        /// <summary>
        /// Tag requirements that will remove this GE
        /// </summary>
        [SerializeField] public GameplayTagRequirements removalTagRequirements;

        /// <summary>
        /// Remove GE that match these tags
        /// </summary>
        [SerializeField] public GameplayTagContainer removeGameplayEffectsWithTags;
    }

}
