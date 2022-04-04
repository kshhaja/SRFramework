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

        [SerializeField]
        private StatsContainer statsContainer;

        public StatsContainer StatsContainer => statsContainer;

        protected virtual void Awake()
        {
            // attribute값을 변경하면 리소스에 영향이 가기때문에 복사하여 사용.
            statsContainer = statsContainer?.CreateRuntimeCopy();
        }

        public bool ApplyGameplayEffect(GameplayEffectScriptableObject ge)
        {
            if (ge == null)
                return true;

            switch (ge.gameplayEffect.duration.policy)
            {
                case EDurationPolicy.HasDuration:
                case EDurationPolicy.Infinite:
                    // appliedGameplayEffects.Add(new GameplayEffectContainer() { });
                    break;
                case EDurationPolicy.Instant:
                    ApplyGameplayEffectInternal(ge);
                    break;
            }

            return true;
        }

        void ApplyGameplayEffectInternal(GameplayEffectScriptableObject ge)
        {
            if (ge.gameplayEffect.adjustment == null)
                return;

            List<ModifierGroup> GetAllDamage(StatsAdjustment adjustment)
            {
                List<ModifierGroup> damages = new List<ModifierGroup>();
                foreach (var adj in adjustment.adjustment)
                {
                    // custom damage definition type
                    //if (adj.definition is DamageDefinition)
                        // damages.Add(adj);
                }

                return damages;
            }

            // 데미지 추출해서 따로 처리한다.
            var all = GetAllDamage(ge.gameplayEffect.adjustment);

            ge.gameplayEffect.adjustment.ApplyAdjustment(statsContainer);
        }
    }
}
