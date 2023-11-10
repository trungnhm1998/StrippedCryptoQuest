using System.Collections.Generic;
using System.Linq;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace IndiGames.GameplayAbilitySystem.Helper
{
    public static class AbilitySystemHelper
    {
        /// <summary>
        /// Checks if an Ability System has all the listed tags
        /// </summary>
        /// <param name="abilitySystem">Ability System</param>
        /// <param name="tags">List of tags to check</param>
        /// <returns><para>true, if the Ability System has all tags</para>
        /// false, if system is invalid or missing 1 or more tags</returns>
        public static bool SystemHasAllTags(AbilitySystemBehaviour abilitySystem, TagScriptableObject[] tags)
        {
            if (abilitySystem == null) return false;
            var tagSystem = abilitySystem.TagSystem;

            foreach (var tag in tags)
            {
                if (!tagSystem.GrantedTags.Contains(tag)) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if an Ability System has none of the listed tags
        /// I could use static method here but I want to pass the ability system to the method
        /// </summary>
        /// <param name="abilitySystem">Ability System</param>
        /// <param name="tags">List of tags to check</param>
        /// <returns><para>true, if the Ability System has none of the tags</para>
        /// false, if system is invalid or has 1 or more tags</returns>
        public static bool SystemHasNoneTags(AbilitySystemBehaviour abilitySystem, TagScriptableObject[] tags)
        {
            if (abilitySystem == null) return false;
            var tagSystem = abilitySystem.TagSystem;

            // got through all tags
            foreach (var tag in tags)
            {
                if (tagSystem.GrantedTags.Contains(tag)) return false;
            }

            return true;
        }

        
        /// <summary>
        /// Also check if there's child tag in system
        /// </summary>
        /// <param name="tagSystem"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static bool CheckSystemHasTags(this IList<TagScriptableObject> tags, TagScriptableObject tag)
        {
            return tags.Where(t => (t == tag) || (t.IsChildOf(tag))).Count() > 0;
        }
    }
}
