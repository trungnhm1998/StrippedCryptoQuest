using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        InventorySO Inventory { get; }
        bool Add(IEquipment equipment);
        bool Remove(IEquipment equipment);
        bool Add(ConsumableInfo consumable, int quantity = 1);
        bool Remove(ConsumableInfo consumable, int quantity = 1);
        bool Add(CurrencyInfo currency);
        bool Remove(CurrencyInfo currency);
    }
}