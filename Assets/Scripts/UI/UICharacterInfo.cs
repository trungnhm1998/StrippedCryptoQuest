using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest
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
        [SerializeField] private CharInfoMockDataSO _charInfoMockData;

        private void Awake() {
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
            _name.text = $"{_charInfoMockData.Name}";
        }

        private void SetLevel()
        {
            _level.text = $"<size=20%>Lv</size>{_charInfoMockData.Level}";
        }

        private void SetHP()
        {
            int curHP = _charInfoMockData.CurrentHP;
            int maxHP = _charInfoMockData.MaxHP;

            _currentHP.text = $"{curHP}";
            _maxHP.text = $"{maxHP}";

            _MPBar.fillAmount = (float)curHP / (float)maxHP;
        }

        private void SetMP()
        {
            int curMP = _charInfoMockData.CurrentMP;
            int maxMP = _charInfoMockData.MaxMP;

            _currentMP.text = $"{curMP}";
            _maxMP.text = $"{maxMP}";

            _MPBar.fillAmount = (float)curMP / (float)maxMP;
        }

        private void SetEXP()
        {
            int curEXP = _charInfoMockData.CurrentEXP;
            int maxEXP = _charInfoMockData.MaxEXP;

            _EXPBar.fillAmount = (float)curEXP / (float)maxEXP;
        }

        private void SetClass()
        {
            _class.text = $"{_charInfoMockData.Class}";
        }

        private void SetElemental()
        {
            _elemental.sprite = _charInfoMockData.Elemental.Icon;
        }

        private void SetAvatar()
        {
            _avatar.sprite = _charInfoMockData.Avatar;
        }
    }
}
