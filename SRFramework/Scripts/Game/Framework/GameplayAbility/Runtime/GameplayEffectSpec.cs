

using SlimeRPG.Framework.StatsSystem;
using System.Collections.Generic;

namespace SlimeRPG.Framework.Ability
{
    public class GameplayEffectSpec
    {
        public AbilitySystemComponent source;
        public List<AbilitySystemComponent> targets = new List<AbilitySystemComponent>();
        public GameplayEffect effect;

        public float level;
        public float durationRemaining;
        public float totalDuration;
        public GameplayEffectPeriod periodDefinition;
        public float timeUntilPeriodTick;


        GameplayEffectSpec(GameplayEffect effect, AbilitySystemComponent source, float level)
        {
            this.effect = effect;
            this.source = source;
            this.level = level;

            if (effect.duration.magnitude.GetValue(level) != 0f)
            {
                durationRemaining = effect.duration.magnitude.GetValue(level)/* * effect.duration.multiplier*/;
                totalDuration = durationRemaining;
            }

            timeUntilPeriodTick = effect.period.magnitude.GetValue(level);
            if (effect.period.executeOnApplication)
            {
                timeUntilPeriodTick = 0;
            }
        }

        public static GameplayEffectSpec CreateNew(GameplayEffect effect, AbilitySystemComponent source, float level = 1)
        {
            return new GameplayEffectSpec(effect, source, level);
        }

        public GameplayEffectSpec AddTarget(AbilitySystemComponent target)
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
                timeUntilPeriodTick = effect.period.magnitude.GetValue(level);

                if (effect.period.magnitude.GetValue(level) > 0)
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

        public void ApplyEffectTo(AbilitySystemComponent target)
        {
            if (target == null)
                return;

            var container = target.StatsContainer;

            if (effect.modifiers.Count > 0)
            {
                foreach (var mod in effect.modifiers)
                {
                    if (mod == null || mod.IsValid == false)
                        continue;

                    container.SetModifier(
                        mod.operatorType, 
                        mod.definition, 
                        effect.name, 
                        mod.GetValue(level));
                }
            }

            if (effect.executions.Count > 0)
            {
                foreach (var execution in effect.executions)
                {
                    var calculationClass = execution.calculationClass;
                    if (calculationClass != null)
                    {
                        calculationClass.Execute(source, target, out var outModifiers);
                        foreach (var mod in outModifiers)
                        {
                            if (mod == null || mod.IsValid == false)
                                continue;

                            container.SetModifier(
                                mod.operatorType, 
                                mod.definition, 
                                string.Format("execution_{0}_{1}", mod.operatorType.ToString(), mod.definition.DisplayName),
                                mod.GetValue(level));
                        }
                    }
                }
            }
        }
    }
}
