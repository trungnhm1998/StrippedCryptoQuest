using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.System.Settings
{
    public class LanguageController : MonoBehaviour
    {
        public event UnityAction<Locale> Save = delegate { };

        private int _currentSelectedOption = 0;
        private LanguageType _savedSelectedOption = default;
        private AsyncOperationHandle _initializeOperation;
        private List<string> _languagesList = new List<string>();

        private void OnEnable()
        {
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;

            if (_initializeOperation.IsDone)
            {
                InitializeCompleted(_initializeOperation);
            }
            else
            {
                _initializeOperation.Completed += InitializeCompleted;
            }
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
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
                    ? locales[i].Identifier.CultureInfo.DisplayName
                    : locales[i].Identifier.Code;
                _languagesList.Add(displayName);
            }

            _savedSelectedOption = (LanguageType)_currentSelectedOption;
            Debug.Log($"Saved Locale is {(LanguageType)_savedSelectedOption}");
            LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        }

        private void SelectedLocaleChanged(Locale locale)
        {
            var index = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            Debug.Log($"Selected Locale changed to {locale.name} ({(LanguageType)index})");
        }
    }

    public enum LanguageType
    {
        English = 0,
        Japanese = 1,
    }
}