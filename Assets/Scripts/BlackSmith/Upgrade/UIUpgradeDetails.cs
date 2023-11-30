using System;
using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIUpgradeDetails : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Color _validColor;
        [SerializeField] private Color _inValidColor;
        private int _selectLevelToUpgrade;
        private int _currentLevel;
        private bool _isValid;
        private float _currentGold;

        public int LevelToUpgrade => _selectLevelToUpgrade;

        public float GoldNeeded { get ; private set; }

        public void SetupUI(IUpgradeEquipment item, float currentGold)
        {
            _currentGold = currentGold;
            InitEquipmentInfo(item);
        }

        private void InitEquipmentInfo(IUpgradeEquipment item)
        {
            _selectLevelToUpgrade = item.Level + 1;
            _currentLevel = _selectLevelToUpgrade;
            _levelText.text = _currentLevel.ToString();
            SetupCostUI(item);
        }

        public void UpdateValue(int direction, IUpgradeEquipment item)
        {
            _currentLevel += direction;

            if (_currentLevel > item.Level && _currentLevel <= item.Equipment.Data.MaxLevel)
            {
                _selectLevelToUpgrade = _currentLevel;
            }
            else
            {
                _currentLevel = _selectLevelToUpgrade;
            }

            _levelText.text = _selectLevelToUpgrade.ToString();
            SetupCostUI(item);
        }

        private void SetupCostUI(IUpgradeEquipment item)
        {
            var levelToUpgrade = _currentLevel - item.Level;
            GoldNeeded = levelToUpgrade * item.Cost;

            _isValid = _currentGold >= GoldNeeded;
            _costText.color = _isValid ? _validColor : _inValidColor;

            _costText.text = $"{GoldNeeded} G";
        }
    }
}