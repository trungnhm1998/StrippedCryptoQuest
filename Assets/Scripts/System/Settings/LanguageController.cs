using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.System.Settings
{
    [Serializable]
    public class LanguageSave
    {
        public int LanguageIndex = 0;
    }

    public class LanguageController : SaveObject
    {
        [Header("UI")]
        [SerializeField] TMP_Dropdown _dropdown;

        [SerializeField] private LanguageSave _saveData;

        private int _currentSelectedOption = 0;
        private AsyncOperationHandle _initializeOperation;
        private List<string> _languagesList = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;
            _initializeOperation.Completed += InitializeCompleted;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
        }

        public void OnChangeLanguage(int index)
        {
            _currentSelectedOption = index;
            OnSelectionChanged();
        }

        public void Initialize() => _dropdown.Select();
        public void DeInitialize() => _dropdown.Hide();

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
            _dropdown.value = index;

            _saveData.LanguageIndex = index;
            SaveSystem?.SaveObject(this);
        }

        #region SaveSystem

        public override string Key => "LanguageIndex";

        public override string ToJson()
        {
            return JsonUtility.ToJson(_saveData);
        }

        public override IEnumerator CoFromJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JsonUtility.FromJsonOverwrite(json, _saveData);
                OnChangeLanguage(_saveData.LanguageIndex);
            }
            yield return null;
        }

        #endregion
    }
}