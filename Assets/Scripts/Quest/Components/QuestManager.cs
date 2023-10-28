using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.Events;
using CryptoQuest.Quest.Actions;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Events;
using CryptoQuest.System;
using IndiGames.Core.SaveSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Components
{
    [Serializable]
    public class QuestData : IJsonSerializable
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
            catch (Exception ex)
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

    public abstract class IQuestManager : SaveObject
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

        private void Awake()
        {
            ServiceProvider.Provide<IQuestManager>(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnConfigureQuest += ConfigureQuestHolder;
            OnRemoveProgressingQuest += RemoveProgressingQuest;
            OnQuestCompleted += QuestCompleted;

            _triggerQuestEventChannel.EventRaised += TriggerQuest;
            _giveQuestEventChannel.EventRaised += GiveQuest;
            _removeQuestEventChannel.EventRaised += RemoveProgressingQuest;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
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

            SaveSystem?.SaveObject(this);
        }

        private void RewardReceived(List<LootInfo> loots)
        {
            _rewardEventChannel.EventRaised(loots);
            _currentQuestData.OnRewardReceived -= RewardReceived;

            SaveSystem?.SaveObject(this);
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

            SaveSystem?.SaveObject(this);
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
            yield return WaitUntilTrue(IsLoaded);
            questConfigure.QuestsToTrack.ForEach(questData => questConfigure.Configure(IsQuestTriggered(questData)));
        }

        private void RemoveProgressingQuest(QuestSO quest)
        {
            foreach (var inProgressQuest in InProgressQuest.ToList())
            {
                if (inProgressQuest.Guid != quest.Guid) continue;
                InProgressQuest.Remove(inProgressQuest);
            }

            SaveSystem?.SaveObject(this);
        }

        #region SaveSystem

        public override string Key => "Quest";

        public override string ToJson()
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

        public override IEnumerator CoFromJson(string json)
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
                        yield return questSoHandle;
                        if (questSoHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            GiveQuest(questSoHandle.Result);
                            OnQuestCompleted(questSoHandle.Result);
                        }
                    }

                    foreach (var guid in questData.InProgressQuest)
                    {
                        var questSoHandle = Addressables.LoadAssetAsync<QuestSO>(guid);
                        yield return questSoHandle;
                        if (questSoHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            GiveQuest(questSoHandle.Result);
                        }
                    }

                    var lastCutsceneActionQuestIdx = CompletedQuests.FindIndex(quest => quest.BaseData.GetType() == typeof(CutsceneBranchingQuestSO) && quest.BaseData.NextAction != null && quest.BaseData.NextAction.GetType() ==  typeof(CutsceneAction));
                    var lastCutsceneBattleQuestIdx = CompletedQuests.FindIndex(quest => quest.BaseData.GetType() == typeof(CutsceneBranchingQuestSO) && quest.BaseData.NextAction != null && quest.BaseData.NextAction.GetType() ==  typeof(BattleAction));
                    var inprogressCutsceneBattleQuestIdx = InProgressQuest.FindIndex(quest => quest.BaseData.GetType() == typeof(CutsceneBranchingQuestSO) && quest.BaseData.NextAction != null && quest.BaseData.NextAction.GetType() ==  typeof(BattleAction));

                    // If has CutsceneAction but BattleAction inprogress, retrigger CutsceneAction
                    if(lastCutsceneActionQuestIdx > lastCutsceneBattleQuestIdx && inprogressCutsceneBattleQuestIdx > -1)
                    {
                        var cutsceneQuest = CompletedQuests.ElementAt(lastCutsceneActionQuestIdx);
                        CompletedQuests.RemoveAt(lastCutsceneActionQuestIdx);
                        InProgressQuest.RemoveAt(inprogressCutsceneBattleQuestIdx);
                        GiveQuest(cutsceneQuest.BaseData);
                    }

                    var lastBattleQuestIdx = CompletedQuests.FindIndex(quest => quest.BaseData.GetType() == typeof(BattleQuestSO));
                    var inprogressBattleQuestIdx = InProgressQuest.FindIndex(quest => quest.BaseData.GetType() == typeof(BattleQuestSO));

                    // If has battle inprogress, retrigger CutsceneAction
                    if (lastCutsceneActionQuestIdx < lastCutsceneBattleQuestIdx && lastCutsceneBattleQuestIdx > lastBattleQuestIdx)
                    {
                        var cutsceneQuest = CompletedQuests.ElementAt(lastCutsceneActionQuestIdx);
                        CompletedQuests.RemoveAt(lastCutsceneBattleQuestIdx);
                        CompletedQuests.RemoveAt(lastCutsceneActionQuestIdx);
                        if (inprogressBattleQuestIdx >= 0) InProgressQuest.RemoveAt(inprogressBattleQuestIdx);
                        GiveQuest(cutsceneQuest.BaseData);
                    }
                }
            }
        }
        #endregion
    }
}