using CryptoQuest.Quest.Actions;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Components;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.System.SaveSystem.SaveObjects
{
    public class QuestSaveObject : SaveObjectBase<QuestManager>
    {
        [Serializable]
        class QuestData
        {
            public List<string> InProgressQuest = new();
            public List<string> CompletedQuests = new();
        }

        public QuestSaveObject(QuestManager obj) : base(obj)
        {
        }

        public override string Key => "Quest";

        public override string ToJson()
        {
            var questData = new QuestData();
            foreach (var item in RefObject.InProgressQuest)
            {
                questData.InProgressQuest.Add(item.Guid);
            }
            foreach (var item in RefObject.CompletedQuests)
            {
                questData.CompletedQuests.Add(item.Guid);
            }
            return JsonUtility.ToJson(questData);
        }

        public override IEnumerator CoFromJson(string json, Action<bool> callback = null)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var questData = new QuestData();
                JsonUtility.FromJsonOverwrite(json, questData);
                if (questData.InProgressQuest.Count > 0 || questData.CompletedQuests.Count > 0)
                {
                    RefObject.CompletedQuests.Clear();
                    RefObject.InProgressQuest.Clear();

                    foreach (var guid in questData.CompletedQuests)
                    {
                        var questSoHandle = Addressables.LoadAssetAsync<QuestSO>(guid);
                        yield return questSoHandle;
                        if (questSoHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            RefObject.GiveQuest(questSoHandle.Result);
                            RefObject.OnQuestCompleted(questSoHandle.Result);
                        }
                    }

                    foreach (var guid in questData.InProgressQuest)
                    {
                        var questSoHandle = Addressables.LoadAssetAsync<QuestSO>(guid);
                        yield return questSoHandle;
                        if (questSoHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            RefObject.GiveQuest(questSoHandle.Result);
                        }
                    }

                    var lastCutsceneActionQuestIdx = RefObject.CompletedQuests.FindLastIndex(quest => quest.BaseData.GetType() == typeof(CutsceneBranchingQuestSO) && quest.BaseData.NextAction != null && quest.BaseData.NextAction.GetType() == typeof(CutsceneAction));
                    var lastCutsceneBattleQuestIdx = RefObject.CompletedQuests.FindLastIndex(quest => quest.BaseData.GetType() == typeof(CutsceneBranchingQuestSO) && quest.BaseData.NextAction != null && quest.BaseData.NextAction.GetType() == typeof(BattleAction));
                    var inprogressCutsceneBattleQuestIdx = RefObject.InProgressQuest.FindLastIndex(quest => quest.BaseData.GetType() == typeof(CutsceneBranchingQuestSO) && quest.BaseData.NextAction != null && quest.BaseData.NextAction.GetType() == typeof(BattleAction));

                    // If has CutsceneAction but BattleAction inprogress, retrigger CutsceneAction
                    if (lastCutsceneActionQuestIdx > lastCutsceneBattleQuestIdx && inprogressCutsceneBattleQuestIdx > -1)
                    {
                        var cutsceneQuest = RefObject.CompletedQuests.ElementAt(lastCutsceneActionQuestIdx);
                        RefObject.CompletedQuests.RemoveAt(lastCutsceneActionQuestIdx);
                        RefObject.InProgressQuest.RemoveAt(inprogressCutsceneBattleQuestIdx);
                        RefObject.GiveQuest(cutsceneQuest.BaseData);
                    }

                    var lastBattleQuestIdx = RefObject.CompletedQuests.FindLastIndex(quest => quest.BaseData.GetType() == typeof(BattleQuestSO));
                    var inprogressBattleQuestIdx = RefObject.InProgressQuest.FindLastIndex(quest => quest.BaseData.GetType() == typeof(BattleQuestSO));

                    // If has battle inprogress, retrigger CutsceneAction
                    if (lastCutsceneActionQuestIdx < lastCutsceneBattleQuestIdx && lastCutsceneBattleQuestIdx > lastBattleQuestIdx)
                    {
                        var cutsceneQuest = RefObject.CompletedQuests.ElementAt(lastCutsceneActionQuestIdx);
                        RefObject.CompletedQuests.RemoveAt(lastCutsceneBattleQuestIdx);
                        RefObject.CompletedQuests.RemoveAt(lastCutsceneActionQuestIdx);
                        if (inprogressBattleQuestIdx >= 0) RefObject.InProgressQuest.RemoveAt(inprogressBattleQuestIdx);
                        RefObject.GiveQuest(cutsceneQuest.BaseData);
                    }

                    if (callback != null) { callback(true); }
                    yield break;
                }
            }
            if (callback != null) { callback(false); }
            yield break;
        }
    }
}