using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.PlayerParty.Helper;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {
        [SerializeField] private EquipmentInventory _inventory;
        
        private List<IEquipment> _equipmentData;
        public List<IEquipment> Equipments => _equipmentData;
        private IPartyController _partyController;

        public IEnumerator CoGetData()
        {
            _equipmentData = new();
            var equipments = new List<IEquipment>();
            _partyController ??= ServiceProvider.GetService<IPartyController>();
            equipments.AddRange(_partyController.GetEquippingEquipments());
            equipments.AddRange(_inventory.Equipments);
            
            foreach (var equipment in equipments)
            {
                if (equipment.Level >= equipment.Data.MaxLevel) continue;
                _equipmentData.Add(equipment);
            }

            yield break;
        }
    }
}