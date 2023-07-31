using CryptoQuest.Audio.Settings;
using UnityEngine;

namespace CryptoQuest.System.Settings
{
    public class AudioSettingController : MonoBehaviour
    {
        [Header("Listen on")]
        [SerializeField] private AudioSettingSO audioSettings;

        public void ChangeAudioVolume(float value)
        {
            audioSettings.Volume = value;
        }
    }
}