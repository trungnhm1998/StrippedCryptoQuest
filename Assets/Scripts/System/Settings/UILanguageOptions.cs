using System.Collections.Generic;
using CryptoQuest.Language;
using CryptoQuest.Language.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.System.Settings
{
    public class UILanguageOptions : MonoBehaviour
    {
        [SerializeField] private LanguageSettingSO _languageSetting;

        [Header("UI")] [SerializeField] TMP_Dropdown _dropdown;

        private int _localeIndex;

        private void OnEnable() => InitializeLanguage();
        private void OnDisable() => LocalizationSettings.SelectedLocaleChanged -= OnChangeLocale;

        public void OnChangeLanguage(int index) => _languageSetting.CurrentLanguage =
            LocalizationSettings.AvailableLocales.Locales[index];

        public void Initialize() => _dropdown.Select();
        public void DeInitialize() => _dropdown.Hide();

        private void InitializeLanguage()
        {
            _dropdown.ClearOptions();
            LocalizationSettings.SelectedLocaleChanged += OnChangeLocale;

            List<string> localeNames = LanguageHelper.GetLocaleNames();
            _localeIndex = LanguageHelper.GetLocaleIndex(_languageSetting.CurrentLanguage);


            _dropdown.AddOptions(localeNames);
            _dropdown.value = _localeIndex;
        }

        private void OnChangeLocale(Locale locale)
        {
            List<string> localeNames = LanguageHelper.GetLocaleNames();

            _dropdown.ClearOptions();
            _dropdown.AddOptions(localeNames);
            _dropdown.value = LanguageHelper.GetLocaleIndex(locale);
        }
    }
}