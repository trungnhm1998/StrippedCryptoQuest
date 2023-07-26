using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

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
            _characterIcon.sprite = _characterData.BattleIconSprite;
            _nameText.text = _characterData.Name;
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

        protected override void OnSelected(string name)
        {
            var currentObject = EventSystem.current.currentSelectedGameObject;

            _selectFrame.gameObject.SetActive(currentObject == gameObject);
            _popupItemFrame.gameObject.SetActive(currentObject == gameObject);
            _itemPopupText.text = name;
        }

        public void OnSelect(BaseEventData eventData)
        {
            var currentObject = EventSystem.current.currentSelectedGameObject;

            _selectFrame.gameObject.SetActive(currentObject == gameObject);
            _popupItemFrame.gameObject.SetActive(currentObject == gameObject);
        }


        public void OnDeselect(BaseEventData eventData)
        {
            _selectFrame.gameObject.SetActive(false);
            _popupItemFrame.gameObject.SetActive(false);
        }

        public void OnCallback()
        {
            Debug.Log("UIHeroInfo::OnCallback");
        }
    }
}