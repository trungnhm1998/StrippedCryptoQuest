using System.Collections;
using CryptoQuest.Audio.Settings;
using CryptoQuest.SaveSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public class AudioSettingsLoader : MonoBehaviour, ILoader
    {
        [SerializeField] private AudioSettingSO _audioSetting;

        public IEnumerator Load(ISaveSystem progressionSystem)
        {
            if (progressionSystem.SaveData.TryGetValue(_audioSetting.name, out var json))
                JsonConvert.PopulateObject(json, _audioSetting);
            yield break;
        }
    }
}