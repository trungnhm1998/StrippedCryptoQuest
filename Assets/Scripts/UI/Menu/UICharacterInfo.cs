using System.Collections;
using System.Collections.Generic;
using CryptoQuest.MockData;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UICharacterInfo : MonoBehaviour
    {
        [Header("UI Components")]
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
            InitCharacterInfo();
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
            _name.text = $"{CharInfoMockData.Name}";
        }

        private void SetLevel()
        {
            _level.text = $"<size=20%>Lv</size>{CharInfoMockData.Level}";
        }

        private void SetHP()
        {
            int curHP = CharInfoMockData.CurrentHP;
            int maxHP = CharInfoMockData.MaxHP;

            _currentHP.text = $"{curHP}";
            _maxHP.text = $"{maxHP}";

            _HPBar.fillAmount = (float)curHP / (float)maxHP;
        }

        private void SetMP()
        {
            int curMP = CharInfoMockData.CurrentMP;
            int maxMP = CharInfoMockData.MaxMP;

            _currentMP.text = $"{curMP}";
            _maxMP.text = $"{maxMP}";

            _MPBar.fillAmount = (float)curMP / (float)maxMP;
        }

        private void SetEXP()
        {
            int curEXP = CharInfoMockData.CurrentEXP;
            int maxEXP = CharInfoMockData.MaxEXP;

            _EXPBar.fillAmount = (float)curEXP / (float)maxEXP;
        }

        private void SetClass()
        {
            _class.text = $"{CharInfoMockData.Class}";
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
