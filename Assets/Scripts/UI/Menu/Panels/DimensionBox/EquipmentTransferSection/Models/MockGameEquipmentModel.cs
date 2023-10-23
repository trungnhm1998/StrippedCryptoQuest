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

        private List<IGame> _mockData;
        public List<IGame> Data => _mockData;

        public IEnumerator CoGetData()
        {
            yield return new WaitForSeconds(1f);
            _mockData = new List<IGame>();

            for (var i = 0; i < MockLength; i++)
            {
                Random rand = new Random();
                bool isEquipped = rand.Next(100) < 20 ? true : false;
                int rdIconIdx = rand.Next(MockIcon.Length - 1);

                var obj = new MockData(MockIcon[rdIconIdx], MockName, isEquipped);
                _mockData.Add(obj);
            }

            yield break;
        }
    }
}