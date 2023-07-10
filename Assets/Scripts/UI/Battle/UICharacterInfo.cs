using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using CryptoQuest.Gameplay.Battle;

namespace CryptoQuest.UI.Battle
{
    public class UICharacterInfo : MonoBehaviour
    {
        [SerializeField] private AttributeScriptableObject _maxHpAttributeSO;
        [SerializeField] private AttributeScriptableObject _hpAttributeSO;
        [SerializeField] private AttributeScriptableObject _maxMpAttributeSO;
        [SerializeField] private AttributeScriptableObject _mpAttributeSO;

        [Header("UI")]
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _hpValueText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Text _mpValueText;
        [SerializeField] private Slider _mpSlider;
        [SerializeField] private Image _characterIcon;

        private AbilitySystemBehaviour _owner;

        private void OnEnable()
        {
            _hpAttributeSO.ValueChangeEvent += OnHPChanged;
            _mpAttributeSO.ValueChangeEvent += OnHPChanged;
        }

        private void OnDisable()
        {
            _hpAttributeSO.ValueChangeEvent -= OnHPChanged;
            _mpAttributeSO.ValueChangeEvent -= OnHPChanged;
        }

        public void SetOwnerSystem(AbilitySystemBehaviour owner)
        {
            _owner = owner;
            InitUI();
        }

        private void InitUI()
        {
            if (_owner == null) return;
            var statsInitializer = _owner.GetComponent<StatsInitializer>();
            if (statsInitializer.DefaultStats is CharacterDataSO data)
            {
                _nameText.text = data.Name;
                _characterIcon.sprite = data.BattleIconSprite;
            }
            UpdateValueUI(_maxHpAttributeSO, _hpAttributeSO, _hpValueText, _hpSlider);
            UpdateValueUI(_maxMpAttributeSO, _mpAttributeSO, _mpValueText, _mpSlider);
        }

        private void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (_owner == null || args.System != _owner.AttributeSystem) return;

            UpdateValueUI(_maxHpAttributeSO, _hpAttributeSO, _hpValueText, _hpSlider);
        }

        private void OnMPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (_owner == null || args.System != _owner.AttributeSystem) return;

            UpdateValueUI(_maxMpAttributeSO, _mpAttributeSO, _mpValueText, _mpSlider);
        }

        private void UpdateValueUI(AttributeScriptableObject maxSO, AttributeScriptableObject attributeSO,
            Text valueText, Slider slider)
        {
            if (!_owner.AttributeSystem.GetAttributeValue(maxSO, out AttributeValue maxValue)) return;
            if (!_owner.AttributeSystem.GetAttributeValue(attributeSO, out AttributeValue attributValue)) return;

            valueText.text = attributValue.CurrentValue.ToString();
            slider.value = attributValue.CurrentValue / maxValue.CurrentValue;  
        }
    }
}