using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        InventorySO Inventory { get; }
        bool Add(EquipmentInfo equipment);
        bool Remove(EquipmentInfo equipment);
        bool Add(NftEquipment equipment);
        bool Remove(NftEquipment equipment);
        bool Add(ConsumableInfo consumable);
        bool Remove(ConsumableInfo consumable);
        bool Contains(EquipmentInfo equipment);
        bool Contains(NftEquipment equipment);
    }
}