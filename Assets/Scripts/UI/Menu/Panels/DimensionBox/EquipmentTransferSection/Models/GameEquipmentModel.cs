using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;
using UnityEngine.Localization;
using Random = System.Random;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class GameEquipmentModel : MonoBehaviour, IGameEquipmentModel
    {
        private List<IGame> _gameEquipmentData;
        public List<IGame> Data => _gameEquipmentData;

        public IEnumerator CoGetData()
        {
            yield return new WaitForSeconds(1f);
            _gameEquipmentData = new List<IGame>();

            yield break;
        }
    }
}