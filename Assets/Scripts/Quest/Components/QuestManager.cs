using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.Events;
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
        public static Action<QuestSO> OnRemoveProgressingQuest;
        public static Action<QuestSO> OnQuestCompleted;

        [Header("Quest Events")] [SerializeField]
        private QuestEventChannelSO _triggerQuestEventChannel;

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        [SerializeField] private RewardLootEvent _rewardEventChannel;

        [field: SerializeReference, HideInInspector]
        public List<QuestInfo> InProgressQuest { get; private set; } = new();

        [field: SerializeReference, HideInInspector]
        public List<QuestInfo> CompletedQuests { get; private set; } = new();

        private readonly List<string> _completedQuestsId = new();

        private QuestSO _currentQuestData;
        private QuestInfo _currentQuestInfo;

        private void OnEnable()
        {
            OnConfigureQuest += ConfigureQuestHolder;
            OnRemoveProgressingQuest += RemoveProgressingQuest;
            OnQuestCompleted += QuestCompleted;

            _triggerQuestEventChannel.EventRaised += TriggerQuest;
            _giveQuestEventChannel.EventRaised += GiveQuest;
        }

        private void OnDisable()
        {
            OnConfigureQuest -= ConfigureQuestHolder;
            OnRemoveProgressingQuest -= RemoveProgressingQuest;
            OnQuestCompleted -= QuestCompleted;

            _triggerQuestEventChannel.EventRaised -= TriggerQuest;
            _giveQuestEventChannel.EventRaised -= GiveQuest;
        }

        public void TriggerQuest(QuestSO questData)
        {
            if (IsQuestTriggered(questData)) return;

            foreach (var progressQuestInfo in InProgressQuest)
            {
                if (progressQuestInfo.BaseData != questData) continue;

                progressQuestInfo.TriggerQuest();
                _currentQuestInfo = progressQuestInfo;
                break;
            }
        }

        public void GiveQuest(QuestSO questData)
        {
            if (IsQuestTriggered(questData)) return;

            foreach (var progressingQuest in InProgressQuest)
                if (progressingQuest.BaseData == questData)
                    return;

            QuestInfo currentQuestInfo = questData.CreateQuest(this);

            if (!_completedQuestsId.Contains(questData.Guid))
            {
                InProgressQuest.Add(currentQuestInfo);
                currentQuestInfo.GiveQuest();
            }

            _currentQuestData = questData;
            questData.OnRewardReceived += RewardReceived;
        }


        private void RewardReceived(List<LootInfo> loots)
        {
            _rewardEventChannel.EventRaised(loots);
            _currentQuestData.OnRewardReceived -= RewardReceived;
        }

        private void UpdateQuestProgress(QuestInfo questInfo)
        {
            InProgressQuest.Remove(questInfo);

            CompletedQuests.Add(questInfo);
            _completedQuestsId.Add(questInfo.BaseData.Guid);
        }

        private void QuestCompleted(QuestSO questSo)
        {
            foreach (var progressQuestInfo in InProgressQuest)
            {
                if (progressQuestInfo.BaseData != questSo) continue;
                UpdateQuestProgress(progressQuestInfo);
                break;
            }
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
            questConfigure.Configure(IsQuestTriggered(questConfigure.QuestToTrack));
        }

        private void RemoveProgressingQuest(QuestSO quest)
        {
            foreach (var inProgressQuest in InProgressQuest.ToList())
            {
                if (inProgressQuest.BaseData != quest) continue;
                InProgressQuest.Remove(inProgressQuest);
            }
        }
    }
}