using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestCompletionHandler : MonoBehaviour
    {
        public void HandleNextAction(QuestSO data)
        {
            if (data.NextAction == null) return;
            StartCoroutine(data.NextAction.Execute());
        }

        public void GrantCompletionRewards(QuestSO questSo)
        {
            if (questSo.Rewards.Length <= 0) return;
            questSo.OnRewardReceived?.Invoke(GetRewards(questSo.Rewards));
        }

        private List<LootInfo> GetRewards(QuestReward[] rewards)
        {
            return rewards.Select(reward => reward.CreateReward()).ToList();
        }
    }
}