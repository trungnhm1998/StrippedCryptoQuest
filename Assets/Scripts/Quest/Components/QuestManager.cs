using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.Events;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Common;
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
        [SerializeField, HideInInspector] private List<QuestInfo> _currentQuestInfos = new();
        [NonSerialized] private Dictionary<string, QuestInfo> _questInfoDict = new();
        private bool _isCacheDirty;

        private Dictionary<string, QuestInfo> QuestInfoLookup
        {
            get
            {
                if (!_isCacheDirty) return _questInfoDict;
                _isCacheDirty = false;
                _questInfoDict.Clear();
                foreach (var data in _currentQuestInfos)
                {
                    _questInfoDict.Add(data.Guid, data);
                }

                return _questInfoDict;
            }
        }

        private void Awake()
        {
            ServiceProvider.Provide<IQuestManager>(this);
        }

        private void MarkCacheDirty()
        {
            _isCacheDirty = true;
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

        private void OnDestroy()
        {
            foreach (var info in _currentQuestInfos)
            {
                info.Release();
            }
        }

        public override void TriggerQuest(QuestSO questData)
        {
            if (IsQuestTriggered(questData))
            {
                Debug.Log($"<color=green>QuestManager::TriggerQuest::Already triggered: {questData.QuestName}</color>");
                return;
            }

            if (QuestInfoLookup.TryGetValue(questData.Guid, out var questInfo))
            {
                Debug.Log($"<color=green>QuestManager::TriggerQuest::Triggered: {questData.QuestName}</color>");
                questInfo.TriggerQuest();
                questInfo.Release();
                _currentQuestInfos.Remove(questInfo);
                MarkCacheDirty();
            }
            else
            {
                Debug.Log(
                    $"<color=red>QuestManager::TriggerQuest::Triggered:QuestInfoIsNotCurrentQuest {questData.QuestName}</color>");
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
                if (_currentQuestInfos.Any(questInfo => questInfo.Guid == questData.Guid)) return;

                var info = questData.CreateQuest();
                _currentQuestInfos.Add(info);
                MarkCacheDirty();
                info.GiveQuest();

                return;
            }

            if (!IsQuestCompleted(questData))
            {
                Debug.Log($"<color=green>QuestManager::GiveQuest::Give: {questData.QuestName}</color>");
                var info = questData.CreateQuest();
                _currentQuestInfos.Add(info);
                MarkCacheDirty();
                info.GiveQuest();
                _saveData.AddInProgressQuest(info.Guid);
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
            _rewardEventChannel.RaiseEvent(loots);
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

        private bool IsCurrentlyPlayingQuest(QuestSO quest)
        {
            return _currentQuestInfos.Any(questInfo => questInfo.Guid == quest.Guid);
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
            foreach (var questInfo in _currentQuestInfos.ToList())
            {
                if (questInfo.Guid != quest.Guid) continue;
                _currentQuestInfos.Remove(questInfo);
            }

            MarkCacheDirty();

            foreach (var inProgressQuest in _saveData.InProgressQuest.ToList())
            {
                if (inProgressQuest != quest.Guid) continue;
                _saveData.RemoveInProgressQuest(inProgressQuest);
            }
        }
    }
}