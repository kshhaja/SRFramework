using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Gameplay.Character;


namespace SlimeRPG.Gameplay.Item
{
    public enum Armour
    {
        bodyArmour,
        boots,
        gloves,
        helmet,
        shield,
    }

    [CreateAssetMenu(menuName = "Gameplay/Item/Armour")]
    public class ArmourItem : EquipmentItem
    {
        public Armour type;

        
        public abstract class DamageTakenEffect : ScriptableObject
        {
            public string description;

            public virtual void OnDamageTaken(CharacterBase source, CharacterBase target)
            {
            }
        }

        public List<DamageTakenEffect> damageTakenEffects;
    }
}
