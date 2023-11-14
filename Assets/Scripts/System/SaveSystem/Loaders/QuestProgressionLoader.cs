using System.Collections;
using CryptoQuest.Quest;
using CryptoQuest.SaveSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public class QuestProgressionLoader : MonoBehaviour, ILoader
    {
        [SerializeField] private QuestSaveSO _questSave;

        public IEnumerator Load(ISaveSystem progressionSystem)
        {
#if UNITY_EDITOR
            _questSave.InProgressQuest.Clear();
            _questSave.CompletedQuests.Clear();
#endif
            if (progressionSystem.SaveData.TryGetValue(_questSave.name, out var json))
                JsonConvert.PopulateObject(json, _questSave);
            yield break;
        }
    }
}