using SlimeRPG.Framework.Ability;
using SlimeRPG.Gameplay.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Gameplay.Item
{
    public enum Weapon
    {
        axe,
        bow,
        claw,
        dagger,
        fishingRod,
        mace,
        sceptre,
        staff,
        sword,
        wand
    }

    [CreateAssetMenu(menuName = "Gameplay/Item/Weapon")]
    public class WeaponItem : EquipmentItem
    {
        public Weapon type;
        
        [Tooltip("play random sound when hit")]
        public AudioClip[] hitSounds;
        public AudioClip[] swingSounds;

        public abstract class WeaponAttackEffect : ScriptableObject
        {
            public string description;

            public virtual void OnAttack(CharacterBase source, CharacterBase target)
            {
            }

            public virtual void OnPostAttack(CharacterBase source, CharacterBase target)
            {
            }
        }

        public List<WeaponAttackEffect> attackEffects;
        private GameplayEffectSpec spec;


        public void RegisterGameplayEffectToSelf(GameplayEffectSpec spec)
        {
            this.spec = spec;
        }

        public void Attack(CharacterBase source, CharacterBase target)
        {
            // wait for trigger by animation

            foreach (var effect in attackEffects)
                effect.OnAttack(source, target);

            target.abilitySystem.ApplyGameplayEffect(spec);
            // stat.ApplyModContainer(target.abilitySystem.container, 1);

            foreach (var effect in attackEffects)
                effect.OnPostAttack(source, target);
        }

        public bool RandomHitSound()
        {
            if (hitSounds == null || hitSounds.Length == 0)
            { 
                // return SFXManager.GetDefaultHit();
            }

            return hitSounds[Random.Range(0, hitSounds.Length)];
        }

        public bool RandomSwingSound()
        {
            if (swingSounds == null || swingSounds.Length == 0)
            {
                // return SFXManager.GetDefaultSwingSound();
            }

            return swingSounds[Random.Range(0, swingSounds.Length)];
        }
    }
}
