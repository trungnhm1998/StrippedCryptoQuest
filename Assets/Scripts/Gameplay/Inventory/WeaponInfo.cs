using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class WeaponInfo : EquipmentInfo
    {
        public WeaponInfo() { }
        public WeaponInfo(WeaponSO itemSO) : base(itemSO) { }
    }
}