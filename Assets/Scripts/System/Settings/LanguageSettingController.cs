using System.Collections.Generic;
using CryptoQuest.Language;
using CryptoQuest.Language.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.System.Settings
{
    public class LanguageSettingController : MonoBehaviour
    {
        [SerializeField] private LanguageSettingSO _languageSetting;

        [Header("UI")]
        [SerializeField] TMP_Dropdown _dropdown;

        private void Start() => InitializeLanguage();
        public void OnChangeLanguage(int index) => _languageSetting.CurrentLanguageIndex = index;
        public void Initialize() => _dropdown.Select();
        public void DeInitialize() => _dropdown.Hide();

        private void InitializeLanguage()
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_languageSetting.LanguageList);
            _dropdown.value = _languageSetting.CurrentLanguageIndex;
        }
    }
}