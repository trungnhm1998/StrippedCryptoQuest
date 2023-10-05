using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.ScriptableObjects;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour
    {
        public static Action<IQuestConfigure> OnConfigureQuest;

        [Header("Listening Channels")]
        [SerializeField] private VoidEventChannelSO _onSceneLoadedChannel;

        [Header("Quest Events")]
        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannel;

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        [SerializeField] private RewardSO _rewardEventChannel;

        [field: SerializeReference] public List<QuestInfo> InProgressQuest { get; set; }
        [field: SerializeReference] public List<QuestInfo> CompletedQuests { get; set; }

        private QuestSO _currentQuestData;
        private QuestInfo _currentQuestInfo;

        private void OnEnable()
        {
            // This is a temp fix, because we have one quest on beginning
            // So I don't want to create new for this 
            // ehehe (*/ω＼*)
            InProgressQuest ??= new();
            CompletedQuests ??= new();

            _onSceneLoadedChannel.EventRaised += LoadingFirstQuest;

            OnConfigureQuest += ConfigureQuestHolder;
            _triggerQuestEventChannel.EventRaised += TriggerQuest;
            _giveQuestEventChannel.EventRaised += GiveQuest;
        }

        private void OnDisable()
        {
            _onSceneLoadedChannel.EventRaised -= LoadingFirstQuest;

            OnConfigureQuest -= ConfigureQuestHolder;
            _triggerQuestEventChannel.EventRaised -= TriggerQuest;
            _giveQuestEventChannel.EventRaised -= GiveQuest;
        }

        private void LoadingFirstQuest()
        {
            if (InProgressQuest == null) return;

            QuestSO firstQuestData = InProgressQuest[0].BaseData;
            GiveQuest(firstQuestData);
        }

        private void TriggerQuest(QuestSO questData)
        {
            foreach (var progressQuestInfo in InProgressQuest)
            {
                if (progressQuestInfo.BaseData != questData) continue;

                progressQuestInfo.TriggerQuest();
                _currentQuestInfo = progressQuestInfo;
            }

            _currentQuestData.OnRewardReceived += RewardReceived;
            _currentQuestData.OnQuestCompleted += QuestCompleted;
        }

        private void GiveQuest(QuestSO questData)
        {
            QuestInfo currentQuestInfo = questData.CreateQuest(this);

            if (!InProgressQuest.Contains(currentQuestInfo))
            {
                InProgressQuest.Add(currentQuestInfo);
            }

            _currentQuestData = questData;

            currentQuestInfo.GiveQuest();
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