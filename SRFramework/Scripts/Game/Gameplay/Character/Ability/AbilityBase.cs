using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.Ability;
using System;
using System.Linq;
using SlimeRPG.Framework.Tag;


namespace SlimeRPG.Gameplay.Character.Ability
{
    public abstract class AbilityBase : GameplayAbility
    {
        public int level = 1;
        public int quality = 0;
        public Sprite icon;
        public AnimationClip clip;

        public List<GameplayEffectContainer> effectContainers = new List<GameplayEffectContainer>();
    }
}
