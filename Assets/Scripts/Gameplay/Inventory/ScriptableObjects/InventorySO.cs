using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Inventory")]
    public class InventorySO : ScriptableObject
    {
        [field: SerializeField] public List<UsableInfo> UsableItems { get; private set; }
        [field: SerializeField] public List<EquipmentInfo> Equipments { get; private set; }


        [SerializeField] private List<WeaponInfo> _weapons = new();

        public List<WeaponInfo> Weapons
        {
            get
            {
#if UNITY_EDITOR
                for (int i = 0; i < _weapons.Count; i++)
                {
                    if (!(_weapons[i].Item is WeaponSO currentWeapon)) continue;
                    if (currentWeapon != null) continue;
                    WeaponInfo weaponInfo = new(currentWeapon as WeaponSO);
                    _weapons[i] = weaponInfo;
                }
#endif
                return _weapons;
            }
        }
    }
}