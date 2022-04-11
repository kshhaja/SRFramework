using SlimeRPG.Framework.Tag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Framework.Ability
{
    public class GameplayAbilitySpec
    {
        public AbilitySystemComponent instigator;
        public AbilitySystemComponent source;

        public GameplayAbility ability;

        #region Internal Variables
        protected bool isActive = false;
        protected bool isBlockingOtherAbilities = false;
        protected bool isCancelable = true;
        #endregion


        public GameplayAbilitySpec(GameplayAbility ability, AbilitySystemComponent instigator, AbilitySystemComponent source, float level)
        {
            this.ability = ability;
            this.instigator = instigator;
            this.source = source;
        }

        public static GameplayAbilitySpec CreateNew(GameplayAbility ability, AbilitySystemComponent instigator, AbilitySystemComponent source, float level = 1)
        {
            return new GameplayAbilitySpec(ability, instigator, source, level);
        }

        public virtual bool CanActivateAbility()
        {
            return CheckCooldown()
                && CheckCost()
                && DoesAbilitySatisfyTagRequirements();
        }
        public virtual bool CommitAbility()
        {
            // applyCooldown;
            // applyCost;

            // return false when failed to apply attributes

            return true;
        }

        public virtual IEnumerator PreActivateAbility()
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

        public virtual IEnumerator ActivateAbility()
        {
            if (CommitAbility() == false)
                yield break;

            yield return null;
        }

        public virtual IEnumerator CancelAbility()
        {
            // do something...
            yield return EndAbility();
        }

        public virtual IEnumerator EndAbility()
        {
            // remove gameplay tags ...
            yield return null;
        }

        public virtual IEnumerator TryActivateAbility()
        {
            // Flow of a simple GameplayAbility
            if (CanActivateAbility() == false)
                yield break;

            yield return PreActivateAbility();
            yield return ActivateAbility();
            yield return EndAbility();
        }

        protected virtual bool CheckCooldown()
        {
            if (ability.coolDown == null)
                return true;

            GameplayTagContainer cooldownTags = ability.coolDown.tags.assetTag;

            if (cooldownTags.Num() > 0)
                if (source.gameplayTagCountContainer.HasAnyMatchingGameplayTags(cooldownTags))
                    return false;

            return true;
        }

        protected virtual bool CheckCost()
        {
            // check can apply modifiers
            if (ability.cost == null)
                return true;

            return true;
        }

        protected virtual bool DoesAbilitySatisfyTagRequirements()
        {
            // check block, missing ...
            return true;
        }
    }
}