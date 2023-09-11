using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class UIRewardItem : MonoBehaviour
    {
        public event Action<string> StringChanged;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private LocalizeStringEvent _rewardContent;
        public LocalizeStringEvent RewardContent => _rewardContent;
        public void OnStringChanged(string value) => StringChanged?.Invoke(value);
        public void SetText(string text) => _rewardText.text = text;
    }
}