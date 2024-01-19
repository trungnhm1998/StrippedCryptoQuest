using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Inventory.Helper
{
    public static class IEquipmentExtension
    {
        public static bool CanEquipByHero(this IEquipment equipment, HeroBehaviour hero)
        {
            if (!equipment.IsValid()) return false;

            hero.TryGetComponent(out LevelSystem levelSystem);
            if (equipment.Data.RequiredCharacterLevel > levelSystem.Level)
                return false;

            var equipmentAllowedClasses = equipment.Prefab.EquipmentType.AllowedClasses;
            if (!Array.Exists(equipmentAllowedClasses, allowedClass => allowedClass == hero.Class))
                return false;

            return true;
        }
    }
}