using System.Collections.Generic;
using CryptoQuest.Language.Settings;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Language
{
    public class LanguageManager : MonoBehaviour
    {
        [SerializeField] private LanguageSettingSO _languageSetting;

        private AsyncOperationHandle _initializeOperation;
        private int _currentSelectedOption = 0;

        protected void OnEnable()
        {
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;
            _initializeOperation.Completed += InitializeCompleted;
            _languageSetting.CurrentLanguageIndexChanged += OnChangeLanguage;
        }

        protected void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
            _languageSetting.CurrentLanguageIndexChanged -= OnChangeLanguage;
        }

        private void InitializeCompleted(AsyncOperationHandle obj)
        {
            _initializeOperation.Completed -= InitializeCompleted;

            List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
            List<string> languagesList = new();

            for (int i = 0; i < locales.Count; ++i)
            {
                var locale = locales[i];
                if (LocalizationSettings.SelectedLocale == locale) _currentSelectedOption = i;

                var displayName = locales[i].Identifier.CultureInfo != null
                    ? locales[i].Identifier.CultureInfo.NativeName
                    : locales[i].ToString();
                languagesList.Add(displayName);
            }

            _languageSetting.LanguageList = languagesList;

            LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        }

        private void OnSelectionChanged()
        {
            LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;

            var locale = LocalizationSettings.AvailableLocales.Locales[_currentSelectedOption];
            LocalizationSettings.SelectedLocale = locale;

            LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        }

        private void SelectedLocaleChanged(Locale locale)
        {
            var index = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            _currentSelectedOption = index;
        }

        private void OnChangeLanguage(int index)
        {
            _currentSelectedOption = index;
            OnSelectionChanged();
        }
    }
}