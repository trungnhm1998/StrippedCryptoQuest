using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using CryptoQuest.System;
using UnityEngine;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class GameEquipmentModel : MonoBehaviour, IGameEquipmentModel
    {
        private List<IGame> _gameEquipmentData;
        public List<IGame> Data => _gameEquipmentData;

        public IEnumerator CoGetData()
        {
            _gameEquipmentData = new List<IGame>();

            var equipments = LoadEquipmentsFromInventory();

            foreach (var item in equipments)
            {
                var obj = new GameEquipmentData(item);
                _gameEquipmentData.Add(obj);
            }

            yield break;
        }

        private List<EquipmentInfo> LoadEquipmentsFromInventory()
        {
            return ServiceProvider.GetService<IInventoryController>().Inventory.Equipments;
        }
    }
}