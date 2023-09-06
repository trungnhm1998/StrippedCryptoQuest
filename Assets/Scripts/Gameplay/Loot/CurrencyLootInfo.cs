using System;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class CurrencyLootInfo : LootInfo<CurrencyInfo>
    {
        public override void AddItemToInventory(InventorySO inventory)
        {
            inventory.Add(Item);
        }

        public CurrencyLootInfo(CurrencyInfo item) : base(item) { }
    }
}