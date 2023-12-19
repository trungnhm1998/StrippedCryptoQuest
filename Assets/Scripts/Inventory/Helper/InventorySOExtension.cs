using System.Collections.Generic;
using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Item.Consumable;

namespace CryptoQuest.Inventory.Helper
{
    public static class InventorySOExtension
    {
        public static IEnumerable<ConsumableInfo> GetItemsInBattle(this ConsumableInventory equipmentInventory)
        {
            return GetItemsWithUsageScenario(equipmentInventory, EAbilityUsageScenario.Battle);
        }

        public static IEnumerable<ConsumableInfo> GetItemsInField(this ConsumableInventory equipmentInventory)
        {
            return GetItemsWithUsageScenario(equipmentInventory, EAbilityUsageScenario.Field);
        }

        public static IEnumerable<ConsumableInfo> GetItemsInBattleAndField(this ConsumableInventory equipmentInventory)
        {
            return GetItemsWithUsageScenario(equipmentInventory, EAbilityUsageScenario.Battle | EAbilityUsageScenario.Field);
        }

        /// <summary>
        /// Get all item with ability filter with usage scenario
        /// </summary>
        /// <param name="equipmentInventory"></param>
        /// <param name="usageScenario"></param>
        /// <returns></returns>
        public static IEnumerable<ConsumableInfo> GetItemsWithUsageScenario(this ConsumableInventory equipmentInventory,
            EAbilityUsageScenario usageScenario)
        {
            foreach (var itemInfo in equipmentInventory.Items)
            {
                if ((itemInfo.Data.UsageScenario & usageScenario) <= 0) continue;
                yield return itemInfo;
            }
        }
    }
}