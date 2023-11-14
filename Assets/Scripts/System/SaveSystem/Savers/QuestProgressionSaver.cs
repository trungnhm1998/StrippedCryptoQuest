using CryptoQuest.Quest;
using CryptoQuest.SaveSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public class QuestProgressionSaver : MonoBehaviour
    {
        [SerializeField] private QuestSaveSO _questSave;
        [SerializeField] private SaveSystemSO _saveSystem;

        private void OnEnable()
        {
            _questSave.Changed += SaveQuestProgression;
        }

        private void OnDisable()
        {
            _questSave.Changed -= SaveQuestProgression;
        }

        private void SaveQuestProgression()
        {
            _saveSystem.SaveData[_questSave.name] = JsonConvert.SerializeObject(_questSave);
            _saveSystem.Save();
        }
    }
}