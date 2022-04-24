using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SlimeRPG.Gameplay.Character.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Ability/Projectile/Projectile Action Base", order = 1)]
    public class ProjectileAbilityBase : AbilityBase
    {
        public Projectile projectile;
    }
}