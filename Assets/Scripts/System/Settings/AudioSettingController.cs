using UnityEngine;
using AudioSettings = CryptoQuest.Events.Settings.AudioSettings;

namespace CryptoQuest.System.Settings
{
    public class AudioSettingController : MonoBehaviour
    {
        [Header("Listen on")]
        [SerializeField] private AudioSettings _soundValueEventChannel;

        public void OnSoundChanged(float value)
        {
            _soundValueEventChannel.Volume = value;
        }
    }
}