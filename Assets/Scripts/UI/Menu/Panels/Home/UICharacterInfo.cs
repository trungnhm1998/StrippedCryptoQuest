using CryptoQuest.Gameplay.Character;
using CryptoQuest.UI.Character;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    public interface ICharacterInfo
    {
        public void SetLocalizedName(LocalizedString localizedName);
        public void SetName(string charName);
        public void SetClass(LocalizedString localizedClassName);
        public void SetAvatar(Sprite avatar);
        public void SetElement(Sprite elementIcon);
        public void SetLevel(int lvl);
        public void SetExp(float exp);
        public void SetMaxExp(int maxExp);
    }

    public interface ICharacterStats
    {
        public void SetCurrentHp(float currentHp);
        public void SetMaxHp(float maxHp);
        public void SetCurrentMp(float currentMp);
        public void SetMaxMp(float maxMp);
    }

    public class UICharacterInfo : MonoBehaviour, ICharacterInfo, ICharacterStats
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

        private CharacterSpec _memberInSlot;

        public void Init(CharacterSpec member)
        {
            _memberInSlot = member;
            member.SetupUI(this);
        }

        private void OnEnable()
        {
            _attributeChangeEvent.AttributeSystemReference = _memberInSlot.CharacterComponent.AttributeSystem;
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