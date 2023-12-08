using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Shop.UI.Item;
using System.Collections.Generic;
using CryptoQuest.Item.Consumable;

public static class InventoryExtensions
{

    public static List<Equipment> GetWeapons(this InventorySO inventory)
    {
        var weaponList = new List<Equipment>();

        // foreach (var item in inventory.Equipments)
        // {
        //     if (item.Config.EquipmentCategory == EEquipmentCategory.Weapon
        //         && !item.IsNftItem)
        //     {
        //         weaponList.Add(item);
        //     }
        // }

        return weaponList;
    }

    public static List<Equipment> GetNonWeapons(this InventorySO inventory)
    {
        var nonWeaponList = new List<Equipment>();

        // foreach (var item in inventory.Equipments)
        // {
        //     if (item.Config.EquipmentCategory != EEquipmentCategory.Weapon
        //         && !item.IsNftItem)
        //     {
        //         nonWeaponList.Add(item);
        //     }
        // }

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
