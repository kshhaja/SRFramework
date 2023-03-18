using UnityEngine;
using SlimeRPG.Gameplay.Character;


namespace SlimeRPG.Gameplay.Item
{
    public class Item : ScriptableObject
    {
        public enum Rarity
        {
            normal,
            magic,
            rare,
            unique,
        }

        public string itemName;
        public Sprite icon;
        public string description;

        public Rarity rarity;
        public GameObject lootObject;

        public virtual bool CheckRequirements(CharacterBase character)
        {
            return true;
        }
    }
}