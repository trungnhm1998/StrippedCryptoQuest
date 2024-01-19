using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CryptoQuest.Quest.Authoring;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.CryptoQuestEditor.Scripts.System.QuestSystem
{
    public class QuestChapterLoader : ScriptableObject
    {
        [SerializeField] private TextAsset _questChapterData;
        [SerializeField] private List<QuestSO> _allQuests = new();

        private const string IGNORE_PATH_QUEST_SYSTEM = "Assets/ScriptableObjects/QuestSystem/WIP";
        private const string IGNORE_PATH_EVENTS_QUEST = "Assets/ScriptableObjects/Events/Quest/WIP";

        private const string CHAPTER_FIELD = "<Chapter>k__BackingField";
        private const string EVENT_DESCRIPTION_FIELD = "<EventDescription>k__BackingField";

        [ContextMenu("Load Quests")]
        public void LoadChapterQuest()
        {
            var csvPath = AssetDatabase.GetAssetPath(_questChapterData);
            using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream = new StreamReader(fs);

            var config = new CsvConfiguration(CultureInfo.CurrentCulture);
            using var csv = new CsvReader(stream, config);
            csv.Read();
            csv.ReadHeader();
            csv.Read();
            while (csv.Read())
            {
                string[] paths = AssetDatabase.FindAssets("t:QuestSO");

                string questNameCSV = csv.GetField<string>("quest_name").Replace(" ", "");
                int chapterCSV = csv.GetField<int>("chapter");
                string descriptionCSV = csv.GetField<string>("description_JP").Replace(" ", "");

                UpdateIgnoredQuests(paths);

                List<QuestSO> quests = paths
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(path => !path.StartsWith(IGNORE_PATH_EVENTS_QUEST))
                    .Where(path => !path.StartsWith(IGNORE_PATH_QUEST_SYSTEM))
                    .Select(AssetDatabase.LoadAssetAtPath<QuestSO>)
                    .ToList();

                if (quests.Count == 0) continue;

                _allQuests = quests;

                foreach (QuestSO quest in _allQuests)
                {
                    var instance = new SerializedObject(quest);
                    instance.Update();

                    string nameQuestDataLower = quest.QuestName.ToLower();
                    string nameQuestCSVLower = questNameCSV.ToLower();

                    if (!nameQuestDataLower.Equals(nameQuestCSVLower)) continue;

                    instance.FindProperty(CHAPTER_FIELD).intValue = chapterCSV;
                    instance.FindProperty(EVENT_DESCRIPTION_FIELD).stringValue = descriptionCSV;

                    instance.ApplyModifiedProperties();

                    EditorUtility.SetDirty(quest);
                    AssetDatabase.SaveAssets();
                }
            }
        }


        private void UpdateIgnoredQuests(string[] paths)
        {
            var questIgnore = paths
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path =>
                    path.StartsWith(IGNORE_PATH_QUEST_SYSTEM) ||
                    path.StartsWith(IGNORE_PATH_EVENTS_QUEST))
                .Select(AssetDatabase.LoadAssetAtPath<QuestSO>)
                .ToList();

            foreach (var quest in questIgnore)
            {
                var instance = new SerializedObject(quest);
                instance.Update();

                instance.FindProperty(CHAPTER_FIELD).intValue = -1;
                instance.FindProperty(EVENT_DESCRIPTION_FIELD).stringValue = string.Empty;

                instance.ApplyModifiedProperties();
                EditorUtility.SetDirty(quest);
                AssetDatabase.SaveAssets();
            }
        }
    }
}