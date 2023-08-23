using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Helper;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AbilitySystem
{
    public class GameplayAbilitySpec
    {
        protected bool _isActive;
        public bool IsActive => _isActive;

        protected AbilityScriptableObject _abilitySO;
        public AbilityScriptableObject AbilitySO => _abilitySO;

        private AbilitySystemBehaviour _owner;

        public AbilitySystemBehaviour Owner
        {
            get => _owner;
            set => _owner = value;
        }

        /// <summary>
        /// Initiation method of ability
        /// </summary>
        /// <param name="owner">Owner of this ability</param>
        /// <param name="abilitySO">Ability's data SO</param>
        public virtual void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            _owner = owner;
            _abilitySO = abilitySO;
        }

        public virtual void TryActiveAbility()
        {
            if (_owner == null)
            {
                Debug.LogWarning($"Try to active a Ability [{AbilitySO.name}] with invalid owner");
                return;
            }

            _owner.TryActiveAbility(this);
        }

        /// <summary>
        /// Not the same as granted a ability, ability might be granted but not activate yet
        /// </summary>
        public virtual void ActivateAbility()
        {
            if (!CanActiveAbility()) return;

            _isActive = true;
            _owner.StartCoroutine(InternalActiveAbility());
            _owner.TagSystem.AddTags(AbilitySO.Tags.ActivationTags);
        }

        /// <summary>
        /// Not the same as ability being removed, ability ended but still in the system
        /// </summary>
        public virtual void EndAbility()
        {
            if (!_isActive || _owner == null) return;

            _isActive = false;
            _owner.StopCoroutine(InternalActiveAbility());
            _owner.TagSystem.RemoveTags(AbilitySO.Tags.ActivationTags);
        }

        /// <summary>
        /// Need the owner to active so we could use the coroutine
        /// </summary>
        /// <returns></returns>
        public virtual bool CanActiveAbility()
        {
            return _owner.gameObject.activeSelf && CheckTags();
        }

        /// <summary>
        /// This ability can only active if the Owner system has all the required tags
        /// and none of the Ignore tags
        /// Source and Target are not implemented yet
        /// </summary>
        protected virtual bool CheckTags()
        {
            return AbilitySystemHelper.SystemHasAllTags(_owner, AbilitySO.Tags.OwnerTags.RequireTags)
                   // && AbilitySystemHelper.SystemHasAllTags(Source, AbilitySO.Tags.SourceTags.RequireTags)
                   // && AbilitySystemHelper.SystemHasAllTags(Target, AbilitySO.Tags.TargetTags.RequireTags)
                   // ---------------------------------------------------------
                   // && AbilitySystemHelper.SystemHasNoneTags(Source, AbilitySO.Tags.SourceTags.IgnoreTags)
                   // && AbilitySystemHelper.SystemHasNoneTags(Target, AbilitySO.Tags.TargetTags.IgnoreTags)
                   && AbilitySystemHelper.SystemHasNoneTags(_owner, AbilitySO.Tags.OwnerTags.IgnoreTags);
        }

        public virtual void OnAbilityRemoved(GameplayAbilitySpec gameplayAbilitySpec)
        {
            EndAbility();
        }

        public virtual void OnAbilityGranted(GameplayAbilitySpec gameplayAbilitySpec) { }

        /// <summary>
        /// Will be called by <see cref="ActivateAbility"/>
        /// Using IEnumerator so the ability can produce step by step like having delay time, etc...
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator InternalActiveAbility()
        {
            yield break;
        }
    }
}