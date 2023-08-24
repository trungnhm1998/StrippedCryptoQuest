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
            float volumePercentage = _audioSettings.Volume * 100;
            UpdateVolumeDisplay(volumePercentage);
        }

        public void ChangeAudioVolume(float value)
        {
            float volumeValue = value / 100;
            _audioSettings.Volume = volumeValue;

            UpdateVolumeDisplay(value);
        }

        private void UpdateVolumeDisplay(float value)
        {
            _volumeText.text = $"{value:F0}%";
            _volumeSlider.value = value;
        }
    }
}