using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {
        private List<IUpgradeEquipment> _upgradeData;
        public List<IUpgradeEquipment> ListEquipment => _upgradeData;
        public void CoGetData(InventorySO inventory)
        {
            _upgradeData = new();
            var listEquipment = inventory.Equipments;
            foreach (var equipment in listEquipment)
            {
                IUpgradeEquipment equipmentData = new MockUpgradeEquipment(equipment);
                if (equipment.Level < equipment.Def.MaxLevel)
                    _upgradeData.Add(equipmentData);
            }
        }
    }
}