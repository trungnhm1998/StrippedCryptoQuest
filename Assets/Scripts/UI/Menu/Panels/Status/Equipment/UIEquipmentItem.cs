using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using PolyAndCode.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentItem : MultiInputButton, ICell
    {
        public static event UnityAction<UIStats.Equipment> InspectingEquipment;
        public static event UnityAction<Button> InspectingRow;
        public event UnityAction<int> SelectedEvent;

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
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _selectEffect;

        private GameObject _unEquipSlot;
        private int _index;

        public void Init(EquipmentInfo data, int index = 0)
        {
            _index = index;

            if (_unEquipSlot != null)
                _unEquipSlot.SetActive(false);

            _itemName.text = data.Item.name;
            if (data.Item != null && data.Item.DisplayName != null)
            {
                _name.StringReference = data.Item.DisplayName;
            }
        }

        public void OnPressed()
        {
            Debug.Log($"Inventory item pressed");
        }

        public override void OnSelect(BaseEventData eventData)
        {
            InspectingRow?.Invoke(this);
            SelectedEvent?.Invoke(_index);

            base.OnSelect(eventData);
            _selectEffect.SetActive(true);
            InspectingEquipment?.Invoke(CreateFakeData());
        }

        private UIStats.Equipment CreateFakeData()
        {
            var mockEquipment = new UIStats.Equipment();
            var numberOfModifiedAttributes = Random.Range(1, 22);
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