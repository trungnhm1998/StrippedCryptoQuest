using CryptoQuest.Events;
using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Language.Settings
{
    public class LanguageSettingSO : ScriptableObject 
    {
        [SerializeField] private Locale _currentLanguage;
        public event UnityAction<Locale> CurrentLanguageChanged;

        public Locale CurrentLanguage
        {
            get => _currentLanguage;

            set
            {
                _currentLanguage = value;
                this.CallEventSafely(CurrentLanguageChanged, _currentLanguage);
            }
        }
    }
}