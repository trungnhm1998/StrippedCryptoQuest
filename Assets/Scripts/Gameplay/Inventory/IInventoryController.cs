using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Item.Equipment;

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
        bool Contains(Equipment equipment);
        bool Contains(NftEquipment equipment);
        bool Add(CurrencyInfo currency);
        bool Remove(CurrencyInfo currency);
    }
}