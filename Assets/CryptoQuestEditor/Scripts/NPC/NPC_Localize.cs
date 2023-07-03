using UnityEngine.Localization.Settings;

namespace CryptoQuestEditor.NPC
{
    public class NPC_Localize
    {
        public string _createTitle;
        public string _name;
        public string _sprite;
        public string _collider;
        public string _exportPath;
        public string _baseNPC;
        public string _buttonCreate;
        public string _updateTitle;
        public string _selectNPC;
        public string _btnUpdate;
        public string _btnUpdateAll;
        public string _btnBack;

        public void InitializeSetup(string language)
        {
            var englishLanguage = LocalizationSettings.AvailableLocales.GetLocale(language);
            _createTitle = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "create_title", englishLanguage);
            _name = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "create_name", englishLanguage);
            _sprite = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "create_sprite", englishLanguage);
            _collider = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "create_collider", englishLanguage);
            _exportPath = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "create_export_path", englishLanguage);
            _baseNPC = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "create_base_npc", englishLanguage);
            _buttonCreate = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "create_button", englishLanguage);
            _updateTitle = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "update_title", englishLanguage);
            _selectNPC = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "update_select_npc", englishLanguage);
            _btnUpdate = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "update_button", englishLanguage);
            _btnUpdateAll = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "update_button_all", englishLanguage);
            _btnBack = LocalizationSettings.StringDatabase.GetLocalizedString("NPCLocalize", "update_button_back", englishLanguage);
        }
    }
}
