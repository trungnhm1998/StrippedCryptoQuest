using CryptoQuest.Audio.Settings;
using UnityEngine;

namespace CryptoQuest.System.Settings
{
    public class AudioSettingController : MonoBehaviour
    {
        [Header("Listen on")]
        [SerializeField] private AudioSettingsSO _settings;

        public void ChangeAudioVolume(float value)
        {
            _settings.Volume = value;
        }
    }
}