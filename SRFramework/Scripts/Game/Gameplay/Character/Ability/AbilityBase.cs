using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeRPG.Framework.Ability;


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


        protected CharacterBase owner;
        protected List<CharacterBase> targets = new List<CharacterBase>();
        protected Animator animator;

        protected bool isSetup = false;
        protected bool isActive = false;


        public virtual void Setup(CharacterBase owner)
        {
            this.owner = owner;
            animator = owner.animationController.animator;
            isSetup = true;
            isActive = false;

            if (effect)
            {
                if (effect.gameplayEffect.duration.modifier != 0f)
                {
                    durationRemaining = effect.gameplayEffect.duration.modifier;
                    totalDuration = durationRemaining;
                }

                timeUntilPeriodTick = effect.period.value;
                if (effect.period.executeOnApplication)
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
                owner.attributeController.ApplyGameplayEffect(cost);
        }

        public virtual void ApplyCooldown()
        {
            if (coolDown)
                owner.attributeController.ApplyGameplayEffect(coolDown);
        }

        public virtual bool CheckCost()
        {
            if (cost == null) return true;

            var ge = cost.gameplayEffect;

            if (ge.duration.policy != EDurationPolicy.Instant)
                return true;

            if (ge.adjustment)
            {
                foreach (var modifier in ge.adjustment.adjustment)
                {
                    if (modifier.operatorType != SlimeRPG.Framework.StatsSystem.OperatorType.Add) 
                        continue;

                    var container = owner.attributeController.StatsContainer;
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
            if (coolDown == null) return new AbilityCooldownTime();
            var cooldownTags = coolDown.gameplayEffectTags.GrantedTags;

            float longestCooldown = 0f;

            // Check if the cooldown tag is granted to the player, and if so, capture the remaining duration for that tag
            //for (var i = 0; i < source.AppliedGameplayEffects.Count; i++)
            //{
            //    var grantedTags = source.AppliedGameplayEffects[i].spec.GameplayEffect.gameplayEffectTags.GrantedTags;
            //    for (var iTag = 0; iTag < grantedTags.Length; iTag++)
            //    {
            //        for (var iCooldownTag = 0; iCooldownTag < cooldownTags.Length; iCooldownTag++)
            //        {
            //            if (grantedTags[iTag] == cooldownTags[iCooldownTag])
            //            {
            //                // If this is an infinite GE, then return null to signify this is on CD
            //                if (source.AppliedGameplayEffects[i].spec.GameplayEffect.gameplayEffect.DurationPolicy == EDurationPolicy.Infinite) return new AbilityCooldownTime()
            //                {
            //                    timeRemaining = float.MaxValue,
            //                    totalDuration = 0
            //                };

            //                var durationRemaining = source.AppliedGameplayEffects[i].spec.DurationRemaining;

            //                if (durationRemaining > longestCooldown)
            //                {
            //                    longestCooldown = durationRemaining;
            //                    maxDuration = source.AppliedGameplayEffects[i].spec.TotalDuration;
            //                }
            //            }

            //        }
            //    }
            //}

            return new AbilityCooldownTime()
            {
                timeRemaining = longestCooldown,
                totalDuration = maxDuration
            };
        }

        public void AddTarget(CharacterBase target)
        {
            targets.Add(target);
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
                timeUntilPeriodTick = effect.period.value;

                if (effect.period.value > 0)
                    executePeriodicTick = true;
            }
        }
    }
}