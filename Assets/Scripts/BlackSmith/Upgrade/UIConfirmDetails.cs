using System;
using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIConfirmDetails : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private UIUpgradeDetails _upgradeDetailsUI;
        [SerializeField] private string _costTextFormat = "{0} G";

        private void OnEnable()
        {
            SetupUI(_upgradeDetailsUI.LevelToUpgrade, _upgradeDetailsUI.GoldNeeded);
        }

        private void SetupUI(int level, float currentGold)
        {
            _levelText.text = level.ToString();
            _costText.text = string.Format(_costTextFormat, currentGold);
        }
    }
}