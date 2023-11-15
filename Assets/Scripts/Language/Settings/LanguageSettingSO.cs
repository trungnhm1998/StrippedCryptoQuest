using CryptoQuest.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Language.Settings
{
    public class LanguageSettingSO : ScriptableObject
    {
        /// <summary>
        /// Hide in inspector because I don't want any one edit here
        /// ƪ(˘⌣˘)ʃ
        /// </summary>
        [SerializeField] private Locale _currentLanguage;

        public event UnityAction<Locale> CurrentLanguageIndexChanged;

        public Locale CurrentLanguage
        {
            get => _currentLanguage;

            set
            {
                _currentLanguage = value;
                CurrentLanguageIndexChanged.SafeInvoke(_currentLanguage);
            }
        }
    }
}