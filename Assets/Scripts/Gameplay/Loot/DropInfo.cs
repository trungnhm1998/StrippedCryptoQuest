using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Gameplay.Loot
{
    public class DropInfo : LootInfo<ItemInfo>
    {
        public float Chance;
        public DropInfo(ItemInfo item) : base(item) { }

        public override void AddItemToInventory(InventorySO inventory) { }
    }
}