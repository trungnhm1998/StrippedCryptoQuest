using System.Collections.Generic;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Helper
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

        public static IEnumerable<CastableAbilitySpec> GetAbilitiesInBattle(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Battle);
        }

        public static IEnumerable<CastableAbilitySpec> GetAbilitiesInField(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Field);
        }

        public static IEnumerable<CastableAbilitySpec> GetAbilitiesInBattleAndField(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Battle | EAbilityUsageScenario.Field);
        }
        
        /// <summary>
        /// Get all granted ability filter with usage scenario
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="usageScenario"></param>
        /// <returns></returns>
        public static IEnumerable<CastableAbilitySpec> GetAbilitiesWithUsageScenario(
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