using System;
using CryptoQuest.Quest;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class QuestProgressionSaver : SaverBase
    {
        [SerializeField] private QuestSaveSO _questSave;

        public override void RegistEvents()
        {
            _questSave.Changed += SaveQuestProgression;
        }

        public override void UnregistEvents()
        {
            _questSave.Changed -= SaveQuestProgression;
        }

        private void SaveQuestProgression()
        {
            _saveSystem.SaveData[_questSave.name] = JsonConvert.SerializeObject(_questSave);
            _saveHandler.Save();
        }
    }
}