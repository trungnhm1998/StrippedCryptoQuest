using System;
using CryptoQuest.Audio.Settings;
using CryptoQuest.Language.Settings;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class Settings
    {
        public float Volume;
        public string Language; // guid
    }

    [Serializable]
    public class SettingsSaver : SaverBase
    {
        [SerializeField] private AudioSettingSO _audioSetting;
        [SerializeField] private LanguageSettingSO _languageSetting;

        public override void RegistEvents()
        {
            _audioSetting.VolumeChanged += VolumeChanged_Save;
            _languageSetting.Changed += LanguageChanged_Save;
        }

        public override void UnregistEvents()
        {
            _audioSetting.VolumeChanged -= VolumeChanged_Save;
            _languageSetting.Changed -= LanguageChanged_Save;
        }

        private void LanguageChanged_Save(Locale _) => Save();

        private void VolumeChanged_Save(float _) => Save();

        private void Save()
        {
            _saveSystem.SaveData[SerializeKeys.SETTINGS] = JsonConvert.SerializeObject(new Settings
            {
                Volume = _audioSetting.Volume,
                Language = _languageSetting.CurrentLocale.Guid
            });

            // Save locally because change volume create so many request
            // will be upload to server when force save
            _saveSystem.Save();
        }
    }
}