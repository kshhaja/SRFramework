using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.Ability;
using System;
using System.Linq;
using SlimeRPG.Framework.Tag;


namespace SlimeRPG.Gameplay.Character.Ability
{
    // 이 클래스부터는 커스텀 클래스.
    public abstract class AbilityBase : GameplayAbility
    {
        // GAS 내부에서는 필요없는 데이터. 혹은 커스텀으로 동작해야하는 데이터
        public int level = 1;
        public int quality = 0;
        public Sprite icon;
        public AnimationClip clip;

        // 실질적으로 적용할 효과들.
        public List<GameplayEffectContainer> effectContainers = new List<GameplayEffectContainer>();
    }
}
