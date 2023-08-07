using System.Collections;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using TMPro;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CharacterInfo
{
    public class UIHeroInfo : CharacterInfoBase, IDeselectHandler, ISelectHandler
    {
        [SerializeField] private AttributeScriptableObject _maxHpAttributeSO;
        [SerializeField] private AttributeScriptableObject _maxMpAttributeSO;
        [SerializeField] private AttributeScriptableObject _mpAttributeSO;

        [Header("UI")]
        [SerializeField] private Text _nameText;

        [SerializeField] private Text _hpValueText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Text _mpValueText;
        [SerializeField] private Slider _mpSlider;
        [SerializeField] private Image _selectFrame;
        [SerializeField] private Image _popupItemFrame;
        [SerializeField] protected Image _characterIcon;
        [SerializeField] private TextMeshProUGUI _itemPopupText;
        [SerializeField] private Button _button;

        protected override void OnEnable()
        {
            base.OnEnable();
            _mpAttributeSO.ValueChangeEvent += OnMPChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _mpAttributeSO.ValueChangeEvent -= OnMPChanged;
        }

        protected override void Setup()
        {
            //TODO: Refactor this later
            if (_characterInfo.Data is HeroDataSO heroData)
            {
                _characterIcon.sprite = heroData.BattleIconSprite;
            }
            _nameText.text = _characterInfo.DisplayName;
            UpdateValueUI(_maxHpAttributeSO, _hpAttributeSO, _hpValueText, _hpSlider);
            UpdateValueUI(_maxMpAttributeSO, _mpAttributeSO, _mpValueText, _mpSlider);
        }

        protected override void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (args.System != _attributeSystem) return;

            UpdateValueUI(_maxHpAttributeSO, _hpAttributeSO, _hpValueText, _hpSlider);
        }

        private void OnMPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (args.System != _attributeSystem) return;

            UpdateValueUI(_maxMpAttributeSO, _mpAttributeSO, _mpValueText, _mpSlider);
        }

        private void UpdateValueUI(AttributeScriptableObject maxSO, AttributeScriptableObject attributeSO,
            Text valueText, Slider slider)
        {
            if (_attributeSystem == null) return;

            _attributeSystem.GetAttributeValue(maxSO, out AttributeValue maxValue);
            _attributeSystem.GetAttributeValue(attributeSO, out AttributeValue attributeValue);

            valueText.text = attributeValue.CurrentValue.ToString();

            if (maxValue.CurrentValue == 0) return;
            slider.value = attributeValue.CurrentValue / maxValue.CurrentValue;
        }

        private bool IsCurrentlySelected => EventSystem.current.currentSelectedGameObject == gameObject &&
                                            _button.interactable;

        protected override void OnSelected(string name)
        {
            _button.interactable = true;

            _selectFrame.gameObject.SetActive(IsCurrentlySelected);
            _popupItemFrame.gameObject.SetActive(IsCurrentlySelected);

            _itemPopupText.text = name;
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (!_button.interactable) return;

            _selectFrame.gameObject.SetActive(true);
            _popupItemFrame.gameObject.SetActive(true);
        }


        public void OnDeselect(BaseEventData eventData)
        {
            _selectFrame.gameObject.SetActive(false);
            _popupItemFrame.gameObject.SetActive(false);
        }

        public void OnClicked()
        {
            Debug.Log("UIHeroInfo::OnCallback");
        }

        public void SetButtonActive(bool isActive)
        {
            _button.interactable = isActive;
        }
    }
}