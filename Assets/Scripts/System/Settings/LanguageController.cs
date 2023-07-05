using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.System.Settings
{
    public class LanguageController : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown _dropdown;

        private int _currentSelectedOption = 0;
        private AsyncOperationHandle _initializeOperation;
        private List<string> _languagesList = new List<string>();

        private void OnEnable()
        {
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;

            _initializeOperation.Completed += InitializeCompleted;
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
        }

        private void InitializeLanguage()
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_languagesList);
        }

        private void InitializeCompleted(AsyncOperationHandle obj)
        {
            _initializeOperation.Completed -= InitializeCompleted;

            List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;

            for (int i = 0; i < locales.Count; ++i)
            {
                var locale = locales[i];
                if (LocalizationSettings.SelectedLocale == locale) _currentSelectedOption = i;

                var displayName = locales[i].Identifier.CultureInfo != null
                    ? locales[i].Identifier.CultureInfo.NativeName
                    : locales[i].ToString();
                _languagesList.Add(displayName);
            }

            InitializeLanguage();
            LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        }

        private void SelectedLocaleChanged(Locale locale)
        {
            var index = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            _dropdown.value = index;
        }

        public void OnChangeLanguage(int index)
        {
            _currentSelectedOption = index;
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;

            var locale = LocalizationSettings.AvailableLocales.Locales[_currentSelectedOption];
            LocalizationSettings.SelectedLocale = locale;

            LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        }
    }
}