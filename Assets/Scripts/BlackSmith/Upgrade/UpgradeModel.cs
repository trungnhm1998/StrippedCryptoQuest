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
            _partyController ??= ServiceProvider.GetService<IPartyController>();

            // Equipping equipments will not be removed from inventory
            foreach (var equipment in _inventory.Equipments)
            {
                if (equipment.Level >= equipment.Data.MaxLevel) continue;
                _equipmentData.Add(equipment);
            }

            yield break;
        }
    }
}