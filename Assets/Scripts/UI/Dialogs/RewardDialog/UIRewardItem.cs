using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class UIRewardItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private LocalizeStringEvent _rewardContent;
        public LocalizeStringEvent RewardContent => _rewardContent;
        public void SetText(string text) => _rewardText.text = text;
    }
}