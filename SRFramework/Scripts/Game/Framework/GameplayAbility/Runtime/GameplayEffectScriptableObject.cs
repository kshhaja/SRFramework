using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.StatsSystem;
using SlimeRPG.Framework.StatsSystem.StatsContainers;

namespace SlimeRPG.Framework.Ability
{
    [CreateAssetMenu(menuName = "Gameplay/Ability/Effect Definition")]
    public class GameplayEffectScriptableObject : ScriptableObject
    {
        public GameplayEffectDuration duration;

        public StatAdjustmentCollection modifiers;
        public GameplayExecutionCollection execution;
        public List<ConditionalGameplayEffectContainer> conditionalEffects;

        public GameplayEffectTags gameplayEffectTags;
        public GameplayEffectPeriod period;        
    }

    public class GameplayEffectSpec
    {
        public AbilitySystemCharacter source;
        public List<AbilitySystemCharacter> targets = new List<AbilitySystemCharacter>();
        public GameplayEffectScriptableObject effect;

        public float level;
        public float durationRemaining;
        public float totalDuration;
        public GameplayEffectPeriod periodDefinition;
        public float timeUntilPeriodTick;


        GameplayEffectSpec(GameplayEffectScriptableObject effect, AbilitySystemCharacter source, float level)
        {
            this.effect = effect;
            this.source = source;
            this.level = level;

            if (effect.duration.modifier != 0f)
            {
                durationRemaining = effect.duration.modifier * effect.duration.multiplier;
                totalDuration = durationRemaining;
            }

            timeUntilPeriodTick = effect.period.interval;
            if (effect.period.executeOnApplication)
            {
                timeUntilPeriodTick = 0;
            }
        }

        public static GameplayEffectSpec CreateNew(GameplayEffectScriptableObject effect, AbilitySystemCharacter source, float level = 1)
        {
            return new GameplayEffectSpec(effect, source, level);
        }

        public GameplayEffectSpec AddTarget(AbilitySystemCharacter target)
        {
            targets.Add(target);
            return this;
        }

        public void SetTotalDuration(float totalDuration)
        {
            this.totalDuration = totalDuration;
        }

        public GameplayEffectSpec SetDuration(float duration)
        {
            durationRemaining = duration;
            return this;
        }

        public GameplayEffectSpec UpdateRemainingDuration(float deltaTime)
        {
            durationRemaining -= deltaTime;
            return this;
        }

        public GameplayEffectSpec UpdatePeriod(float deltaTime, out bool execute)
        {
            timeUntilPeriodTick -= deltaTime;
            execute = false;
            if (timeUntilPeriodTick <= 0)
            {
                timeUntilPeriodTick = effect.period.interval;

                if (effect.period.interval > 0)
                {
                    execute = true;
                }
            }

            return this;
        }

        public GameplayEffectSpec SetLevel(float level)
        {
            this.level = level;
            // try evaluate all modifiers
            return this;
        }

        public void ApplyEffectTo(AbilitySystemCharacter target)
        {
            if (target == null)
                return;

            if (effect.modifiers)
                effect.modifiers.ApplyAdjustment(target.StatsContainer);

            if (effect.execution)
                effect.execution.TryExecute(target, 1);
        }
    }
}
