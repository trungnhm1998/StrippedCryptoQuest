using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Inventory.Currency;

namespace CryptoQuest.Battle
{
    public interface ILootMerger
    {
        /// <summary>
        /// Except equipments, merge all loots with same item data
        /// e.g. 2x <see cref="CurrencyInfo"/> with same <see cref="CurrencySO"/> will be merged into 1x <see cref="CurrencyInfo"/> with amount 2
        /// </summary>
        /// <param name="loots">Contains <see cref="CurrencyLootInfo"/>, <see cref="ExpLoot"/>, <see cref="ConsumableLootInfo"/>, <see cref="EquipmentLootInfo"/></param>
        /// <returns>Loots that merged and cloned to be add into inventory</returns>
        List<LootInfo> Merge(List<LootInfo> loots);

        bool Merge(ConsumableLootInfo otherLoot);
        bool Merge(CurrencyLootInfo loot);
        bool Merge(ExpLoot otherLoot);
        bool Merge(MagicStoneLoot otherLoot);
    }
}