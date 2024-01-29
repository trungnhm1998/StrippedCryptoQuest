using System;
using System.Collections;
using CryptoQuest.Audio;
using CryptoQuest.Audio.Settings;
using CryptoQuest.Language.Settings;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class SettingsLoader : Loader
    {
        [SerializeField] private AudioSettingSO _audioSetting;
        [SerializeField] private LanguageSettingSO _languageSetting;
        [SerializeField] private SaveSystemSO _progressionSystem;

        public override IEnumerator LoadAsync()
        {
            if (!_progressionSystem.SaveData.TryGetValue(SerializeKeys.SETTINGS, out var json))
            {
                ActionDispatcher.Dispatch(new PlayMusicInTitleSceneAction());
                yield break;
            }

            var settings = JsonConvert.DeserializeObject<Savers.Settings>(json);

            _audioSetting.Volume = settings.Volume;

            ActionDispatcher.Dispatch(new PlayMusicInTitleSceneAction());

            var language = Addressables.LoadAssetAsync<SerializeLocale>(settings.Language);
            yield return language;

            _languageSetting.CurrentLanguage = language.Result.Locale;
        }
    }
}