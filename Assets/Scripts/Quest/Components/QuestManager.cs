using System;
using System.Collections.Generic;
using System.Linq;
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
        public static Action<QuestSO> OnRemoveProgressingQuest;

        [Header("Quest Events")] [SerializeField]
        private QuestEventChannelSO _triggerQuestEventChannel;

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        [SerializeField] private RewardSO _rewardEventChannel;

        [field: SerializeReference, HideInInspector]
        public List<QuestInfo> InProgressQuest { get; private set; } = new();

        [field: SerializeReference, HideInInspector]
        public List<QuestInfo> CompletedQuests { get; private set; } = new();

        private List<string> _completedQuestsId = new();

        private QuestSO _currentQuestData;
        private QuestInfo _currentQuestInfo;

        private void OnEnable()
        {
            OnConfigureQuest += ConfigureQuestHolder;
            _triggerQuestEventChannel.EventRaised += TriggerQuest;
            _giveQuestEventChannel.EventRaised += GiveQuest;
            OnRemoveProgressingQuest += RemoveProgressingQuest;
        }

        private void OnDisable()
        {
            OnConfigureQuest -= ConfigureQuestHolder;
            _triggerQuestEventChannel.EventRaised -= TriggerQuest;
            _giveQuestEventChannel.EventRaised -= GiveQuest;
            OnRemoveProgressingQuest -= RemoveProgressingQuest;
        }

        public void TriggerQuest(QuestSO questData)
        {
            foreach (var progressQuestInfo in InProgressQuest)
            {
                if (progressQuestInfo.BaseData != questData) continue;

                _currentQuestInfo = progressQuestInfo;
                break;
            }

            _currentQuestInfo.TriggerQuest();
        }

        public void GiveQuest(QuestSO questData)
        {
            if (IsQuestTriggered(questData)) return;
            QuestInfo currentQuestInfo = questData.CreateQuest(this);

            if (!_completedQuestsId.Contains(questData.Guid))
            {
                InProgressQuest.Add(currentQuestInfo);
                currentQuestInfo.GiveQuest();
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
            _completedQuestsId.Add(_currentQuestInfo.BaseData.Guid);

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