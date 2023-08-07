using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Weapon Item", menuName = "Crypto Quest/Inventory/Weapon Item")]
    public class WeaponSO : EquipmentSO
    {
        public new WeaponTypeSO WeaponType => (WeaponTypeSO)EquipmentType;
    }
}