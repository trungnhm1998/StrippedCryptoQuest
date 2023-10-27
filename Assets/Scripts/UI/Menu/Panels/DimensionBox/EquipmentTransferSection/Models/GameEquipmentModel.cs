using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using CryptoQuest.System;
using UnityEngine;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.PlayerParty.Helper;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class GameEquipmentModel : MonoBehaviour, IGameEquipmentModel
    {
        private List<IGame> _gameEquipmentData;
        public List<IGame> Data => _gameEquipmentData;

        private IPartyController _partyController;

        public IEnumerator CoGetData()
        {
            _gameEquipmentData = new List<IGame>();

            var equipments = LoadEquipmentsFromInventory();

            foreach (var equipment in equipments)
            {
                AddEquipmentData(equipment);
            }

            foreach (var equipment in LoadEquippedEquipments())
            {
                AddEquipmentData(equipment);
            }

            yield break;
        }

        private void AddEquipmentData(EquipmentInfo equipment)
        {
            var obj = new GameEquipmentData(equipment);
            _gameEquipmentData.Add(obj);
        }

        private List<EquipmentInfo> LoadEquipmentsFromInventory()
        {
            return ServiceProvider.GetService<IInventoryController>().Inventory.Equipments;
        }

        private IEnumerable<EquipmentInfo> LoadEquippedEquipments()
        {
            _partyController ??= ServiceProvider.GetService<IPartyController>();
            return _partyController.GetEquippedEquipments();
        }
    }
}