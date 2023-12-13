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
    public class UIBeastDetail : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        [Header("Configs")]
        [SerializeField] private LocalizeStringEvent _beastName;

        [SerializeField] private LocalizeStringEvent _localizedPassiveSkill;
        [SerializeField] private TMP_Text _txtPassiveSkill;
        [SerializeField] private TMP_Text _txtLevel;
        [SerializeField] private Image _beastImage;
        [SerializeField] private Image _beastElement;

        [Header("Components")]
        [SerializeField] private UIStars _stars;

        [SerializeField] private List<UIAttribute> _attributeBar;

        [Header("Events")]
        [SerializeField] private BeastEventChannel _showBeastDetailsEventChannel;

        [SerializeField] private BeastAttributeSystemSO _beastAttributeSystemSo;

        private string _lvlFormat = string.Empty;

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
            SetPassiveSkill(beast.Passive);
            SetBeastName(beast.LocalizedName);
            SetElement(beast.Elemental);
            SetStars(beast.Stars);
            SetLevel(beast.Level);
            SetAvatar(beast);
        }

        #region Setup

        private void UpdateBeastStats(AttributeSystemBehaviour attributeValues)
        {
            Debug.Log("UIBeastDetail::UpdateBeastStats");
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
            _txtPassiveSkill.text = "";
            _localizedPassiveSkill.StringReference =
                beastPassive != null ? beastPassive.Description : new LocalizedString();
            _localizedPassiveSkill.RefreshString();
        }

        private void SetLevel(int value)
        {
            if (_lvlFormat == string.Empty)
            {
                _lvlFormat = _txtLevel.text;
            }

            _txtLevel.text = string.Format(_lvlFormat, value);
        }

        private void SetBeastName(LocalizedString beastName) => _beastName.StringReference = beastName;

        private void SetStars(int value) => _stars.SetStars(value);

        private void SetElement(Elemental element) => _beastElement.sprite = element.Icon;

        public void SetEnabled(bool isActive = true) => _content.SetActive(isActive);

        #endregion
    }
}