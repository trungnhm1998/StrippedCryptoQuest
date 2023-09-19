using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Interfaces;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class MockEquipmentModel : MonoBehaviour, IEquipmentModel
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
                var obj = new MockData(MockIcon[i], MockName);
                _mockData.Add(obj);
            }

            yield break;
        }
    }
}