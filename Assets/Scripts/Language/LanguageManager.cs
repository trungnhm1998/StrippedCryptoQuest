using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Audio.Settings;
using CryptoQuest.Core;
using CryptoQuest.Language.Settings;
using CryptoQuest.System.SaveSystem.Actions;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Language
{
    [Serializable]
    public class LanguageSave
    {
        public int Index = 0;
    }

    public class LanguageManager : MonoBehaviour
    {
        [SerializeField] private LanguageSettingSO _languageSetting;

        [HideInInspector]
        public LanguageSave SaveData;
        private TinyMessenger.TinyMessageSubscriptionToken _listenToLoadCompletedEventToken;

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

            _listenToLoadCompletedEventToken = ActionDispatcher.Bind<LoadLanguageCompletedAction>(action => InitLanguage(action.IsSuccess));
            ActionDispatcher.Dispatch(new LoadLanguageAction(this));
        }

        private void InitLanguage(bool loaded)
        {
            ActionDispatcher.Unbind(_listenToLoadCompletedEventToken);
            if (loaded)
            {
                OnChangeLanguage(SaveData.Index);
            }
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

            SaveData.Index = index;
            ActionDispatcher.Dispatch(new SaveLanguageAction(this));
        }

        private void OnChangeLanguage(int index)
        {
            _currentSelectedOption = index;
            OnSelectionChanged();
        }
    }
}