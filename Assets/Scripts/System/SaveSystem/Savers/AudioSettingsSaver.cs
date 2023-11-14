using CryptoQuest.Audio.Settings;
using CryptoQuest.SaveSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public class AudioSettingsSaver : MonoBehaviour
    {
        [SerializeField] private AudioSettingSO _audioSetting;
        [SerializeField] private SaveSystemSO _saveSystem;

        protected void OnEnable()
        {
            _audioSetting.VolumeChanged += VolumeChanged_Save;
        }

        protected void OnDisable()
        {
            _audioSetting.VolumeChanged -= VolumeChanged_Save;
        }

        private void VolumeChanged_Save(float volume)
        {
            _saveSystem.SaveData[_audioSetting.name] = JsonConvert.SerializeObject(_audioSetting);
            _saveSystem.Save();
        }
    }
}