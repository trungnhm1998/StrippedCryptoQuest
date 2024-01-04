using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Beast.Avatar;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.UI.Tooltips;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UIAttribute = CryptoQuest.UI.Character.UIAttribute;

namespace CryptoQuest.Ranch.Tooltip
{
    public interface ITooltipBeastProvider
    {
        public IBeast Beast { get; }
    }

    public class UIBeastTooltip : MonoBehaviour
    {
        [SerializeField] private BeastAttributeSystemSO _beastAttributeSystemSo;
        [SerializeField] private CalculatorBeastStatsSO _calculatorBeastStatsSo;
        [SerializeField] private Image _illustration;
        [SerializeField] private Image _element;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _passiveDescription;
        [SerializeField] private TextMeshProUGUI _passiveValue;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private LocalizeStringEvent _passiveSkillLocalize;
        [SerializeField] private UIStars _uiStars;
        [SerializeField] private List<UIAttribute> _attributeBar;
        protected IBeast _beast;
        private IBeastAvatarProvider _beastAvatarProvider;

        private void OnEnable()
        {
            _beastAttributeSystemSo.EventRaised += UpdateBeastStats;
        }

        private void OnDisable()
        {
            _beast = null;
            _beastAttributeSystemSo.EventRaised -= UpdateBeastStats;
        }

        public void Init(IBeast beast)
        {
            _beast = beast;
            SetupInfo();
            SetupSkills();
        }

        private void SetupInfo()
        {
            _calculatorBeastStatsSo.RaiseEvent(_beast);
            _beastAvatarProvider = GetComponent<IBeastAvatarProvider>();
            _nameLocalize.StringReference = _beast.LocalizedName;
            _level.text = $"Lv{_beast.Level.ToString()}";
            _element.sprite = _beast.Elemental.Icon;
            _uiStars.SetStars(_beast.Stars);
            StartCoroutine(_beastAvatarProvider.LoadAvatarAsync(_illustration, _beast));
        }

        private void SetupSkills()
        {
            _passiveDescription.text = "";
            if (_beast.Passive != null)
            {
                _passiveSkillLocalize.StringReference = _beast.Passive.Description;
                _passiveValue.text = $"{_beast.Passive.Context.SkillInfo.SkillParameters.BasePower}%";
            }
            else
            {
                _passiveSkillLocalize.StringReference = new LocalizedString();
                _passiveValue.text = "";
            }
            _passiveSkillLocalize.RefreshString();
        }

        private void UpdateBeastStats(AttributeSystemBehaviour attributeValues)
        {
            foreach (var attribute in attributeValues.AttributeValues)
            {
                foreach (var attributeValue in _attributeBar)
                {
                    if (attribute.Attribute == attributeValue.Attribute)
                    {
                        attributeValue.SetValue(attribute.BaseValue);
                    }
                }
            }
        }
    }
}