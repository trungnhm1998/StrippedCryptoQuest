using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.PlayerParty.Helper;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {

        private List<Item.Equipment.IEquipment> _equipmentData;
        public List<Item.Equipment.IEquipment> Equipments => _equipmentData;
        private IPartyController _partyController;

        public IEnumerator CoGetData(InventorySO inventory)
        {
            _equipmentData = new();
            var equipments = new List<IEquipment>();
            _partyController ??= ServiceProvider.GetService<IPartyController>();
            equipments.AddRange(_partyController.GetEquippingEquipments());
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