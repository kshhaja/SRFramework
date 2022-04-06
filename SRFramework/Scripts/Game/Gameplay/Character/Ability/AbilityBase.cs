﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.Ability;
using System;
using System.Linq;

namespace SlimeRPG.Gameplay.Character.Ability
{
    public abstract class AbilityBase : AbstractAbilityScriptableObject
    {
        public int level = 1;
        public int quality = 0;
        public Sprite icon;
        public AnimationClip clip;


        public struct AbilityCooldownTime
        {
            public float timeRemaining;
            public float totalDuration;
        }

        public float durationRemaining { get; private set; }
        public float totalDuration { get; private set; }
        public GameplayEffectPeriod periodDefinition { get; private set; }
        public float timeUntilPeriodTick { get; private set; }


        protected WeakReference<CharacterBase> weakInstigator;
        protected WeakReference<CharacterBase> weakSource;
        protected List<WeakReference<CharacterBase>> weakTargets = new List<WeakReference<CharacterBase>>();
        protected Animator animator;

        protected bool isSetup = false;
        protected bool isActive = false;

        protected CharacterBase Instigator 
        {
            get
            {
                if (weakInstigator.TryGetTarget(out var instigator))
                    return instigator;

                return null;
            }
        }

        protected CharacterBase Source
        {
            get
            {
                if (weakSource.TryGetTarget(out var source))
                    return source;

                return null;
            }
        }

        protected List<CharacterBase> Targets
        {
            get
            {
                // 아직 살아있는 target만 가져온다.
                return weakTargets.Select(x =>
                {
                    if (x.TryGetTarget(out var target))
                        return target;
                    return null;
                }).Where(x => x != null).ToList();
            }
        }


        public virtual void Setup(CharacterBase instigator)
        {
            weakInstigator = new WeakReference<CharacterBase>(instigator);
            animator = instigator.animationController.animator;
            isSetup = true;
            isActive = false;

            if (effect)
            {
                if (effect.duration.modifier != 0f)
                {
                    durationRemaining = effect.duration.modifier;
                    totalDuration = durationRemaining;
                }

                timeUntilPeriodTick = effect.duration.period.interval;
                if (effect.duration.period.executeOnApplication)
                    timeUntilPeriodTick = 0;
            }
        }

        public virtual IEnumerator TryCastAbility()
        {
                // Todo: create montage event.

            if (!CanActivateAbility())
                yield break;

            isActive = true;
            yield return PreCast();
            yield return CastAbility();
            // yield return PostCast();
            EndAbility();
        }

        protected virtual IEnumerator PreCast()
        {
            yield return null;
        }

        protected virtual IEnumerator CastAbility()
        {
            ApplyCost();
            ApplyCooldown();
            yield return null;
        }

        public abstract void CancelAbility();

        public virtual void EndAbility()
        {
            isActive = false;
        }

        public virtual bool CanActivateAbility()
        {
            return isSetup && !isActive
                && CheckGameplayTags()
                && CheckCost()
                && CheckCooldown().timeRemaining <= 0;
        }

        public abstract bool CheckGameplayTags();

        public virtual void ApplyCost()
        {
            if (cost)
                Instigator?.attributeController.ApplyGameplayEffect(cost);
        }

        public virtual void ApplyCooldown()
        {
            if (coolDown)
                Instigator?.attributeController.ApplyGameplayEffect(coolDown);
        }

        public virtual bool CheckCost()
        {
            if (cost == null) 
                return true;

            if (cost.duration.policy != Duration.instant)
                return true;

            if (cost.modifiers)
            {
                foreach (var modifier in cost.modifiers.adjustment)
                {
                    if (modifier.operatorType != Framework.StatsSystem.OperatorType.Add) 
                        continue;

                    var container = Instigator?.attributeController.StatsContainer;
                    if (container.HasRecord(modifier.definition))
                    {
                        var costValue = modifier.GetValue(0);
                        var attributeValue = container.GetStat(modifier.definition);
                        if (attributeValue + costValue < 0)
                            return false;
                    }
                }
            }
            return true;
        }

        public virtual AbilityCooldownTime CheckCooldown()
        {
            float maxDuration = 0;
            if (coolDown == null) 
                return new AbilityCooldownTime();
            
            var cooldownTags = coolDown.gameplayEffectTags.GrantedTags;

            float longestCooldown = 0f;

            var appliedFx = Instigator?.abilityController.appliedGameplayEffects;
            for (var i = 0; i < appliedFx.Count; i++)
            {
                var grantedTags = appliedFx[i].ability.effect.gameplayEffectTags.GrantedTags;
                if (grantedTags.HasAnyTags(cooldownTags))
                {
                    if (appliedFx[i].ability.effect.duration.policy == Duration.infinite)
                        return new AbilityCooldownTime()
                        {
                            timeRemaining = float.MaxValue,
                            totalDuration = 0
                        };


                    var durationRemaining = appliedFx[i].ability.durationRemaining;

                    if (durationRemaining > longestCooldown)
                    {
                        longestCooldown = durationRemaining;
                        maxDuration = appliedFx[i].ability.totalDuration;
                    }
                }
            }

            return new AbilityCooldownTime()
            {
                timeRemaining = longestCooldown,
                totalDuration = maxDuration
            };
        }

        public void AddTarget(CharacterBase target)
        {
            weakTargets.Add(new WeakReference<CharacterBase>(target));
        }

        public virtual bool Update()
        {
            return true;
        }

        public virtual bool FixedUpdate()
        {
            return true;
        }

        public virtual void DrawGizmos()
        {
        }

        public virtual void OnAnimatorIK(int layerIndex)
        {
        }

        public virtual void UpdateRemainingDuration(float deltaTime)
        {
            durationRemaining -= deltaTime;
        }

        public virtual void TickPeriodic(float deltaTime, out bool executePeriodicTick)
        {
            timeUntilPeriodTick -= deltaTime;
            executePeriodicTick = false;

            if (timeUntilPeriodTick <= 0)
            {
                timeUntilPeriodTick = effect.duration.period.interval;

                if (effect.duration.period.interval > 0)
                    executePeriodicTick = true;
            }
        }
    }
}