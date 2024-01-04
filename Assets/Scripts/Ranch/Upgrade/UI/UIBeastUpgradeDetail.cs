using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Beast;
using CryptoQuest.Beast.Avatar;
using UIAttribute = CryptoQuest.UI.Character.UIAttribute;
using CryptoQuest.UI.Tooltips;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIBeastUpgradeDetail : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _illustration;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private LocalizeStringEvent _LocalizePassiveSkill;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _passiveSkill;
        [SerializeField] private UIStars _startsUi;
        [SerializeField] private UIAttribute[] _attributeUIs;

        [SerializeField] private PreviewAttributeChangeEvent _previewAttributeChangeEvent;
        private IBeastAvatarProvider _beastAvatarProvider;

        private void OnValidate()
        {
            _attributeUIs = GetComponentsInChildren<UIAttribute>();
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
            PreviewBeastStats();
            ResetAttributesUI();
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

        public void PreviewBeastStats(bool isPreview = false)
        {
            _previewAttributeChangeEvent.enabled = isPreview;
        }

        public void ResetAttributesUI()
        {
            foreach (var attributeUI in _attributeUIs)
            {
                attributeUI.ResetAttributeUI();
            }
        }


        #endregion
    }
}