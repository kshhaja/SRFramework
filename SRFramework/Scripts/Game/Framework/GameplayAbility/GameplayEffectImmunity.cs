using System;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    [Serializable]
    public struct GameplayEffectImmunity
    {
        [SerializeField] public GameplayTagRequirements grantedApplicationImmunityTags;
        [SerializeField] public GameplayEffectQuery grantedApplicationImmunityQuery;
    }
}
