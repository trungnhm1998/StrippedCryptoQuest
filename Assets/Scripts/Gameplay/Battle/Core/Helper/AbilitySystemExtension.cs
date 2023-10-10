using System.Collections.Generic;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.Helper
{
    public static class AbilitySystemExtension
    {
        public static void ActivateAbilityWithTag(this AbilitySystemBehaviour owner, TagScriptableObject tag)
        {
            var abilities = owner.GrantedAbilities;
            foreach (var ability in abilities)
            {
                if (ability.AbilitySO.Tags.AbilityTag != tag) continue;
                owner.TryActiveAbility(ability);
            }
        }

        public static IEnumerable<CastSkillAbilitySpec> GetAbilitiesInBattle(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Battle);
        }

        public static IEnumerable<CastSkillAbilitySpec> GetAbilitiesInField(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Field);
        }

        public static IEnumerable<CastSkillAbilitySpec> GetAbilitiesInBattleAndField(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Battle | EAbilityUsageScenario.Field);
        }
        
        /// <summary>
        /// Get all granted ability filter with usage scenario
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="usageScenario"></param>
        /// <returns></returns>
        public static IEnumerable<CastSkillAbilitySpec> GetAbilitiesWithUsageScenario(
            this AbilitySystemBehaviour owner,
            EAbilityUsageScenario usageScenario)
        {
            // TODO: REFACTOR BATTLE
            // foreach (var grantedAbility in owner.GrantedAbilities)
            // {
            //     if (!(grantedAbility is SimpleGameplayAbilitySpec ability)) continue;
            //     var abilitySO = ability.AbilityDef;
            //     if (!abilitySO.Info.CheckUsageScenario(usageScenario)) continue;
            //     yield return ability;
            // }
            yield break;
        }
    }
}