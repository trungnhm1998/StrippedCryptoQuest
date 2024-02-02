using System.Collections.Generic;
using System.Linq;
using CommandTerminal;
using CryptoQuest.Quest;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Sagas;
using IndiGames.Core.Events;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class QuestCheat : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private QuestSaveSO _saveData;
        [SerializeField] private List<QuestSO> _allQuest = new();

        private Dictionary<string, QuestSO> _questDict = new();
        private static int HIGHEST_CHAPTER = 1;

        private void OnValidate()
        {
#if UNITY_EDITOR
            var paths = AssetDatabase.FindAssets("t:QuestSO");
            var quests = paths.Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<QuestSO>)
                .ToList();

            _allQuest = quests;
            EditorUtility.SetDirty(this);
#endif
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("quest.delete", RequestDeleteCompletedQuest, 1, 1,
                "quest.delete <quest_guid>, to delete completed quest");

            Terminal.Shell.AddCommand("quest.show.completed", RequestShowCompletedQuests, 0, 0,
                "quest.show.completed, to show all completed quests");

            Terminal.Shell.AddCommand("quest.show.inprogress", RequestShowInProgressQuests, 0, 0,
                "quest.show.inprogress, to show all in progress quests");

            Terminal.Shell.AddCommand("quest.delete.all", RequestDeleteAllCompletedQuests, 0, 0,
                "quest.delete.all, to delete all completed quests");

            Terminal.Shell.AddCommand("quest.complete.chapter", RequestCompleteQuestByChapter, 1, 1,
                "quest.complete.chapter <chapter>, to complete all quest in chapter");

            Terminal.Shell.AddCommand("quest.complete", RequestCompleteQuest, 1, 1,
                "quest.complete <quest_guid>, to complete quest");

            foreach (var questSo in _allQuest)
            {
                _questDict.Add(questSo.Guid, questSo);
                Terminal.Autocomplete.Register(questSo.Guid);
            }
        }

        private void RequestCompleteQuest(CommandArg[] questGuids)
        {
            var guid = questGuids[0].String;

            if (IsValid(guid)) return;

            _saveData.AddCompleteQuest(guid);
            Debug.Log($"<color=green>Completed quest {guid}</color>");
        }

        private void RequestCompleteQuestByChapter(CommandArg[] questGuids)
        {
            int chapter = questGuids[0].Int;
            if (chapter is 0 or -1) return;


            if (chapter > HIGHEST_CHAPTER) HIGHEST_CHAPTER = chapter;

            for (int chapterIndex = 0; chapterIndex < HIGHEST_CHAPTER; chapterIndex++)
            {
                List<QuestSO> quests = _allQuest.Where(quest => quest.Chapter == chapterIndex).ToList();
                quests.ForEach(quest => _saveData.AddCompleteQuest(quest.Guid));
            }
        }

        private void RequestDeleteAllCompletedQuests(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new QuestCleanAllAction());
            Debug.Log($"<color=green>Deleted all completed quests</color>");
        }

        private void ShowQuests(string header, List<string> questGuids)
        {
            List<QuestSO> quests = questGuids.Select(guid => _questDict[guid]).ToList();

            Debug.Log($"<color=green>{header}</color>");
            Debug.Log($"<color=red>Current quest: </color> {quests.Count}");

            for (var index = 0; index < quests.Count; index++)
            {
                var quest = quests[index];
                Debug.Log($"{index + 1}. {quest.QuestName} - {quest.Guid}");
            }
        }

        private void RequestDeleteCompletedQuest(CommandArg[] questGuids)
        {
            var guid = questGuids[0].String;

            if (IsValid(guid)) return;

            _saveData.RemoveCompleteQuest(guid);
            Debug.Log($"<color=green>Deleted quest {guid}</color>");
        }

        private void RequestShowInProgressQuests(CommandArg[] _) =>
            ShowQuests("Current In Progress Quest", _saveData.InProgressQuest);

        private void RequestShowCompletedQuests(CommandArg[] _) =>
            ShowQuests("Current Completed Quest", _saveData.CompletedQuests);

        private bool IsValid(string guid)
        {
            if (_saveData.CompletedQuests.Contains(guid)) return false;
            Debug.Log($"<color=red>Quest {guid} is not completed</color>");
            return true;
        }
    }
}