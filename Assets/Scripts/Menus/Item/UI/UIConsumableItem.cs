using System.Collections;
using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Gameplay;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Map;
using CryptoQuest.Menu;
using CryptoQuest.UI.Extensions;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Item.UI
{
    public class UIConsumableItem : MonoBehaviour
    {
        public delegate void UIConsumableItemEvent(UIConsumableItem item);

        public static event UIConsumableItemEvent Inspecting;
        public static event UIConsumableItemEvent Using;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _quantity;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _enabledColor;
        private ConsumableInfo _consumable;
        public ConsumableInfo Consumable => _consumable;
        private bool _canClick;
        private ICurrentSceneProvider _currentSceneProvider;

        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }

        private void OnEnable()
        {
            _button.Selected += OnInspectingItem;
            _button.DeSelected += OnDeselectItem;
            _button.onClick.AddListener(OnUse);
            _currentSceneProvider = ServiceProvider.GetService<ICurrentSceneProvider>();
        }

        private void OnDisable()
        {
            _button.Selected -= OnInspectingItem;
            _button.DeSelected -= OnDeselectItem;
            _button.onClick.RemoveListener(OnUse);
            CancelInvoke(nameof(SelectButton));
            if (_consumable != null) _consumable.QuantityChanged -= SetQuantityText;
        }

        public void OnUse()
        {
            if (!_canClick) return;
            Using?.Invoke(this);
            _consumable.Consuming(); // this will show a correct UI
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
            if (_consumable != null) _consumable.QuantityChanged -= SetQuantityText;

            _consumable = item;
            StartCoroutine(CoLoadIcon());
            _name.StringReference = item.DisplayName;
            SetQuantityText(item);

            var allowedInField = IsCorrectScenario(_consumable.Data.UsageScenario);
            SetColorText(allowedInField);

            _consumable.QuantityChanged += SetQuantityText;
        }

        public void SetQuantityText(ConsumableInfo item)
        {
            _quantity.text = $"x{item.Quantity}";
        }

        private IEnumerator CoLoadIcon()
        {
            if (_consumable.Icon == null) yield break;
            if (!_consumable.Icon.RuntimeKeyIsValid()) yield break;

            _icon.LoadSpriteAndSet(_consumable.Icon);
        }

        private void SetColorText(bool allowed = false)
        {
            var color = allowed ? _enabledColor : _disabledColor;

            _nameText.color = color;
            _quantity.color = color;

            _canClick = allowed;
        }

        private const float ERROR_PRONE_DELAY = 0.01f;

        public void Inspect()
        {
            Invoke(nameof(SelectButton), ERROR_PRONE_DELAY);
            OnInspectingItem();
        }

        private void SelectButton()
        {
            _button.Select();
        }

        private bool IsCorrectScenario(EAbilityUsageScenario scenario)
        {
            return ScenarioProvider.IsCorrectScenario(scenario);
        }
    }
}