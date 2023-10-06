using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Shop.UI.Item;
using System.Collections.Generic;

public static class InventoryExtensions
{

    public static List<EquipmentInfo> GetWeapons(this InventorySO inventory)
    {
        var weaponList = new List<EquipmentInfo>();

        foreach (var item in inventory.Equipments)
        {
            if (item.Data.EquipmentCategory == EEquipmentCategory.Weapon
             && !item.IsNftItem && !item.IsEquipped)
            {
                weaponList.Add(item);
            }
        }

        return weaponList;
    }

    public static List<EquipmentInfo> GetNonWeapons(this InventorySO inventory)
    {
        var nonWeaponList = new List<EquipmentInfo>();

        foreach (var item in inventory.Equipments)
        {
            if (item.Data.EquipmentCategory != EEquipmentCategory.Weapon
             && !item.IsNftItem && !item.IsEquipped)
            {
                nonWeaponList.Add(item);
            }
        }

        return nonWeaponList;
    }

    public static List<ConsumableInfo> GetConsumables(this InventorySO inventory)
    {
        var consumableList = new List<ConsumableInfo>();

        foreach (var item in inventory.Consumables)
        {
            if (item.Data.ConsumableType != EConsumableType.Key)
            {
                consumableList.Add(item);
            }
        }

        return consumableList;
    }
}
