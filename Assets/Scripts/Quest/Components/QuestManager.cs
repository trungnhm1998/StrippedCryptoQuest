using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.Events;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using CryptoQuest.System;
using IndiGames.Core.SaveSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Components
{
    [Serializable]
    public class QuestData: IJsonSerializable
    {
        public List<string> InProgressQuest = new();
        public List<string> CompletedQuests = new();

        public bool FromJson(string json)
        {
            try
            {
                JsonUtility.FromJsonOverwrite(json, this);
                return true;
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
            }
            return false;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour, ISaveObject
    {
        public static Action<IQuestConfigure> OnConfigureQuest;
        public static Action<QuestSO> OnRemoveProgressingQuest;
        public Action<QuestSO> OnQuestCompleted;

        [Header("Quest Events")] [SerializeField]
        private QuestEventChannelSO _triggerQuestEventChannel;

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        [SerializeField] private RewardLootEvent _rewardEventChannel;

        [field: SerializeReference, HideInInspector]
        public List<QuestInfo> InProgressQuest { get; private set; } = new();

        [field: SerializeReference, HideInInspector]
        public List<QuestInfo> CompletedQuests { get; private set; } = new();

        [SerializeField, HideInInspector] private QuestSO _currentQuestData;
        private ISaveSystem _saveSystem;

        private void Awake()
        {
            ServiceProvider.Provide(this);
            _saveSystem = ServiceProvider.GetService<ISaveSystem>();
        }

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

        private void Start()
        {
            _saveSystem?.LoadObject(this);
        }

        public void TriggerQuest(QuestSO questData)
        {
            if (IsQuestTriggered(questData))
            {
                Debug.Log("TriggerQuest(), Already triggered: " + questData.QuestName);
                return;
            }
            foreach (var progressQuestInfo in InProgressQuest)
            {
                if (progressQuestInfo.Guid != questData.Guid) continue;

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

        public void GiveQuest(QuestSO questData)
        {
            // Find and remove completed quest
            // This allow parent-quest to respawn child-quest when it not finished
            // while child(s) may finished already (eg: CompletedSlimeQuest)
            var questIdx = CompletedQuests.FindIndex(quest => quest.Guid == questData.Guid);
            if(questIdx != -1)
            {
                CompletedQuests.RemoveAt(questIdx);
            }

            if (IsQuestTriggered(questData))
            {
                Debug.Log("GiveQuest(), Already triggered: " + questData.QuestName);
                return;
            }

            if (InProgressQuest.Any(questInfo => questInfo.Guid == questData.Guid))
            {
                Debug.Log("GiveQuest(), Already inprogress: " + questData.QuestName);
                return;
            }

            QuestInfo currentQuestInfo = questData.CreateQuest();

            if (!IsQuestCompleted(questData))
            {
                InProgressQuest.Add(currentQuestInfo);
                currentQuestInfo.GiveQuest();
            }
            else
            {
                Debug.Log("GiveQuest(), Already completed: " + questData.QuestName);
            }

            _currentQuestData = questData;
            questData.OnRewardReceived += RewardReceived;

            _saveSystem?.SaveObject(this);
        }


        private void RewardReceived(List<LootInfo> loots)
        {
            _rewardEventChannel.EventRaised(loots);
            _currentQuestData.OnRewardReceived -= RewardReceived;

            _saveSystem?.SaveObject(this);
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

            _saveSystem?.SaveObject(this);
        }

        private bool IsQuestTriggered(QuestSO questSo)
        {
            return CompletedQuests.Any(quest => quest.Guid == questSo.Guid);
        }

        private void ConfigureQuestHolder(IQuestConfigure questConfigure)
        {
            questConfigure.QuestsToTrack.ForEach(q => questConfigure.Configure(IsQuestTriggered(q)));
        }

        private void RemoveProgressingQuest(QuestSO quest)
        {
            foreach (var inProgressQuest in InProgressQuest.ToList())
            {
                if (inProgressQuest.Guid != quest.Guid) continue;
                InProgressQuest.Remove(inProgressQuest);
            }

            _saveSystem?.SaveObject(this);
        }

        #region SaveSystem

        public string Key => "Quest";

        public string ToJson()
        {
            var questData = new QuestData();
            foreach (var item in InProgressQuest)
            {
                questData.InProgressQuest.Add(item.Guid);
            }
            foreach (var item in CompletedQuests)
            {
                questData.CompletedQuests.Add(item.Guid);
            }
            return questData.ToJson();
        }

        public bool FromJson(string json)
        {
            StartCoroutine(CoFromJson(json));
            return true;
        }

        public IEnumerator CoFromJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var questData = new QuestData();
                JsonUtility.FromJsonOverwrite(json, questData);
                if (questData.InProgressQuest.Count() > 0 || questData.CompletedQuests.Count() > 0)
                {
                    CompletedQuests.Clear();
                    InProgressQuest.Clear();

                    foreach (var guid in questData.CompletedQuests)
                    {
                        var questSoHandle = Addressables.LoadAssetAsync<QuestSO>(guid);
                        {
                            yield return questSoHandle;
                            if (questSoHandle.Status == AsyncOperationStatus.Succeeded)
                            {
                                var questSO = questSoHandle.Result;
                                GiveQuest(questSO);
                                QuestCompleted(questSO);
                            }
                        }
                    }

                    foreach (var guid in questData.InProgressQuest)
                    {
                        var questSoHandle = Addressables.LoadAssetAsync<QuestSO>(guid);
                        {
                            yield return questSoHandle;
                            if (questSoHandle.Status == AsyncOperationStatus.Succeeded)
                            {
                                var questSO = questSoHandle.Result;
                                GiveQuest(questSO);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}