using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        InventorySO Inventory { get; }
        bool Add(Equipment equipment);
        bool Remove(Equipment equipment);
        bool Add(NftEquipment equipment);
        bool Remove(NftEquipment equipment);
        bool Add(ConsumableInfo consumable, int quantity = 1);
        bool Remove(ConsumableInfo consumable, int quantity = 1);
        bool Add(MagicStoneInfo magicStone);
        bool Remove(MagicStoneInfo magicStone);
        bool Contains(Equipment equipment);
        bool Contains(NftEquipment equipment);
        bool Add(CurrencyInfo currency);
        bool Remove(CurrencyInfo currency);
    }
}