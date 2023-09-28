using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;
using UnityEngine.Localization;
using Random = System.Random;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class MockGameEquipmentModel : MonoBehaviour, IGameEquipmentModel
    {
        public int MockLength;
        public Sprite[] MockIcon;
        public LocalizedString MockName;

        private List<IData> _mockData;
        public List<IData> Data => _mockData;

        public IEnumerator CoGetData()
        {
            yield return new WaitForSeconds(1f);
            _mockData = new List<IData>();

            for (var i = 0; i < MockLength; i++)
            {
                Random rand = new Random();
                bool isEquipped = rand.Next(100) < 20 ? true : false;

                var obj = new MockData(MockIcon[i], MockName, isEquipped);
                _mockData.Add(obj);
            }

            yield break;
        }
    }
}