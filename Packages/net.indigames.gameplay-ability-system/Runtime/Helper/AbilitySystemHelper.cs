using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace IndiGames.GameplayAbilitySystem.Helper
{
    public static class AbilitySystemHelper
    {
        public static List<AbstractEffect> ApplyEffectSpecToTarget(AbstractEffect effect, AbilitySystemBehaviour target)
        {
            var appliedEffects = new List<AbstractEffect>();
            var effectSpec = target.EffectSystem.ApplyEffectToSelf(effect);
            if (effectSpec.Owner == null || effectSpec.Target == null)
                return appliedEffects;
            appliedEffects.Add(effectSpec);

            return appliedEffects;
        }

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
    }
}
