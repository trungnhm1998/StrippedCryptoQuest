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

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIBeastUpgradeDetail : MonoBehaviour
    {
        private const int MAX_STAR = 5;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _illustration;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private LocalizeStringEvent _LocalizePassiveSkill;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _passiveSkill;
        [SerializeField] private List<UIAttribute> _attributeBar;
        [SerializeField] private BeastAttributeSystemSO _beastAttributeSystemSo;
        [SerializeField] private UIStars _startsUi;

        private IBeastAvatarProvider _beastAvatarProvider;

        private void OnEnable()
        {
            _beastAttributeSystemSo.EventRaised += UpdateBeastStats;
        }

        private void OnDisable()
        {
            _beastAttributeSystemSo.EventRaised -= UpdateBeastStats;
        }

        public void SetupUI(IBeast beast)
        {
            _beastAvatarProvider = GetComponent<IBeastAvatarProvider>();
            SetIcon(beast.Elemental);
            SetDisplayName(beast.LocalizedName);
            SetPassiveSkill(beast.Passive);
            SetLevel(beast.Level);
            SetBeastStar(beast.Stars);
            StartCoroutine(_beastAvatarProvider.LoadAvatarAsync(_illustration, beast));
        }

        #region Setup

        private void SetLevel(int beastLevel)
        {
            _level.text = $"Lv{beastLevel.ToString()}";
        }

        private void SetDisplayName(LocalizedString beastLocalizedName)
        {
            _displayName.StringReference = beastLocalizedName;
        }

        private void SetIcon(Elemental beastElemental)
        {
            _icon.sprite = beastElemental.Icon;
        }

        private void SetBeastStar(int value)
        {
            _startsUi.SetStars(value);
        }

        private void SetPassiveSkill(PassiveAbility beastPassive)
        {
            _passiveSkill.text = "";
            _LocalizePassiveSkill.StringReference =
                beastPassive != null ? beastPassive.Description : new LocalizedString();
            _LocalizePassiveSkill.RefreshString();
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

        #endregion
    }
}