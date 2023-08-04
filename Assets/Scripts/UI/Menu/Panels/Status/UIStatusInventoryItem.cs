using System;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIStatusInventoryItem : MonoBehaviour, ICell
    {
        [Serializable]
        public class MockData
        {
            public LocalizedString Name;

            public MockData Clone()
            {
                return new MockData()
                {
                    Name = Name,
                };
            }
        }

        [SerializeField] LocalizeStringEvent _name;
        [SerializeField] Text _itemOrder;
        [SerializeField] private GameObject _selectEffect;

        private GameObject _unEquipSlot;

        public void Init(MockData mockData, int index)
        {
            if (_unEquipSlot != null)
            {
                _unEquipSlot.SetActive(false);
            }

            _name.StringReference = mockData.Name;

            _itemOrder.text = index.ToString();
        }
    }
}