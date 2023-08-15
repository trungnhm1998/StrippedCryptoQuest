using CryptoQuest.Audio.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.System.Settings
{
    public class AudioSettingController : MonoBehaviour
    {
        [Header("Listen on")]
        [SerializeField] private AudioSettingSO _audioSettings;

        [SerializeField] private TMP_Text _volumeText;
        [SerializeField] private Slider _volumeSlider;

        private void OnEnable()
        {
            ChangeAudioVolume(_audioSettings.Volume);
            _volumeSlider.value = _audioSettings.Volume;
        }

        public void ChangeAudioVolume(float value)
        {
            _audioSettings.Volume = (int)value / 100;
            _volumeText.text = $"{value,0:F0}%";
        }
    }
}