using System.Collections.Generic;
using CryptoQuest.Language;
using CryptoQuest.Language.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.Menus.Settings.UI
{
    public class UILanguageOptions : MonoBehaviour
    {
        [SerializeField] private LanguageSettingSO _languageSetting;

        [Header("UI")]
        [SerializeField] TMP_Dropdown _dropdown;

        private void Start() => InitializeLanguage();
        public void OnChangeLanguage(int index) => _languageSetting.CurrentLanguageIndex = LocalizationSettings.AvailableLocales.Locales[index];
        public void Initialize() => _dropdown.Select();
        public void DeInitialize() => _dropdown.Hide();

        private void InitializeLanguage()
        {
            _dropdown.ClearOptions();
            List<string> localeNames = LanguageHelper.GetLocaleNames();
            int localeIndex = LanguageHelper.GetLocaleIndex(_languageSetting.CurrentLanguageIndex);
            
            _dropdown.AddOptions(localeNames);
            _dropdown.value = localeIndex;
        }
    }
}