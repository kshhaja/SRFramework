using System.Collections;
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

        protected WeakReference<CharacterBase> weakInstigator;
        protected WeakReference<AbilitySystemCharacter> weakSource;
        protected List<WeakReference<AbilitySystemCharacter>> weakTargets = new List<WeakReference<AbilitySystemCharacter>>();
        protected Animator animator;

        protected bool isSetup = false;

        protected CharacterBase Instigator 
        {
            get
            {
                if (weakInstigator.TryGetTarget(out var instigator))
                    return instigator;

                return null;
            }
        }

        protected AbilitySystemCharacter Source
        {
            get
            {
                if (weakSource.TryGetTarget(out var source))
                    return source;

                return null;
            }
        }

        protected List<AbilitySystemCharacter> Targets
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
            weakSource = new WeakReference<AbilitySystemCharacter>(instigator.GetComponent<AbilitySystemCharacter>());
            animator = instigator.animationController.animator;
            isSetup = true;
        }

        public virtual IEnumerator TryActivateAbility()
        {
            // Todo: create montage event.

            if (!CanActivateAbility())
                yield break;

            yield return PreActivateAbility();
            yield return ActivateAbility();
            EndAbility();
        }

        protected virtual IEnumerator PreActivateAbility()
        {
            yield return null;
        }

        protected virtual IEnumerator ActivateAbility()
        {
            ApplyCost();
            ApplyCooldown();
            yield return null;
        }

        public abstract void CancelAbility();

        public virtual void EndAbility()
        {
        }

        public virtual bool CanActivateAbility()
        {
            return isSetup
                && CheckGameplayTags()
                && CheckCost()
                && CheckCooldown().timeRemaining <= 0;
        }

        public abstract bool CheckGameplayTags();

        public virtual void ApplyCost()
        {
            if (cost == null)
                return;
            
            var spec = Source.MakeOutgoingSpec(cost, 1);
            Source.ApplyGameplayEffect(spec);
        }

        public virtual void ApplyCooldown()
        {
            if (coolDown == null)
                return;

            var spec = Source.MakeOutgoingSpec(coolDown, 1);
            Source.ApplyGameplayEffect(spec);
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
            if (coolDown == null) 
                return new AbilityCooldownTime();
            
            float maxDuration = 0;
            float longestCooldown = 0f;

            var appliedSpecs = Source.AppliedSpecs;
            var cooldownTags = coolDown.gameplayEffectTags.GrantedTags;

            for (var i = 0; i < appliedSpecs.Count; i++)
            {
                var grantedTags = appliedSpecs[i].effect.gameplayEffectTags.GrantedTags;
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

        public void AddTarget(CharacterBase target)
        {
            weakTargets.Add(new WeakReference<AbilitySystemCharacter>(target.GetComponent<AbilitySystemCharacter>()));
        }

        // functions below are for locomotion or something variables...
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
    }
}
