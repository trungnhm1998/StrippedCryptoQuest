using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class UsableLootInfo : LootInfo<UsableInfo>
    {
        public override void AddItemToInventory(InventorySO inventory)
        {
            inventory.Add(Item);
        }

        public UsableLootInfo(UsableInfo item) : base(item) { }
    }
}