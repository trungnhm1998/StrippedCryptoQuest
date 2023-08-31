using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

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

        public static IEnumerable<SimpleGameplayAbilitySpec> GetAbilitiesInBattle(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Battle);
        }

        public static IEnumerable<SimpleGameplayAbilitySpec> GetAbilitiesInField(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Field);
        }

        public static IEnumerable<SimpleGameplayAbilitySpec> GetAbilitiesInBattleAndField(this AbilitySystemBehaviour owner)
        {
            return GetAbilitiesWithUsageScenario(owner, EAbilityUsageScenario.Battle | EAbilityUsageScenario.Field);
        }
        
        /// <summary>
        /// Get all granted ability filter with usage scenario
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="usageScenario"></param>
        /// <returns></returns>
        public static IEnumerable<SimpleGameplayAbilitySpec> GetAbilitiesWithUsageScenario(
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