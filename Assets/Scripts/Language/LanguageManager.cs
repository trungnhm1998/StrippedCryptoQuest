using System;
using System.Collections.Generic;
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
        public Locale Locale;
    }

    /// <summary>
    /// TODO: This class must be wait to Sage scene loaded
    /// Because it will be load language from save data
    /// 
    /// <remarks>
    /// @Author: thai-phi
    /// </remarks>
    /// 
    /// </summary>
    public class LanguageManager : MonoBehaviour
    {
        [SerializeField] private LanguageSettingSO _languageSetting;

        [HideInInspector] public LanguageSave SaveData;

        private TinyMessenger.TinyMessageSubscriptionToken _listenToLoadCompletedEventToken;
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

        private void InitLanguage(bool loaded)
        {
            ActionDispatcher.Unbind(_listenToLoadCompletedEventToken);
            OnChangeLanguage(loaded ? SaveData.Locale : LocalizationSettings.AvailableLocales.Locales[0]);
        }

        private void InitializeCompleted(AsyncOperationHandle obj)
        {
            _initializeOperation.Completed -= InitializeCompleted;

            _listenToLoadCompletedEventToken =
                ActionDispatcher.Bind<LoadLanguageCompletedAction>(action => InitLanguage(action.IsSuccess));
            ActionDispatcher.Dispatch(new LoadLanguageAction(this));

            LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        }

        private void SelectedLocaleChanged(Locale locale)
        {
            int index = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            Locale currentLocale = LocalizationSettings.AvailableLocales.Locales[index];

            SaveData.Locale = currentLocale;
            ActionDispatcher.Dispatch(new SaveLanguageAction(this));
        }

        private void OnChangeLanguage(Locale locale) => LocalizationSettings.SelectedLocale = locale;
    }
}