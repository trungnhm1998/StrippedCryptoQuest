using System;
using System.Collections;
using CryptoQuest.Quest;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class QuestProgressionLoader : Loader
    {
        [SerializeField] private QuestSaveSO _questSave;
        [SerializeField] private SaveSystemSO _progressionSystem;

        public override void Load()
        {
            _questSave.InProgressQuest.Clear();
            _questSave.CompletedQuests.Clear();
            if (!_progressionSystem.SaveData.TryGetValue(_questSave.name, out var json)) return;
            JsonConvert.PopulateObject(json, _questSave);
        }
    }
}