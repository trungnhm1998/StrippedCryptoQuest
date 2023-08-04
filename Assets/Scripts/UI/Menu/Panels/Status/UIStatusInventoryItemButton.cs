using System;
using System.Collections.Generic;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Stats;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIStatusInventoryItemButton : MultiInputButton, ICell
    {
        public static event UnityAction<UIStats.Equipment> InspectingEquipment;
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

        [Header("Mock")]
        [SerializeField] private List<AttributeScriptableObject> _allAttributeToRandomFrom;

        [Header("Game Components")]
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _itemOrder;
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

        public void OnPressed()
        {
            Debug.Log($"Inventory item pressed");
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _selectEffect.SetActive(true);
            InspectingEquipment?.Invoke(CreateFakeData());
            Debug.Log($"Inventory");
        }

        private UIStats.Equipment CreateFakeData()
        {
            var mockEquipment = new UIStats.Equipment();
            var numberOfModifiedAttributes = Random.Range(1, 3);
            for (int i = 0; i < numberOfModifiedAttributes; i++)
            {
                mockEquipment.ModifiedAttributes.Add(_allAttributeToRandomFrom[i], Random.Range(50, 200));
            }
            return mockEquipment;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            _selectEffect.SetActive(false);
        }
    }
}