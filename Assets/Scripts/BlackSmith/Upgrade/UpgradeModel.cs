using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {

        private List<Item.Equipment.IEquipment> _equipmentData;
        public List<Item.Equipment.IEquipment> Equipments => _equipmentData;

        public IEnumerator CoGetData(InventorySO inventory)
        {
            _equipmentData = new();
            var equipments = new List<EquipmentInfo>();
            equipments.AddRange(inventory.NftEquipments);
            equipments.AddRange(inventory.Equipments);
            foreach (var equipment in equipments)
            {
                if (equipment.Level >= equipment.Data.MaxLevel) continue;
                _equipmentData.Add(equipment);
            }

            yield break;
        }
    }
}