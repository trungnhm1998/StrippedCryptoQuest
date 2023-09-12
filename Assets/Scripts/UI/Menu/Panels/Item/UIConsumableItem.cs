using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIConsumableItem : MonoBehaviour, ICell
    {
        public static event Action<UIConsumableItem> Using;

        public delegate void InspectingItem(UIConsumableItem item);

        public event InspectingItem Inspecting;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _charaterX;
        [SerializeField] private Text _quantity;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _enabledColor;
        private UsableInfo _consumable;
        private bool _canClick;
        public UsableInfo Consumable => _consumable;

        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }

        private void OnEnable()
        {
            _button.Selected += OnInspectingItem;
            _button.DeSelected += OnDeselectItem;
        }


        private void OnDisable()
        {
            _button.Selected -= OnInspectingItem;
            _button.DeSelected -= OnDeselectItem;
        }

        public void OnUse()
        {
            if (!_canClick) return;
            Using?.Invoke(this);
        }

        public void Use()
        {
            _consumable.Use();
        }

        private void OnInspectingItem()
        {
            _selectedBackground.SetActive(true);
            Inspecting?.Invoke(this);
        }

        private void OnDeselectItem()
        {
            _selectedBackground.SetActive(false);
        }

        public void Init(UsableInfo item)
        {
            _consumable = item;
            _icon.sprite = item.Icon;
            _name.StringReference = item.DisplayName;
            _quantity.text = item.Quantity.ToString();

            var info = _consumable.Data.ItemAbilityInfo;
            bool isField = info.CheckUsageScenario(EAbilityUsageScenario.Field);

            SetColorText(isField);
        }

        private void SetColorText(bool isField = false)
        {
            var color = isField ? _enabledColor : _disabledColor;

            _nameText.color = color;
            _charaterX.color = color;
            _quantity.color = color;

            _canClick = isField;
        }


        public void Inspect()
        {
            _button.Select();
            OnInspectingItem();
        }
    }
}