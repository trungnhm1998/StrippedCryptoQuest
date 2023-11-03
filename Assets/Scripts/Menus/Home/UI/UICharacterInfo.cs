using CryptoQuest.Battle.Components;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Home.UI
{
    public class UICharacterInfo : MonoBehaviour
    {
        // general info
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private LocalizeStringEvent _class;
        [SerializeField] private Image _element;
        [SerializeField] private Image _avatar;

        // stats
        [SerializeField] private UIAttributeBar _hpBar;
        [SerializeField] private UIAttributeBar _mpBar;
        [SerializeField] private UIAttributeBar _expBar;

        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        private string _lvlFormat = string.Empty;

        private HeroBehaviour _hero;

        public void Init(HeroBehaviour hero)
        {
            _hero = hero;
            _hero.TryGetComponent(out LevelSystem levelSystem);
            SetLocalizedName(_hero.DetailsInfo.LocalizedName);
            SetElement(_hero.Element.Icon);
            SetLevel(levelSystem.Level);
            SetClass(_hero.Class.Name);
            SetupExpUI(hero);
        }

        private void SetupExpUI(HeroBehaviour hero)
        {
            if (!hero.TryGetComponent<LevelSystem>(out var levelSystem)) return;
            SetMaxExp(levelSystem.GetNextLevelRequireExp());
            SetExp(levelSystem.GetCurrentLevelExp());
        }

        private void OnEnable()
        {
            if (_hero == null || _hero.IsValid() == false) return;
            _attributeChangeEvent.AttributeSystemReference = _hero.GetComponent<AttributeSystemBehaviour>();
        }

        #region Setup

        public void SetLocalizedName(LocalizedString localizedName)
        {
            _localizedName.StringReference = localizedName;
        }

        public void SetName(string charName)
        {
            _name.text = charName;
        }

        public void SetCurrentHp(float currentHp)
        {
            _hpBar.SetValue(currentHp);
        }

        public void SetMaxHp(float maxHp)
        {
            _hpBar.SetMaxValue(maxHp);
        }

        public void SetCurrentMp(float currentMp)
        {
            _mpBar.SetValue(currentMp);
        }

        public void SetMaxMp(float maxMp)
        {
            _mpBar.SetMaxValue(maxMp);
        }

        public void SetExp(float exp)
        {
            _expBar.SetValue(exp);
        }

        public void SetMaxExp(int maxExp)
        {
            _expBar.SetMaxValue(maxExp);
        }

        public void SetClass(LocalizedString localizedClassName)
        {
            _class.StringReference = localizedClassName;
        }

        public void SetElement(Sprite element)
        {
            _element.sprite = element;
        }

        public void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }

        public void SetLevel(int lvl)
        {
            if (_lvlFormat == string.Empty)
            {
                _lvlFormat = _level.text;
            }

            _level.text = string.Format(_lvlFormat, lvl);
        }

        #endregion
    }
}