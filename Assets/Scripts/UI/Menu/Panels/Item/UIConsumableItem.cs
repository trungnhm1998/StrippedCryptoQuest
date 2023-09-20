using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIConsumableItem : MonoBehaviour
    {
        public delegate void UIConsumableItemEvent(UIConsumableItem item);

        public static event UIConsumableItemEvent Inspecting;
        public static event UIConsumableItemEvent Using;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _charaterX;
        [SerializeField] private Text _quantity;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _enabledColor;
        private ConsumableInfo _consumable;
        public ConsumableInfo Consumable => _consumable;
        private bool _canClick;

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
            _consumable.ConsumeWithCorrectUI(); // this will show a correct UI
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

        public void Init(ConsumableInfo item)
        {
            _consumable = item;
            _icon.sprite = item.Icon;
            _name.StringReference = item.DisplayName;
            _quantity.text = item.Quantity.ToString();


            var allowedInField = (_consumable.Data.UsageScenario & EAbilityUsageScenario.Field) > 0;
            SetColorText(allowedInField);
        }

        private void SetColorText(bool allowed = false)
        {
            var color = allowed ? _enabledColor : _disabledColor;

            _nameText.color = color;
            _charaterX.color = color;
            _quantity.color = color;

            _canClick = allowed;
        }


        public void Inspect()
        {
            _button.Select();
            OnInspectingItem();
        }
    }
}