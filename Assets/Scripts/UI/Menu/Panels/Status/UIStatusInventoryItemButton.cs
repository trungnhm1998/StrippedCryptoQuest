using System;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Stats;
using IndiGames.Core.Events.ScriptableObjects;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIStatusInventoryItemButton : MultiInputButton, ICell
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

        [Header("Game Components")]
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _itemOrder;
        [SerializeField] private GameObject _selectEffect;
        [SerializeField] private FloatEventChannelSO CompareValueEvent;

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

        public void OnPressed()
        {
            Debug.Log($"Inventory item pressed");
            // _uiAttribute.CompareValue(120);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _selectEffect.SetActive(true);

            CompareValueEvent.RaiseEvent(120);
            Debug.Log($"Inventory");
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            _selectEffect.SetActive(false);
        }
    }
}