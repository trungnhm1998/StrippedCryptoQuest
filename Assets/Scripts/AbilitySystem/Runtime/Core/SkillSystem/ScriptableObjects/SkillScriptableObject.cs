using System;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class SkillScriptableObject : ScriptableObject
    {
        [SerializeField] private SkillTags tags;
        public SkillTags Tags => tags;

        [SerializeField] private AbilityParameters parameters;
        public AbilityParameters Parameters => parameters;

        public AbstractSkill GetSkillSpec(AbilitySystem owner, AbilityParameters parameters)
        {
            var skill = CreateSkill();
            skill.InitSkill(owner, this, parameters);
            return skill;
        }

        protected abstract AbstractSkill CreateSkill();
    }

    /// <summary>
    /// Override this to create new skill SO with a new abstract skill
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SkillScriptableObject<T> : SkillScriptableObject where T : AbstractSkill, new()
    {
        protected override AbstractSkill CreateSkill() => new T();
    }

    [Serializable]
    public struct SkillTags
    {
        /// <summary>
        /// Tag to define this skill should be unique
        /// </summary>
        public TagScriptableObject SkillTag;

        /// <summary>
        /// Active the skill on the same system will cancel any skill that have these tags
        /// </summary>
        public TagScriptableObject[] CancelSkillWithTags;

        /// <summary>
        /// Prevents execution of any other Skills with a matching Tag while this Skill is executing.
        /// Skill that have these tags will be blocked from activating on the same system
        /// e.g. silencing skill that enemy could use to prevent use to use any skill
        /// </summary>
        public TagScriptableObject[] BlockSkillWithTags;

        /// <summary>
        /// These tags will be granted to the source system while this skill is active
        /// </summary>
        public TagScriptableObject[] ActivationTags;

        /// <summary>
        /// This skill can only active if owner system has all of the RequiredTags
        /// and none of the Ignore tags
        /// </summary>
        public TagRequireIgnoreDetails OwnerTags;

        /// <summary>
        /// This skill can only active if the Source system has all the required tags
        /// and none of the Ignore tags
        /// </summary>
        public TagRequireIgnoreDetails SourceTags;

        /// <summary>
        /// This skill can only active if the Target system has all the required tags
        /// and none of the Ignore tags
        /// </summary>
        public TagRequireIgnoreDetails TargetTags;
    }
}