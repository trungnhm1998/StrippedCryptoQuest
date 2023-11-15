using System.Collections.Generic;
using CryptoQuest.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Language.Settings
{
    public class LanguageSettingSO : ScriptableObject
    {
        [SerializeField] private SerializeLocale _currentLanguage;
        public SerializeLocale CurrentLocale => _currentLanguage;
        [SerializeField] private SerializeLocale[] _availableLanguages;

        private Dictionary<Locale, SerializeLocale> _availableLanguagesDictionary;

        private void OnEnable()
        {
            _availableLanguagesDictionary = new Dictionary<Locale, SerializeLocale>();
            foreach (SerializeLocale language in _availableLanguages)
            {
                _availableLanguagesDictionary.Add(language.Locale, language);
            }
        }

        public event UnityAction<Locale> Changed;

        public Locale CurrentLanguage
        {
            get => _currentLanguage.Locale;
            set
            {
                _currentLanguage = _availableLanguagesDictionary[value];
                Changed.SafeInvoke(_currentLanguage.Locale);
            }
        }
    }
}