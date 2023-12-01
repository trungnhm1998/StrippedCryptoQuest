using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Menus.Status.UI.Stats;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI
{
    public class UICharacterStatsPanel : MonoBehaviour
    {
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        [Header("Character Info UI References")]
        [SerializeField] private Image _avatar;

        [SerializeField] private Image _characterElement;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private LocalizeStringEvent _localizedClassName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private List<UIElementAttribute> _elementAttributes;
        [SerializeField] private UIAttributeBar _expBar;

        private string _lvlTxtFormat = string.Empty; // could made this into static
        private AttributeSystemBehaviour _inspectingAttributeSystem;
        public void InspectCharacter(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            _inspectingAttributeSystem = hero.GetComponent<AttributeSystemBehaviour>();
            _attributeChangeEvent.AttributeSystemReference = _inspectingAttributeSystem;
            RenderElementsStats(_inspectingAttributeSystem);
            SetupUI(hero);
        }

        private void SetupUI(HeroBehaviour hero)
        {
            hero.TryGetComponent(out LevelSystem levelSystem);
            SetElement(hero.Element.Icon);
            SetLevel(levelSystem.Level);
            SetClass(hero.Class.Name);
            SetLocalizedName(hero.DetailsInfo.LocalizedName);

            SetAvatar(hero.Avatar);
            SetupExpUI(hero);
        }

        private void SetupExpUI(HeroBehaviour hero)
        {
            if (!hero.TryGetComponent<LevelSystem>(out var levelSystem)) return;
            SetMaxExp(levelSystem.GetNextLevelRequireExp());
            SetExp(levelSystem.GetCurrentLevelExp());
        }

        private void RenderElementsStats(AttributeSystemBehaviour attributeSystem)
        {
            foreach (var elementUI in _elementAttributes)
            {
                elementUI.SetStats(attributeSystem);
            }
        }

        #region Setup UI

        private void SetLocalizedName(LocalizedString localizedName) => _localizedName.StringReference = localizedName;

        private void SetName(string charName) => _name.text = charName;

        private void SetClass(LocalizedString localizedClassName) =>
            _localizedClassName.StringReference = localizedClassName;

        private void SetAvatar(Sprite avatar) => _avatar.sprite = avatar;

        private void SetElement(Sprite elementIcon) => _characterElement.sprite = elementIcon;

        private void SetExp(float exp) => _expBar.SetValue(exp);

        private void SetMaxExp(int maxExp) => _expBar.SetMaxValue(maxExp);

        private void SetLevel(int lvl)
        {
            if (string.IsNullOrEmpty(_lvlTxtFormat))
                _lvlTxtFormat = _level.text;
            _level.text = string.Format(_lvlTxtFormat, lvl);
        }

        #endregion
    }
}