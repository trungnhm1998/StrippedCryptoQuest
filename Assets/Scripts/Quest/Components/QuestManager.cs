using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.ScriptableObjects;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour
    {
        public static Action<IQuestConfigure> OnConfigureQuest;

        [Header("Quest Events")]
        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannel;

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        [SerializeField] private RewardSO _rewardEventChannel;

        [field: Header("Quest Data")]
        [field: SerializeReference] public List<QuestInfo> InProgressQuest { get; set; } = new();

        [field: SerializeReference] public List<QuestInfo> CompletedQuests { get; set; } = new();

        private QuestSO _currentQuestData;
        private QuestInfo _currentQuestInfo;

        private void OnEnable()
        {
            OnConfigureQuest += ConfigureQuestHolder;
            _triggerQuestEventChannel.EventRaised += TriggerQuest;
            _giveQuestEventChannel.EventRaised += GiveQuest;
        }

        private void OnDisable()
        {
            OnConfigureQuest -= ConfigureQuestHolder;
            _triggerQuestEventChannel.EventRaised -= TriggerQuest;
            _giveQuestEventChannel.EventRaised -= GiveQuest;
        }

        public void GiveQuest(QuestSO questData)
        {
            QuestInfo currentQuestInfo = questData.CreateQuest(this);
            InProgressQuest.Add(currentQuestInfo);

            currentQuestInfo.GiveQuest();
        }

        public void TriggerQuest(QuestSO questData)
        {
            foreach (var progressQuestInfo in InProgressQuest)
            {
                if (progressQuestInfo.BaseData != questData) continue;

                _currentQuestInfo = progressQuestInfo;
                progressQuestInfo.TriggerQuest();
            }

            _currentQuestData = questData;
            questData.OnQuestCompleted += QuestCompleted;
            questData.OnRewardReceived += RewardReceived;
        }


        private void RewardReceived(LootInfo[] loots)
        {
            _rewardEventChannel.RewardRaiseEvent(loots);
            _currentQuestData.OnRewardReceived -= RewardReceived;
        }

        private void QuestCompleted()
        {
            InProgressQuest.Remove(_currentQuestInfo);

            CompletedQuests.Add(_currentQuestInfo);

            _currentQuestData.OnQuestCompleted -= QuestCompleted;
        }

        private bool IsQuestTriggered(QuestSO questSo)
        {
            foreach (var quest in CompletedQuests)
            {
                if (quest.BaseData == questSo)
                    return true;
            }

            return false;
        }

        private void ConfigureQuestHolder(IQuestConfigure questConfigure)
        {
            questConfigure.IsQuestCompleted = IsQuestTriggered(questConfigure.Quest);
            questConfigure.Configure();
        }
    }
}