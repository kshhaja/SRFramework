using System;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Gameplay.Character;
using SlimeRPG.Framework.StatsSystem;


namespace SlimeRPG.Gameplay.Item
{
    [Serializable]
    public struct StatRequirement
    {
        public int level;
        public int strength;
        public int dexterity;
        public int intelligence;
    }

    public class EquipmentItem : Item
    {
        // maybe.. it's useless... just meta data
        public int level;

        [Range(0, 20)]
        public int quality;

        public StatRequirement requirement;

        // mod group으로 변경예정.
        // mod는 하나 이상의 스탯으로 이루어진 옵션이라고 보면 된다.
        public ItemModContainer stat;

        public abstract class EquippedEffect : ScriptableObject
        {
            public string description;
            public abstract void OnEquipped(CharacterBase character);
            public abstract void OnUnequipped(CharacterBase character);
        }

        public List<EquippedEffect> equippedEffects;


        public void EquippedBy(CharacterBase character)
        {
            foreach (var effect in equippedEffects)
                effect.OnEquipped(character);

            stat.ApplyModContainer(character.abilitySystem.container, 1);
        }

        public void UnequippedBy(CharacterBase character)
        {
            foreach (var effect in equippedEffects)
                effect.OnUnequipped(character);

            stat.RemoveModContainer(character.abilitySystem.container);
        }
    }
}
