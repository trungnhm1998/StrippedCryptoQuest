using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Language.Settings;
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

    public class LanguageManager : SaveObject
    {
        [SerializeField] private LanguageSettingSO _languageSetting;

        [SerializeField, HideInInspector] private LanguageSave _saveData;

        private AsyncOperationHandle _initializeOperation;
        private int _currentSelectedOption = 0;

        protected override void OnEnable()
        {
            base.OnEnable();

            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;
            _initializeOperation.Completed += InitializeCompleted;

            _languageSetting.CurrentLanguageIndexChanged += OnChangeLanguage;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

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

            _saveData.Index = index;
            SaveSystem?.SaveObject(this);
        }

        private void OnChangeLanguage(int index)
        {
            _currentSelectedOption = index;
            OnSelectionChanged();
        }

        #region SaveSystem

        public override string Key => "Language";

        public override string ToJson()
        {
            return JsonUtility.ToJson(_saveData);
        }

        public override IEnumerator CoFromJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JsonUtility.FromJsonOverwrite(json, _saveData);
                OnChangeLanguage(_saveData.Index);
            }

            yield return null;
        }

        #endregion
    }
}