using System;
using CryptoQuest.Gameplay.Character;
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
        public void SetClass(LocalizedString localizedClass);
        public void SetAvatar(Sprite avatar);
        public void SetElement(Sprite elementIcon);
        public void SetLevel(int lvl);
    }

    public interface ICharacterStats
    {
        public void SetCurrentHp(float currentHp);
        public void SetMaxHp(float maxHp);
        public void SetCurrentMp(float currentMp);
        public void SetMaxMp(float maxMp);
        public void SetExp(float exp);
        public void SetMaxExp(int maxExp);
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
        [SerializeField] private TMP_Text _currentHp;
        [SerializeField] private TMP_Text _maxHp;
        [SerializeField] private Image _HpBar;
        [SerializeField] private TMP_Text _currentMp;
        [SerializeField] private TMP_Text _maxMp;
        [SerializeField] private Image _MpBar;
        [SerializeField] private Image _ExpBar;

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
            _memberInSlot.CharacterComponent.AttributeSystem.UpdateAttributeValues();
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
            _currentHp.text = $"{(int)currentHp}";
        }

        public void SetMaxHp(float maxHp)
        {
            _maxHp.text = $"{(int)maxHp}";
        }

        public void SetCurrentMp(float currentMp)
        {
            _currentMp.text = $"{(int)currentMp}";
        }

        public void SetMaxMp(float maxMp)
        {
            _maxMp.text = $"{(int)maxMp}";
        }

        public void SetExp(float exp) { }

        public void SetMaxExp(int maxExp) { }

        public void SetClass(LocalizedString localizedClass)
        {
            _class.StringReference = localizedClass;
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