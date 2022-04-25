using SlimeRPG.Framework.Tag;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public abstract class AbstractGameplayAbilitySpec
    {
        public abstract bool CanActivateAbility();
        public abstract bool CommitAbility();
        public abstract IEnumerator PreActivateAbility();
        public abstract IEnumerator ActivateAbility();
        public abstract IEnumerator CancelAbility();
        public abstract IEnumerator EndAbility();
        public abstract IEnumerator TryActivateAbility();
    }

    public class GameplayAbilitySpec<T> : AbstractGameplayAbilitySpec where T : GameplayAbility
    {
        protected WeakReference<AbilitySystemComponent> weakInstigator;
        protected WeakReference<AbilitySystemComponent> weakSource;
        protected List<WeakReference<AbilitySystemComponent>> weakTargets = new List<WeakReference<AbilitySystemComponent>>();

        public T ability;
        public float level = 1f;

        #region Internal Variables
        protected bool isActive = false;
        protected bool isBlockingOtherAbilities = false;
        protected bool isCancelable = true;
        #endregion

        public struct AbilityCooldownTime
        {
            public float timeRemaining;
            public float totalDuration;
        }


        protected AbilitySystemComponent Instigator
        {
            get
            {
                if (weakInstigator.TryGetTarget(out var instigator))
                    return instigator;

                return null;
            }
        }

        protected AbilitySystemComponent Source
        {
            get
            {
                if (weakSource.TryGetTarget(out var source))
                    return source;

                return null;
            }
        }

        protected List<AbilitySystemComponent> Targets
        {
            get
            {
                return weakTargets.Select(x =>
                {
                    if (x.TryGetTarget(out var target))
                        return target;
                    return null;
                }).Where(x => x != null).ToList();
            }
        }

        public GameplayAbilitySpec(T ability, AbilitySystemComponent instigator, AbilitySystemComponent source, float level)
        {
            this.ability = ability;
            weakInstigator = new WeakReference<AbilitySystemComponent>(instigator);
            weakSource = new WeakReference<AbilitySystemComponent>(source);
            this.level = level;
        }

        public override bool CanActivateAbility()
        {
            return CheckCooldown()
                && CheckCost()
                && DoesAbilitySatisfyTagRequirements();
        }

        public override bool CommitAbility()
        {
            // When committing, the situation may have changed, 
            // so check it again
            if (CheckCooldown() && CheckCost())
            {
                ApplyCooldown();
                ApplyCost();
                return true;
            }

            return false;
        }

        public override IEnumerator PreActivateAbility()
        {
            isActive = true;
            isBlockingOtherAbilities = true;
            isCancelable = false;

            // asc.handleChangeAbilityCanbeCanceled(abilityTags);
            // GameplayAbilityEndedSubscribe(endedDelegate);

            // asc.OnAbilityActivated(handle, this);
            // asc.applyAbilityBlockAdnCancelTags(AbilityTags)
            yield return null;
        }

        public override IEnumerator ActivateAbility()
        {
            yield return null;
        }

        public override IEnumerator CancelAbility()
        {
            // do something...
            yield return EndAbility();
        }

        public override IEnumerator EndAbility()
        {
            // remove gameplay tags ...
            yield return null;
        }

        public override IEnumerator TryActivateAbility()
        {
            // Flow of a simple GameplayAbility
            if (CanActivateAbility() == false)
                yield break;

            yield return PreActivateAbility();
            
            if (CommitAbility())
                yield return ActivateAbility();

            yield return EndAbility();
        }

        protected virtual bool CheckCooldown()
        {
            if (ability.coolDown == null)
                return true;

            GameplayTagContainer cooldownTags = ability.coolDown.tags.assetTag;

            if (cooldownTags.Num() > 0)
                if (Source.gameplayTagCountContainer.HasAnyMatchingGameplayTags(cooldownTags))
                    return false;

            if (RemainingCooldownTime().timeRemaining > 0)
                return false;

            return true;
        }

        protected virtual bool CheckCost()
        {
            if (ability.cost == null)
                return true;

            GameplayTagContainer costTags = ability.cost.tags.assetTag;

            if (costTags.Num() > 0)
                if (Source.gameplayTagCountContainer.HasAnyMatchingGameplayTags(costTags))
                    return false;

            return true;
        }

        public virtual void ApplyCost()
        {
            if (ability.cost == null)
                return;

            var spec = Source.MakeOutgointEffectSpec(ability.cost, level);
            Source.ApplyGameplayEffect(spec);
        }

        public virtual void ApplyCooldown()
        {
            if (ability.coolDown == null)
                return;

            var spec = Source.MakeOutgointEffectSpec(ability.coolDown, level);
            Source.ApplyGameplayEffect(spec);
        }

        public AbilityCooldownTime RemainingCooldownTime()
        {
            if (ability.coolDown == null)
                return new AbilityCooldownTime();

            float maxDuration = 0;
            float longestCooldown = 0f;

            var appliedSpecs = Source.AppliedSpecs;
            var cooldownTags = ability.coolDown.tags.grantedTags;

            for (var i = 0; i < appliedSpecs.Count; i++)
            {
                var grantedTags = appliedSpecs[i].effect.tags.grantedTags;
                if (grantedTags.HasAnyTags(cooldownTags))
                {
                    if (appliedSpecs[i].effect.duration.policy == Duration.infinite)
                        return new AbilityCooldownTime()
                        {
                            timeRemaining = float.MaxValue,
                            totalDuration = 0
                        };

                    var durationRemaining = appliedSpecs[i].durationRemaining;

                    if (durationRemaining > longestCooldown)
                    {
                        longestCooldown = durationRemaining;
                        maxDuration = appliedSpecs[i].totalDuration;
                    }
                }
            }

            return new AbilityCooldownTime()
            {
                timeRemaining = longestCooldown,
                totalDuration = maxDuration
            };
        }

        protected virtual bool DoesAbilitySatisfyTagRequirements()
        {
            // check block, missing ...
            return true;
        }

        public void AddTarget(AbilitySystemComponent target)
        {
            weakTargets.Add(new WeakReference<AbilitySystemComponent>(target));
        }
    }
}