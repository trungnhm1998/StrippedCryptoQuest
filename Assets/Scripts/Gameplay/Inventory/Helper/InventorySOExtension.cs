using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Helper
{
    public static class InventorySOExtension
    {
        public static IEnumerable<ConsumableInfo> GetItemsInBattle(this InventorySO inventory)
        {
            return GetItemsWithUsageScenario(inventory, EAbilityUsageScenario.Battle);
        }

        public static IEnumerable<ConsumableInfo> GetItemsInField(this InventorySO inventory)
        {
            return GetItemsWithUsageScenario(inventory, EAbilityUsageScenario.Field);
        }

        public static IEnumerable<ConsumableInfo> GetItemsInBattleAndField(this InventorySO inventory)
        {
            return GetItemsWithUsageScenario(inventory, EAbilityUsageScenario.Battle | EAbilityUsageScenario.Field);
        }

        /// <summary>
        /// Get all item with ability filter with usage scenario
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="usageScenario"></param>
        /// <returns></returns>
        public static IEnumerable<ConsumableInfo> GetItemsWithUsageScenario(this InventorySO inventory,
            EAbilityUsageScenario usageScenario)
        {
            foreach (var itemInfo in inventory.Consumables)
            {
                if ((itemInfo.Data.UsageScenario & usageScenario) <= 0) continue;
                yield return itemInfo;
            }
        }
    }
}