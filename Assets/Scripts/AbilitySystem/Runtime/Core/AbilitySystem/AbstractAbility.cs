using System.Collections;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class AbstractAbility
    {
        protected bool _isActive;
        public bool IsActive => _isActive;

        protected AbilityScriptableObject _abilitySO;
        public AbilityScriptableObject AbilitySO => _abilitySO;

        protected AbilityParameters _parameters; 

        public AbilitySystemBehaviour Owner;
        public bool IsRemoveAfterActivation;
        public bool IsPendingRemove;

        /// <summary>
        /// Initiation method of ability
        /// </summary>
        /// <param name="owner">Owner of this ability</param>
        /// <param name="abilitySO">Ability's data SO</param>
        /// <param name="parameters">Parameters of the ability</param>
        public virtual void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO, AbilityParameters parameters)
        {
            Owner = owner;
            _abilitySO = abilitySO;
            _parameters = parameters;
        }

        public virtual void TryActiveAbility()
        {
            if (Owner == null)
            {
                Debug.LogWarning($"Try to active a Ability [{AbilitySO.name}] with invalid owner");
                return;
            }

            Owner.TryActiveAbility(this);
        }

        /// <summary>
        /// Not the same as granted a ability, ability might be granted but not activate yet
        /// </summary>
        public virtual void ActivateAbility()
        {
            if (!CanActiveAbility()) return;

            _isActive = true;
            Owner.StartCoroutine(InternalActiveAbility());
            Owner.TagSystem.GrantedTags.AddRange(AbilitySO.Tags.ActivationTags);
        }

        /// <summary>
        /// Not the same as ability being removed, ability ended but still in the system
        /// </summary>
        public virtual void EndAbility()
        {
            if (!_isActive || Owner == null) return;

            _isActive = false;
            Owner.StopCoroutine(InternalActiveAbility());
            Owner.TagSystem.RemoveTags(AbilitySO.Tags.ActivationTags);
        }

        public virtual bool CanActiveAbility()
        {
            return CheckTags();
        }

        /// <summary>
        /// This ability can only active if the Owner system has all the required tags
        /// and none of the Ignore tags
        /// Source and Target are not implemented yet
        /// </summary>
        protected virtual bool CheckTags()
        {
            return AbilitySystemHelper.SystemHasAllTags(Owner, AbilitySO.Tags.OwnerTags.RequireTags)
                // && AbilitySystemHelper.SystemHasAllTags(Source, AbilitySO.Tags.SourceTags.RequireTags)
                // && AbilitySystemHelper.SystemHasAllTags(Target, AbilitySO.Tags.TargetTags.RequireTags)
                // ---------------------------------------------------------
                // && AbilitySystemHelper.SystemHasNoneTags(Source, AbilitySO.Tags.SourceTags.IgnoreTags)
                // && AbilitySystemHelper.SystemHasNoneTags(Target, AbilitySO.Tags.TargetTags.IgnoreTags)
                && AbilitySystemHelper.SystemHasNoneTags(Owner, AbilitySO.Tags.OwnerTags.IgnoreTags);
        }

        public virtual void OnAbilityRemoved(AbstractAbility abilitySpec)
        {
            EndAbility();
        }

        public virtual void OnAbilityGranted(AbstractAbility abilitySpec)
        {
        }

        /// <summary>
        /// Using IEnumerator so the ability can produce step by step like having delay time, etc...
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator InternalActiveAbility();
    }
}