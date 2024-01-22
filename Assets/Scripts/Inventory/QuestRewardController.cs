using System.Collections.Generic;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.Events;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class QuestRewardController : MonoBehaviour
    {
        [Header("Listen Event")]
        [SerializeField] private RewardLootEvent _rewardLootEvent;

        [Header("Raise Event")]
        [SerializeField] private RewardDialogEventChannelSO _showRewardDialogEventChannel;

        private void Awake()
        {
            _rewardLootEvent.EventRaised += Reward;
        }

        private void OnDestroy() => _rewardLootEvent.EventRaised -= Reward;

        private void Reward(List<LootInfo> loots)
        {
            if (loots == null || loots.Count == 0) return;
            _showRewardDialogEventChannel.Show(new RewardDialogData(loots));
        }
    }
}