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


        public void Attack(CharacterBase source, CharacterBase target)
        {
            foreach (var effect in attackEffects)
                effect.OnAttack(source, target);

            // applyGameplayEffect 내부에서 데미지 공식을 호출해줄테니 따로 계산할 필요는 없을듯.
            stat.ApplyModContainer(target.attributeController.StatsContainer, 1);

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
