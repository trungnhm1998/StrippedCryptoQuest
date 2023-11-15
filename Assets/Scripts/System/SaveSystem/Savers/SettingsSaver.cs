using System;
using CryptoQuest.Audio.Settings;
using CryptoQuest.Language.Settings;
using CryptoQuest.SaveSystem;
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

    public class SettingsSaver : MonoBehaviour
    {
        [SerializeField] private AudioSettingSO _audioSetting;
        [SerializeField] private LanguageSettingSO _languageSetting;
        [SerializeField] private SaveSystemSO _saveSystem;

        protected void OnEnable()
        {
            _audioSetting.VolumeChanged += VolumeChanged_Save;
            _languageSetting.Changed += LanguageChanged_Save;
        }

        protected void OnDisable()
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
            _saveSystem.Save();
        }
    }
}