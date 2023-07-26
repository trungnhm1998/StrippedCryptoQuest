using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.MockData;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Home
{
    public class UICharacterInfo : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private GameObject _content;
        [SerializeField] private Text _name;
        [SerializeField] private Text _level;
        [SerializeField] private Text _currentHP;
        [SerializeField] private Text _maxHP;
        [SerializeField] private Image _HPBar;
        [SerializeField] private Text _currentMP;
        [SerializeField] private Text _maxMP;
        [SerializeField] private Image _MPBar;
        [SerializeField] private Image _EXPBar;
        [SerializeField] private Text _class;
        [SerializeField] private Image _elemental;
        [SerializeField] private Image _avatar;

        [Header("Data")]
        private CharInfoMockDataSO _charInfoMockData;
        public CharInfoMockDataSO CharInfoMockData { get => _charInfoMockData; set => _charInfoMockData = value; }

        private void Start()
        {
            CheckExistence();
        }

        private void CheckExistence()
        {
            if (CharInfoMockData == null)
                _content.SetActive(false);
            else
            {
                _content.SetActive(true);
                InitCharacterInfo();
            }
        }

        private void InitCharacterInfo()
        {
            SetName();
            SetLevel();
            SetHP();
            SetHP();
            SetMP();
            SetEXP();
            SetClass();
            SetElemental();
            SetAvatar();
        }

        private void SetName()
        {
            _name.text = CharInfoMockData.Name;
        }

        private void SetLevel()
        {
            _level.text = string.Format(_level.text, CharInfoMockData.Level);
        }

        private void SetHP()
        {
            float curHP = CharInfoMockData.CurrentHP;
            float maxHP = CharInfoMockData.MaxHP;

            _currentHP.text = ((int)curHP).ToString();
            _maxHP.text = ((int)maxHP).ToString();

            _HPBar.fillAmount = curHP / maxHP;
        }

        private void SetMP()
        {
            float curMP = CharInfoMockData.CurrentMP;
            float maxMP = CharInfoMockData.MaxMP;

            _currentMP.text = ((int)curMP).ToString();
            _maxMP.text = ((int)maxMP).ToString();

            _MPBar.fillAmount = curMP / maxMP;
        }

        private void SetEXP()
        {
            float curEXP = CharInfoMockData.CurrentEXP;
            float maxEXP = CharInfoMockData.MaxEXP;

            _EXPBar.fillAmount = curEXP / maxEXP;
        }

        private void SetClass()
        {
            _class.text = CharInfoMockData.Class;
        }

        private void SetElemental()
        {
            _elemental.sprite = CharInfoMockData.Elemental.Icon;
        }

        private void SetAvatar()
        {
            _avatar.sprite = CharInfoMockData.Avatar;
        }
    }
}
