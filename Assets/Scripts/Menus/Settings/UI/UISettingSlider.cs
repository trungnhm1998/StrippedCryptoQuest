using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Audio.Settings;
using IndiGames.Core.Events.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Settings.UI
{
    public class UISettingSlider : MonoBehaviour
    {
        [Header("Listen on")]
        [SerializeField] private AudioSettingSO _settingSO;

        [SerializeField] private TMP_Text _settingText;
        [SerializeField] private Slider _settingSlider;

        private void OnEnable()
        {
            float valuePercentage = _settingSO.Volume * 100;
            UpdateUI(valuePercentage);
        }

        public void ChangeValue(float value)
        {
            float valuePercentage = value / 100;
            _settingSO.Volume = valuePercentage;

            UpdateUI(value);
        }

        private void UpdateUI(float value)
        {
            _settingText.text = $"{value:F0}%";
            _settingSlider.value = value;
        }
    }
}
