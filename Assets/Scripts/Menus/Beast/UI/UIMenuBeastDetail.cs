using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Beast;
using CryptoQuest.Beast.Avatar;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.UI.Tooltips;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UIAttribute = CryptoQuest.UI.Character.UIAttribute;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIMenuBeastDetail : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        [Header("Configs")]
        [SerializeField] private LocalizeStringEvent _localizedPassiveSkill;

        [SerializeField] private LocalizeStringEvent _localizedBeastName;
        [SerializeField] private TMP_Text _txtLevel;
        [SerializeField] private TMP_Text _txtPassiveSkill;
        [SerializeField] private TMP_Text _txtPassiveSkillPercent;
        [SerializeField] private TMP_Text _txtBeastName;
        [SerializeField] private Image _beastImage;
        [SerializeField] private Image _beastElement;

        [Header("Components")]
        [SerializeField] private UIStars _stars;

        [SerializeField] private List<UIAttribute> _attributeBar;

        [Header("Events")]
        [SerializeField] private BeastEventChannel _showBeastDetailsEventChannel;

        [SerializeField] private BeastAttributeSystemSO _beastAttributeSystemSo;

        private string _lvlFormat = string.Empty;
        private string _passiveFormat = string.Empty;

        private void OnEnable()
        {
            _showBeastDetailsEventChannel.EventRaised += FillUI;
            _beastAttributeSystemSo.EventRaised += UpdateBeastStats;
        }

        private void OnDisable()
        {
            _showBeastDetailsEventChannel.EventRaised -= FillUI;
            _beastAttributeSystemSo.EventRaised -= UpdateBeastStats;
        }

        public void FillUI(IBeast beast)
        {
            SetPassiveSkillPercent(0);
            SetBeastName(beast.LocalizedName);
            SetPassiveSkill(beast.Passive);
            SetElement(beast.Elemental);
            SetStars(beast.Stars);
            SetLevel(beast.Level);
            SetAvatar(beast);
        }

        #region Setup

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

        private void SetAvatar(IBeast beast)
        {
            bool hasAvatarProvider = TryGetComponent<IBeastAvatarProvider>(out var converter);
            if (!hasAvatarProvider)
            {
                Debug.LogWarning("UIBeastDetail::SetAvatar::Missing IBeastAvatarProvider");
                return;
            }

            StartCoroutine(converter.LoadAvatarAsync(_beastImage, beast));
        }

        private void SetPassiveSkill(PassiveAbility beastPassive)
        {
            _txtPassiveSkill.text = string.Empty;
            _txtPassiveSkillPercent.text = string.Empty;

            if (beastPassive == null) return;

            SetPassiveSkillPercent(beastPassive.Context.SkillInfo.SkillParameters.BasePower);

            _localizedPassiveSkill.StringReference = beastPassive.Description ?? new LocalizedString();
            _localizedPassiveSkill.RefreshString();
        }

        private void SetPassiveSkillPercent(float value)
        {
            if (_passiveFormat == string.Empty)
            {
                _passiveFormat = _txtPassiveSkillPercent.text;
            }

            _txtPassiveSkillPercent.text = string.Format(_passiveFormat, value);
        }

        private void SetLevel(int value)
        {
            if (_lvlFormat == string.Empty)
            {
                _lvlFormat = _txtLevel.text;
            }

            _txtLevel.text = string.Format(_lvlFormat, value);
        }

        private void SetBeastName(LocalizedString beastName)
        {
            _txtBeastName.text = string.Empty;
            _localizedBeastName.StringReference = beastName ?? new LocalizedString();
            _localizedBeastName.RefreshString();
        }

        private void SetStars(int value) => _stars.SetStars(value);

        private void SetElement(Elemental element) => _beastElement.sprite = element.Icon;

        public void SetEnabled(bool isActive = true) => _content.SetActive(isActive);

        #endregion
    }
}