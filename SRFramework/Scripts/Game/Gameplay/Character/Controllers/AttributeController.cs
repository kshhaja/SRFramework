using UnityEngine;
using System.Collections.Generic;
using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.StatsSystem.StatsContainers;
using SlimeRPG.Framework.Ability;


namespace SlimeRPG.Gameplay.Character.Controller

{
    public class AttributeController : AbstractCharacterController
    {
        public enum Damage
        {
            physical,
            cold,
            fire,
            lightning,
            poison,
        }

        public enum Ailment
        {
            ignite,
            chill,
            freeze,
            shock,
            bleed,
            poison,
        }

        struct DamageAmount
        {
            Damage damage;
            int Amount;
        }


        [SerializeField]
        private StatsContainer statsContainer;

        public StatsContainer StatsContainer => statsContainer;
        private List<GameplayEffectScriptableObject> durationalGEList = new List<GameplayEffectScriptableObject>();

        protected virtual void Awake()
        {
            // attribute값을 변경하면 리소스에 영향이 가기때문에 복사하여 사용.
            statsContainer = statsContainer?.CreateRuntimeCopy();
        }
    }
}
