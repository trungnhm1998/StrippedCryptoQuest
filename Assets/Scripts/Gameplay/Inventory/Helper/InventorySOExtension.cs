using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Helper
{
    public static class InventorySOExtension
    {
        public static IEnumerable<UsableInfo> GetItemsInBattle(this InventorySO inventory)
        {
            return GetItemsWithUsageScenario(inventory, EAbilityUsageScenario.Battle);
        }

        public static IEnumerable<UsableInfo> GetItemsInField(this InventorySO inventory)
        {
            return GetItemsWithUsageScenario(inventory, EAbilityUsageScenario.Field);
        }

        public static IEnumerable<UsableInfo> GetItemsInBattleAndField(this InventorySO inventory)
        {
            return GetItemsWithUsageScenario(inventory, EAbilityUsageScenario.Battle | EAbilityUsageScenario.Field);
        }
        
        /// <summary>
        /// Get all item with ability filter with usage scenario
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="usageScenario"></param>
        /// <returns></returns>
        public static IEnumerable<UsableInfo> GetItemsWithUsageScenario(this InventorySO inventory, EAbilityUsageScenario usageScenario)
        {
            foreach (var itemInfo in inventory.UsableItems)
            {
                var abilitySO = itemInfo.Data.Ability;
                if (!abilitySO.Info.CheckUsageScenario(usageScenario)) continue;
                yield return itemInfo;
            }
        }
    }
}