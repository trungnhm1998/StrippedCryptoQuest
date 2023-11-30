using UnityEngine;
using CryptoQuest.UI.Tooltips;
using UnityEngine.Localization;
using UnityEngine.UI;
using TMPro;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIPreviewUpgradeLevel : MonoBehaviour
    {
        [SerializeField] private Color _increaseColor;
        [SerializeField] private UIUpgradeEquipmentTooltip _upgradeUI;
        [SerializeField] private TMP_Text _levelText;

        private Color _originColor;
        private float _currentLevel;
        private float _currentMaxLevel;

        private void Awake()
        {
            _originColor = _levelText.color;
        }

        private void OnEnable()
        {
            _levelText.color = _originColor;
        }

        public void SetCurrentLevel(int level, int maxLevel)
        {
            _currentLevel = level;
            _currentMaxLevel = maxLevel;
            _levelText.text = string.Format(_upgradeUI.LevelText, level, maxLevel);
        }

        public void ResetPreviewUI()
        {
            _levelText.color = _originColor;
            _levelText.text = string.Format(_upgradeUI.LevelText, _currentLevel, _currentMaxLevel);
        }

        public void SetPreviewLevel(int level, int maxLevel)
        {
            _levelText.color = _increaseColor;
            _levelText.text = string.Format(_upgradeUI.LevelText, level, maxLevel);
        }
    }
}