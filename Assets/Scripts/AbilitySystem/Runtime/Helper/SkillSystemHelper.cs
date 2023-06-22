using System.Collections.Generic;

namespace Indigames.AbilitySystem
{
    public static class SkillSystemHelper
    {
        public static List<AbstractEffect> ApplyEffectSpecToTarget(AbstractEffect effect, SkillSystem target)
        {
            var appliedEffects = new List<AbstractEffect>();
            var effectSpec = target.ApplyEffectToSelf(effect);
            if (effectSpec.Owner == null || effectSpec.Target == null)
                return appliedEffects;
            appliedEffects.Add(effectSpec);

            return appliedEffects;
        }

        /// <summary>
        /// Checks if an Skill System has all the listed tags
        /// </summary>
        /// <param name="skillSystem">Skill System</param>
        /// <param name="tags">List of tags to check</param>
        /// <returns><para>true, if the Skill System has all tags</para>
        /// false, if system is invalid or missing 1 or more tags</returns>
        public static bool SystemHasAllTags(SkillSystem skillSystem, TagScriptableObject[] tags)
        {
            if (skillSystem == null) return false;

            foreach (var tag in tags)
            {
                if (!skillSystem.GrantedTags.Contains(tag)) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if an Skill System has none of the listed tags
        /// I could use static method here but I want to pass the skill system to the method
        /// </summary>
        /// <param name="skillSystem">Skill System</param>
        /// <param name="tags">List of tags to check</param>
        /// <returns><para>true, if the Skill System has none of the tags</para>
        /// false, if system is invalid or has 1 or more tags</returns>
        public static bool SystemHasNoneTags(SkillSystem skillSystem, TagScriptableObject[] tags)
        {
            if (skillSystem == null) return false;

            // got through all tags
            foreach (var tag in tags)
            {
                if (skillSystem.GrantedTags.Contains(tag)) return false;
            }

            return true;
        }
    }
}
