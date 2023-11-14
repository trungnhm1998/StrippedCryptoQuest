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
        [SerializeField, HideInInspector] private Locale _currentLanguageIndex;

        public event UnityAction<Locale> CurrentLanguageIndexChanged;

        public Locale CurrentLanguageIndex
        {
            get => _currentLanguageIndex;

            set
            {
                _currentLanguageIndex = value;
                this.CallEventSafely(CurrentLanguageIndexChanged, _currentLanguageIndex);
            }
        }
    }
}