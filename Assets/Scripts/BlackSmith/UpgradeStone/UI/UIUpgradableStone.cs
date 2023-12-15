using System;
using CryptoQuest.BlackSmith.Commons.UI;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menu;
using CryptoQuest.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UIUpgradableStone : MonoBehaviour
    {
        public event Action<UIUpgradableStone> Selected;
        public event Action<UIUpgradableStone> Inspected;
        public event Action DeSelected;
        [field: SerializeField] public MultiInputButton Button { get; private set; }
        [field: SerializeField] public GameObject MaterialTag { get; private set; }
        [SerializeField] private Image _highlight;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private TMP_Text _lvlText;
        [SerializeField] private UICurrencyValueEnoughText _goldText;
        [SerializeField] private UICurrencyValueEnoughText _metadText;
        public IMagicStone MagicStone { get; private set; }
        public UpgradableStoneData MagicStoneData { get; private set; }

        private bool _isEnoughCurrency;

        public void Initialize(IMagicStone magicStone)
        {
            ResetItemStates();
            MagicStone = magicStone;
            _icon.LoadSpriteAndSet(magicStone.Definition.Image);
            _name.StringReference = magicStone.Definition.DisplayName;
            _lvlText.text = $"Lv.{magicStone.Level}";
        }

        public void InitPrice(ICurrencyValueEnough goldInfo, ICurrencyValueEnough metadInfo,
            UpgradableStoneData data)
        {
            _isEnoughCurrency = goldInfo.IsEnough && metadInfo.IsEnough;
            _goldText.SetValueAndCheckIsEnough(goldInfo);
            _metadText.SetValueAndCheckIsEnough(metadInfo);
            MagicStoneData = data;
        }

        private void OnEnable()
        {
            Button.Selected += OnSelected;
            Button.DeSelected += OnDeselected;
        }

        private void OnDisable()
        {
            Button.Selected -= OnSelected;
            Button.DeSelected -= OnDeselected;
            ResetItemStates();
        }

        private void OnDeselected()
        {
            DeSelected?.Invoke();
        }

        public void OnPressed()
        {
            if (!_isEnoughCurrency) return;
            Selected?.Invoke(this);
        }

        public void OnSelected()
        {
            Inspected?.Invoke(this);
        }

        private void ResetItemStates()
        {
            MaterialTag.SetActive(false);
            Button.interactable = true;
            Highlight(false);
        }

        public void Highlight(bool value)
        {
            _highlight.enabled = value;
            MaterialTag.SetActive(value);
        }
    }
}