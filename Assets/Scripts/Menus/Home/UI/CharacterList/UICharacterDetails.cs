using CryptoQuest.Battle.Components;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Home.UI.CharacterList
{
    public class UICharacterDetails : MonoBehaviour
    {
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        [Header("UI References")]
        [SerializeField] private Image _avatar;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _characterElement;
        [SerializeField] private UIAttributeBar _expBar;

        private AttributeSystemBehaviour _inspectingAttributeSystem;
        [SerializeField] private string _lvlTxtFormat = "Lv. {0}";

        public void InspectCharacter(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            _inspectingAttributeSystem = hero.GetComponent<AttributeSystemBehaviour>();
            _attributeChangeEvent.AttributeSystemReference = _inspectingAttributeSystem;
            SetupUI(hero);
        }

        private void SetupUI(HeroBehaviour hero)
        {
            hero.TryGetComponent(out LevelSystem levelSystem);
            // SetElement(hero.Element.Icon);
            SetLevel(levelSystem.Level);
            SetLocalizedName(hero.DetailsInfo.LocalizedName);

            // SetAvatar(hero.Avatar);
            SetupExpUI(hero);
        }

        private void SetupExpUI(HeroBehaviour hero)
        {
            if (!hero.TryGetComponent<LevelSystem>(out var levelSystem)) return;
            SetMaxExp(levelSystem.GetNextLevelRequireExp());
            SetExp(levelSystem.GetCurrentLevelExp());
        }

        #region Setup UI

        private void SetLocalizedName(LocalizedString localizedName) => _localizedName.StringReference = localizedName;

        private void SetName(string charName) => _name.text = charName;

        private void SetAvatar(Sprite avatar) => _avatar.sprite = avatar;

        private void SetElement(Sprite elementIcon) => _characterElement.sprite = elementIcon;

        private void SetExp(float exp) => _expBar.SetValue(exp);

        private void SetMaxExp(int maxExp) => _expBar.SetMaxValue(maxExp);

        private void SetLevel(int lvl)
        {
            _level.text = string.Format(_lvlTxtFormat, lvl);
        }

        #endregion
    }
}