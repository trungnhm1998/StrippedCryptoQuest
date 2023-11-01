using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.Events;
using CryptoQuest.Quest.Actions;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Events;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem.Actions;
using IndiGames.Core.SaveSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        [SerializeField] private QuestEventChannelSO _removeQuestEventChannel;
        [SerializeField] private RewardLootEvent _rewardEventChannel;

        [field: SerializeReference, HideInInspector]
        public List<QuestInfo> InProgressQuest { get; private set; } = new();

        [field: SerializeReference, HideInInspector]
        public List<QuestInfo> CompletedQuests { get; private set; } = new();

        [SerializeField, HideInInspector] private QuestSO _currentQuestData;

        private bool _questLoaded = false;
        private TinyMessenger.TinyMessageSubscriptionToken _listenToLoadCompletedEventToken;

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

        private void Start()
        {
            _questLoaded = false;
            _listenToLoadCompletedEventToken = ActionDispatcher.Bind<LoadQuestCompletedAction>(_ => LoadQuest());
            ActionDispatcher.Dispatch(new LoadQuestAction(this));
        }

        private void LoadQuest()
        {
            ActionDispatcher.Unbind(_listenToLoadCompletedEventToken);
            _questLoaded = true;
        }    

        public override void TriggerQuest(QuestSO questData)
        {
            if (IsQuestTriggered(questData))
            {
                Debug.Log($"<color=green>QuestManager::TriggerQuest::Already triggered: {questData.QuestName}</color>");
                return;
            }

            foreach (var progressQuestInfo in InProgressQuest)
            {
                if (progressQuestInfo.Guid != questData.Guid) continue;

                Debug.Log($"<color=green>QuestManager::TriggerQuest::Triggered: {questData.QuestName}</color>");
                progressQuestInfo.TriggerQuest();
                break;
            }
        }

        private bool IsQuestCompleted(QuestSO questData)
        {
            if (questData != null && CompletedQuests.Count() > 0)
            {
                return CompletedQuests.Any(questInfo => questData.Guid == questInfo.Guid);
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

            if (InProgressQuest.Any(questInfo => questInfo.Guid == questData.Guid))
            {
                Debug.Log($"<color=green>QuestManager::GiveQuest::Already inprogress: {questData.QuestName}</color>");
                return;
            }

            QuestInfo currentQuestInfo = questData.CreateQuest();

            if (!IsQuestCompleted(questData))
            {
                Debug.Log($"<color=green>QuestManager::GiveQuest::Give: {questData.QuestName}</color>");
                InProgressQuest.Add(currentQuestInfo);
                currentQuestInfo.GiveQuest();
            }
            else
            {
                Debug.Log($"<color=green>QuestManager::GiveQuest::Already completed: {questData.QuestName}</color>");
            }

            _currentQuestData = questData;
            questData.OnRewardReceived += RewardReceived;
            ActionDispatcher.Dispatch(new SaveQuestAction(this));
        }

        private void RewardReceived(List<LootInfo> loots)
        {
            _rewardEventChannel.EventRaised(loots);
            _currentQuestData.OnRewardReceived -= RewardReceived;
            ActionDispatcher.Dispatch(new SaveQuestAction(this));
        }

        private void UpdateQuestProgress(QuestInfo questInfo)
        {
            InProgressQuest.Remove(questInfo);
            CompletedQuests.Add(questInfo);
        }

        private void QuestCompleted(QuestSO questSo)
        {
            foreach (var progressQuestInfo in InProgressQuest)
            {
                if (progressQuestInfo.Guid != questSo.Guid) continue;
                UpdateQuestProgress(progressQuestInfo);
                break;
            }
            ActionDispatcher.Dispatch(new SaveQuestAction(this));
        }

        private bool IsQuestTriggered(QuestSO questSo)
        {
            return CompletedQuests.Any(quest => quest.Guid == questSo.Guid);
        }

        private void ConfigureQuestHolder(IQuestConfigure questConfigure)
        {
            StartCoroutine(CoConfigureQuestHolder(questConfigure));
        }

        private IEnumerator CoConfigureQuestHolder(IQuestConfigure questConfigure)
        {
            yield return new WaitUntil(() => _questLoaded);
            questConfigure.QuestsToTrack.ForEach(questData => questConfigure.Configure(IsQuestTriggered(questData)));
        }

        private void RemoveProgressingQuest(QuestSO quest)
        {
            foreach (var inProgressQuest in InProgressQuest.ToList())
            {
                if (inProgressQuest.Guid != quest.Guid) continue;
                InProgressQuest.Remove(inProgressQuest);
            }
            ActionDispatcher.Dispatch(new SaveQuestAction(this));
        }
    }
}