using System.Collections;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class AbstractSkill
    {
        protected bool _isActive;
        public bool IsActive => _isActive;

        protected SkillScriptableObject _skillSO;
        public SkillScriptableObject SkillSO => _skillSO;

        protected SkillParameters _parameters; 

        public SkillSystem Owner;
        public bool IsRemoveAfterActivation;
        public bool IsPendingRemove;

        /// <summary>
        /// Initiation method of skill
        /// </summary>
        /// <param name="owner">Owner of this skill</param>
        /// <param name="skillSO">Skill's data SO</param>
        /// <param name="parameters">Parameters of the skill</param>
        public virtual void InitSkill(SkillSystem owner, SkillScriptableObject skillSO, SkillParameters parameters)
        {
            Owner = owner;
            _skillSO = skillSO;
            _parameters = parameters;
        }

        public virtual void TryActiveSkill()
        {
            if (Owner == null)
            {
                Debug.LogWarning($"Try to active a Skill [{SkillSO.name}] with invalid owner");
                return;
            }

            Owner.TryActiveSkill(this);
        }

        /// <summary>
        /// Not the same as granted a skill, skill might be granted but not activate yet
        /// </summary>
        public virtual void ActivateSkill()
        {
            if (!CanActiveSkill()) return;

            _isActive = true;
            Owner.StartCoroutine(InternalActiveSkill());
            Owner.AddTags(SkillSO.Tags.ActivationTags);
        }

        /// <summary>
        /// Not the same as skill being removed, skill ended but still in the system
        /// </summary>
        public virtual void EndSkill()
        {
            if (!_isActive) return;

            _isActive = false;
            Owner.StopCoroutine(InternalActiveSkill());
            Owner.RemoveTags(SkillSO.Tags.ActivationTags);
        }

        public bool CanActiveSkill()
        {
            return !_isActive && CheckTags();
        }

        /// <summary>
        /// This skill can only active if the Owner, Source, Target system has all the required tags
        /// and none of the Ignore tags
        /// </summary>
        protected virtual bool CheckTags()
        {
            return SkillSystemHelper.SystemHasAllTags(Owner, SkillSO.Tags.OwnerTags.RequireTags)
                && SkillSystemHelper.SystemHasAllTags(Owner, SkillSO.Tags.SourceTags.RequireTags)
                && SkillSystemHelper.SystemHasAllTags(Owner, SkillSO.Tags.TargetTags.RequireTags)
                // ---------------------------------------------------------
                && SkillSystemHelper.SystemHasNoneTags(Owner, SkillSO.Tags.OwnerTags.IgnoreTags)
                && SkillSystemHelper.SystemHasNoneTags(Owner, SkillSO.Tags.SourceTags.IgnoreTags)
                && SkillSystemHelper.SystemHasNoneTags(Owner, SkillSO.Tags.TargetTags.IgnoreTags);
        }

        public virtual void OnSkillRemoved(AbstractSkill skillSpec)
        {
            EndSkill();
        }

        public virtual void OnSkillGranted(AbstractSkill skillSpec)
        {
        }

        /// <summary>
        /// Using IEnumerator so the skill can produce step by step like having delay time, etc...
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator InternalActiveSkill();
    }
}