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

        protected void OnEnable()
        {
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;
            _languageSetting.CurrentLanguageIndexChanged += OnChangeLanguage;

            if (!_initializeOperation.IsDone)
            {
                _initializeOperation.Completed += InitializeCompleted;
                return;
            }

            InitializeCompleted(_initializeOperation);
        }

        protected void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
            _languageSetting.CurrentLanguageIndexChanged -= OnChangeLanguage;
        }

        private void InitializeCompleted(AsyncOperationHandle obj)
        {
            _initializeOperation.Completed -= InitializeCompleted;

            LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        }

        private void SelectedLocaleChanged(Locale locale)
        {
            int index = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            Locale currentLocale = LocalizationSettings.AvailableLocales.Locales[index];
            _languageSetting.CurrentLanguage = currentLocale;
        }

        private void OnChangeLanguage(Locale locale) => LocalizationSettings.SelectedLocale = locale;
    }
}