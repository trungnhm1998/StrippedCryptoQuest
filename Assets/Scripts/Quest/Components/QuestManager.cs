using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.Events;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public abstract class IQuestManager : MonoBehaviour
    {
        public static Action<IQuestConfigure> OnConfigureQuest;
        public static Action<QuestSO> OnRemoveProgressingQuest;
        public Action<QuestSO> OnQuestCompleted;

        public abstract void TriggerQuest(QuestSO questData);
        public abstract void GiveQuest(QuestSO questData);
    }

    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : IQuestManager
    {
        [Header("Quest Events")]
        [SerializeField]
        private QuestEventChannelSO _triggerQuestEventChannel;

        [Header("Quest Save Data")]
        [SerializeField]
        private QuestSaveSO _saveData;
        public QuestSaveSO SaveData => _saveData;

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        [SerializeField] private QuestEventChannelSO _removeQuestEventChannel;
        [SerializeField] private RewardLootEvent _rewardEventChannel;
        [SerializeField, HideInInspector] private QuestSO _currentQuestData;
        [SerializeField, HideInInspector] private QuestInfo _currentQuestInfo;

        private void Awake()
        {
            ServiceProvider.Provide<IQuestManager>(this);
        }

        protected void OnEnable()
        {
            OnConfigureQuest += ConfigureQuestHolder;
            OnRemoveProgressingQuest += RemoveProgressingQuest;
            OnQuestCompleted += QuestCompleted;

            _triggerQuestEventChannel.EventRaised += TriggerQuest;
            _giveQuestEventChannel.EventRaised += GiveQuest;
            _removeQuestEventChannel.EventRaised += RemoveProgressingQuest;
        }

        protected void OnDisable()
        {
            OnConfigureQuest -= ConfigureQuestHolder;
            OnRemoveProgressingQuest -= RemoveProgressingQuest;
            OnQuestCompleted -= QuestCompleted;

            _triggerQuestEventChannel.EventRaised -= TriggerQuest;
            _giveQuestEventChannel.EventRaised -= GiveQuest;
            _removeQuestEventChannel.EventRaised -= RemoveProgressingQuest;
        }

        public override void TriggerQuest(QuestSO questData)
        {
            if (IsQuestTriggered(questData))
            {
                Debug.Log($"<color=green>QuestManager::TriggerQuest::Already triggered: {questData.QuestName}</color>");
                return;
            }

            if (_currentQuestInfo.Guid == questData.Guid)
            {
                Debug.Log($"<color=green>QuestManager::TriggerQuest::Triggered: {questData.QuestName}</color>");
                _currentQuestInfo.TriggerQuest();
            }
            else
            {
                Debug.Log($"<color=red>QuestManager::TriggerQuest::Triggered:QuestInfoIsNotCurrentQuest {questData.QuestName}</color>");
            }
        }

        private bool IsQuestCompleted(QuestSO questData)
        {
            if (questData != null && _saveData.CompletedQuests.Count() > 0)
            {
                return _saveData.CompletedQuests.Any(questInfo => questData.Guid == questInfo);
            }

            return false;
        }

        public override void GiveQuest(QuestSO questData)
        {
            if (IsQuestTriggered(questData))
            {
                Debug.Log($"<color=green>QuestManager::GiveQuest::Already triggered: {questData.QuestName}</color>");
                return;
            }

            if (_saveData.InProgressQuest.Any(questInfo => questInfo == questData.Guid))
            {
                Debug.Log($"<color=green>QuestManager::GiveQuest::Already inprogress: {questData.QuestName}</color>");
                _currentQuestInfo = questData.CreateQuest();
                _currentQuestInfo.GiveQuest();
                return;
            }

            if (!IsQuestCompleted(questData))
            {
                Debug.Log($"<color=green>QuestManager::GiveQuest::Give: {questData.QuestName}</color>");
                _currentQuestInfo = questData.CreateQuest();
                _currentQuestInfo.GiveQuest();
                _saveData.AddInProgressQuest(_currentQuestInfo.Guid);
            }
            else
            {
                Debug.Log($"<color=green>QuestManager::GiveQuest::Already completed: {questData.QuestName}</color>");
            }

            _currentQuestData = questData;
            questData.OnRewardReceived += RewardReceived;
        }

        private void RewardReceived(List<LootInfo> loots)
        {
            _rewardEventChannel.EventRaised(loots);
            _currentQuestData.OnRewardReceived -= RewardReceived;
        }

        private void UpdateQuestProgress(string questInfo)
        {
            _saveData.AddCompleteQuest(questInfo);
        }

        private void QuestCompleted(QuestSO questSo)
        {
            foreach (var progressQuestInfo in _saveData.InProgressQuest)
            {
                if (progressQuestInfo != questSo.Guid) continue;
                UpdateQuestProgress(progressQuestInfo);
                break;
            }
        }

        private bool IsQuestTriggered(QuestSO questSo)
        {
            return _saveData.CompletedQuests.Any(quest => quest == questSo.Guid);
        }

        private void ConfigureQuestHolder(IQuestConfigure questConfigure)
        {
            questConfigure.QuestsToTrack.ForEach(questData => questConfigure.Configure(IsQuestTriggered(questData)));
        }

        private void RemoveProgressingQuest(QuestSO quest)
        {
            foreach (var inProgressQuest in _saveData.InProgressQuest.ToList())
            {
                if (inProgressQuest != quest.Guid) continue;
                _saveData.RemoveInProgressQuest(inProgressQuest);
            }
        }
    }
}