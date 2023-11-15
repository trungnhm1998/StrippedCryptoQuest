using System.Collections;
using CryptoQuest.Audio.Settings;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Language.Settings;
using CryptoQuest.SaveSystem;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public class SettingsLoader : MonoBehaviour, ILoader
    {
        [SerializeField] private AudioSettingSO _audioSetting;
        [SerializeField] private LanguageSettingSO _languageSetting;

        public IEnumerator Load(ISaveSystem progressionSystem)
        {
            if (progressionSystem.SaveData.TryGetValue(SerializeKeys.SETTINGS, out var json))
            {
                var settings = JsonConvert.DeserializeObject<Savers.Settings>(json);
                _audioSetting.Volume = settings.Volume;
                var language = Addressables.LoadAssetAsync<SerializeLocale>(settings.Language);
                yield return language;
                _languageSetting.CurrentLanguage = language.Result.Locale;
            }
            yield break;
        }
    }
}