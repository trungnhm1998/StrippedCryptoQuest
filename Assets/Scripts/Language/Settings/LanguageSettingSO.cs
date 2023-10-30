using System.Collections.Generic;
using CryptoQuest.Events;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Language.Settings
{
    public class LanguageSettingSO : ScriptableObject
    {
        /// <summary>
        /// Hide in inspector because I don't want any one edit here
        /// ƪ(˘⌣˘)ʃ
        /// </summary>
        [SerializeField, HideInInspector] private List<string> _languageList = new();
        [SerializeField, HideInInspector] private int _currentLanguageIndex = 0;

        public event UnityAction<List<string>> LanguageListChanged;
        public event UnityAction<int> CurrentLanguageIndexChanged;

        public List<string> LanguageList
        {
            get => _languageList;
            set
            {
                _languageList = value;
                this.CallEventSafely(LanguageListChanged, _languageList);
            }
        }

        public int CurrentLanguageIndex
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